using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using PaintShopMVC.Interfaces;
using PaintShopMVC.Models.Accounts;
using PaintShopMVC.Models.Orders;
using PaintShopMVC.Models.Products;
using PaintShopMVC.ModelView;
using Stripe;
using Stripe.Checkout;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Address = PaintShopMVC.Models.Accounts.Address;
using Product = PaintShopMVC.Models.Products.Product;

namespace PaintShopMVC.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly UserManager<AppUsers> _userManager;
        private readonly IShopingCartRepository _cartRepo;
        private readonly IProductRepository _prodRepo;
        private readonly IConfiguration _configuration;

        public ShoppingCartController(UserManager<AppUsers> userManager, IShopingCartRepository cartRepo, IProductRepository prodRepo, IConfiguration configuration)
        {
            _userManager = userManager;
            _cartRepo = cartRepo;
            _prodRepo = prodRepo;
            _configuration = configuration;
        }

        /// <summary>
        /// Displays the shopping cart for the user.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var shoppingCarts = await _cartRepo.GetUserCarts(user.Id);
            return View(shoppingCarts);
        }

        /// <summary>
        /// Adds a product to the shopping cart.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int id, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            var shoppingCarts = await _cartRepo.GetUserCarts(user.Id);

            var shoppingCart = shoppingCarts.FirstOrDefault(s => s.IsEditable);
            if (shoppingCart == null)
            {
                shoppingCart = new Cart { UserId = user.Id };
                _cartRepo.Add(shoppingCart);
            }

            var product = await _prodRepo.GetProductAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var cartItem = shoppingCart.Items.FirstOrDefault(i => i.ProductId == id);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ProductId = id,
                    Price = product.Price,
                    ProductName = product.Name,
                    Quantity = quantity,
                    CartId = shoppingCart.Id
                };

                shoppingCart.Items.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            if (_cartRepo.Save())
            {
                TempData["Powiadomienie"] = "Dodano do koszyka";
            }
            else
            {
                TempData["Powiadomienie"] = "Błąd";
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> Increase(int shoppingCartId, int itemId)
        {
            Cart shoppingCart = await _cartRepo.GetCart(shoppingCartId);
            var cartItem = shoppingCart.Items.FirstOrDefault(s => s.Id == itemId);
            cartItem.Quantity = cartItem.Quantity + 1;
            if (_cartRepo.Save()) TempData["Powiadomienie"] = "Zwiększono ilość";
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> Decrease(int shoppingCartId, int itemId)
        {
            Cart shoppingCart = await _cartRepo.GetCart(shoppingCartId);
            var cartItem = shoppingCart.Items.FirstOrDefault(s => s.Id == itemId);
            if (cartItem.Quantity <= 1)
            {
                shoppingCart.RemoveItem(cartItem.Id);
            }
            else
            {
                cartItem.Quantity = cartItem.Quantity - 1;
            }

            if (_cartRepo.Save()) TempData["Powiadomienie"] = "Zmniejszono ilość";
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> Remove(int shoppingCartId, int itemId)
        {
            Cart shoppingCart = await _cartRepo.GetCart(shoppingCartId);
            var cartItem = shoppingCart.Items.FirstOrDefault(s => s.Id == itemId);
            shoppingCart.RemoveItem(cartItem.Id);
            if (_cartRepo.Save()) TempData["Powiadomienie"] = "Usunięto z koszyka";
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var shoppingCarts = await _cartRepo.GetUserCarts(user.Id);
            foreach (var cart in shoppingCarts)
            {
                cart.IsEditable = false;
            }
            var shoppingCart = shoppingCarts.FirstOrDefault(c => c.Id == id);
            shoppingCart.IsEditable = true;
            _cartRepo.Save();
            return View(shoppingCart);
        }
        public async Task<IActionResult> OrderDetails(int id)
        {
            var order = await _cartRepo.GetOrder(id);
            return View(order);
        }

        public async Task<IActionResult> ConfirmCart(int id)
        {
            string changeCommunicate = string.Empty;
            var shoppingCart = await _cartRepo.GetCart(id);

            List<CartItem> updatedItems = new List<CartItem>();

            foreach (var item in shoppingCart.Items)
            {
                Product product = await _prodRepo.GetProductAsync(item.ProductId);
                if (product == null || product.Quantity < 1)
                {
                    changeCommunicate += $"usunięto {item.ProductName} z powodu braku produktu | \n";
                }
                else
                {
                    CartItem updatedItem = item;

                    if (item.Price != product.Price)
                    {
                        updatedItem.Price = product.Price;
                        changeCommunicate += $"zmieniono cenę {item.ProductName} nowa cena: {item.Price} | \n";
                    }

                    if (item.Quantity > product.Quantity)
                    {
                        updatedItem.Quantity = product.Quantity;
                        changeCommunicate += $"zmieniono ilość {item.ProductName} z powodu braku dostępnej ilości | \n";
                    }

                    if (item.ProductName != product.Name)
                    {
                        changeCommunicate += $"produkt {item.ProductName} zmienił nazwę na {product.Name} |\n";
                        updatedItem.ProductName = product.Name;
                    }

                    updatedItems.Add(updatedItem);
                }
            }

            shoppingCart.Items = updatedItems;

            if (shoppingCart.Items.Count == 0)
            {
                _cartRepo.Delete(shoppingCart);
                return RedirectToAction("Index");
            }
            else if (_cartRepo.Save())
            {
                TempData["Powiadomienie"] = changeCommunicate;
            }
            else
            {
                TempData["Powiadomienie"] = "Koszyk wczytano bez zmian.";
            }
            return View(shoppingCart);
        }
        public async Task<IActionResult> CreateOrder(int cartId)
        {
            TempData["Powiadomienie"] = "Zamówienie przekazane do realizacji";

            OrderViewModel orderView = new OrderViewModel();
            var cart = await _cartRepo.GetCart(cartId);
            var addresses = await _cartRepo.GetUserAddressesAsync(cart.UserId);
            var deliveries = await _cartRepo.GetAllDeliveryAsync();
            orderView.CartId = cartId;
            if (addresses.Any())
                foreach (var address in addresses)
                {
                    orderView.AddressesToSelect.Add(new SelectListItem(
                        $"{address.City} {address.Street} {address.HouseNumber}/{address.ApartmentNumber}", address.Id.ToString()));
                }
            if (deliveries.Any())
                foreach (var deliver in deliveries)
                {
                    orderView.DeliveriesToSelect.Add(new SelectListItem(
                        $"{deliver.Name}  {deliver.Cost.ToString("0.00")} zł", deliver.Id.ToString()
                        ));
                }
            orderView.Items = cart.Items;
            orderView.UserId = cart.UserId;
            return View(orderView);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder(OrderViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            Order order = new Order();
            if (model.AddressId == null)
            {
                Address address = new Address();
                address = model.Address;
                address.UserId = user.Id;
                if (address.Email == null) address.Email = user.Email;
                if (address.PhoneNumber == null) address.PhoneNumber = user.PhoneNumber;
                if (_cartRepo.Add(address)) TempData["Powiadomienie"] = "Dodano adres";
                order.Address = address;
            }
            else order.AddressId = model.AddressId;
            order.DeliveryId = model.DeliveryId;
            var delivery = await _cartRepo.GetDelivery(order.DeliveryId);
            order.DeliveryCost = delivery.Cost;
            Cart cart = await _cartRepo.GetCart(model.CartId);
            foreach (var item in cart.Items)
            {
                order.Items.Add(new OrderItem
                {
                    ProductName = item.ProductName,
                    ProductId = item.ProductId,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    OrderId = order.Id

                });
            }
            
            order.UserId = user.Id;
            if (_cartRepo.Add(order))
            {
                TempData["Powiadomienie"] = "Poprawnie zapisano zamówienie";
                _cartRepo.Delete(cart);
                return RedirectToAction("OrderList");

            }
            else
            {
                var addresses = await _cartRepo.GetUserAddressesAsync(cart.UserId);
                var deliveries = await _cartRepo.GetAllDeliveryAsync();
                if (addresses.Any())
                    foreach (var address in addresses)
                    {
                        model.AddressesToSelect.Add(new SelectListItem(
                            $"{address.City} {address.Street} {address.HouseNumber}/{address.ApartmentNumber}", address.Id.ToString()));
                    }
                if (deliveries.Any())
                    foreach (var deliver in deliveries)
                    {
                        model.DeliveriesToSelect.Add(new SelectListItem(
                            $"{deliver.Name}  {deliver.Cost.ToString("0.00")} zł", deliver.Id.ToString()
                            ));
                    }
                model.Items = cart.Items;
                model.UserId = cart.UserId;

                return View(model);
            }
        }
        public async Task<IActionResult> OrderList()
        {
            var user = await _userManager.GetUserAsync(User);
            var orders = await _cartRepo.GetAllOrdersAsync();
            if (User.IsInRole("Admin")) 
                return View(orders);
            else
                return View(orders.Where(o => o.UserId == user.Id));
        }
        public async Task<IActionResult> OrderConfirmation(int id)
        {
            Order order = await _cartRepo.GetOrder(id);
            var service = new SessionService();
            Session session = service.Get(order.SessionId);
            //sprawdzenie czy płatność została zrealizowana w tym miejscu pozwala uniknąć oszustw
            if (session.PaymentStatus.ToLower() == "paid")
            {
                order.IsPaid = true;
                order.PaymentDate = DateTime.Now;
                foreach (var item in order.Items)
                {
                    var product =await _prodRepo.GetProductAsync(item.ProductId);
                    product.Quantity -= item.Quantity;
                    _prodRepo.Save();
                }
            }

            return View(id);
        }
        public async Task<IActionResult> CreatePayment(int id)
        {
            Order order = await _cartRepo.GetOrder(id);
            var domain = "https://localhost:7235/";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card", "blik","p24"
                },
                LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    SuccessUrl =domain+ $"shoppingCart/OrderConfirmation?id={order.Id}",
                    CancelUrl = domain +$"shoppingCart/OrderList",
            }; 
            foreach (var item in order.Items)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),
                        Currency = "pln",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.ProductName,
                        },

                    },
                    Quantity = item.Quantity,
                };
                options.LineItems.Add(sessionLineItem);
            }
            var sesionLineItemDelivery = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(order.Delivery.Cost * 100),
                    Currency = "pln",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = order.Delivery.Name,
                    },

                },
                Quantity = 1,


            };
            options.LineItems.Add (sesionLineItemDelivery);
            var service = new SessionService();
            Session session = service.Create(options);
            order.SessionId = session.Id;
            order.PaymentIntentId = session.PaymentIntentId;
            _cartRepo.Update(order);
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
        public async Task<IActionResult> CreateOrderPdfDocument(int id)
        {
            var Renderer = new IronPdf.ChromePdfRenderer();
            using var PDF =Renderer.RenderUrlAsPdf($"https://localhost:7235/ShoppingCart/OrderDetails/{@id}");
            var contentLenght = PDF.BinaryData.Length;
            Response.Headers["Content-Lenght"] = contentLenght.ToString();
            Response.Headers.Add("Content-Disposition", "inline; filename=Document_" + id + ".pdf");

            return File(PDF.BinaryData, "application/pdf;");

            //TODO: Inny sposób generowania faktury
        }
        
    }
}