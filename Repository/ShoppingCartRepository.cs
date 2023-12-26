using Microsoft.EntityFrameworkCore;
using PaintShopMVC.Data;
using PaintShopMVC.Interfaces;
using PaintShopMVC.Models.Orders;

namespace PaintShopMVC.Repository
{
    public class ShoppingCartRepository : IShopingCartRepository
    {
        private readonly AppdbContext _context;

        public ShoppingCartRepository(AppdbContext context)
        {
            _context = context;
        }

        public bool Add(Cart item)
        {
            _context.Carts.Add(item);
            return Save();
        }

        public bool Add(CartItem item)
        {
            _context.CartItems.Add(item);
            return Save();
        }

        public bool Add(Address address)
        {
            _context.Addresses.Add(address);
            return Save();
        }

        public bool Add(Order order)
        {
            _context.Orders.Add(order);
            return Save();
        }

        public bool Delete(Cart cart)
        {
            _context.Remove(cart);
            return Save();
        }

        public bool Delete(CartItem cartItem)
        {
            _context.Remove(cartItem);
            return Save();
        }

        public bool Delete(Address address)
        {
            _context.Remove(address);
            return Save();
        }

        public async Task<Address> GetAddress(int addressId)
        {
            return await _context.Addresses.FirstOrDefaultAsync(c => c.Id == addressId);
        }

        public Task<IEnumerable<CartItem>> GetAllCartItemsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CartItem>> GetAllCartsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Delivery>> GetAllDeliveryAsync()
        {
            return await _context.Deliveries.ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.Include(o=>o.Items).Include(o=>o.Address).Include(o=>o.Delivery).ToListAsync();
        }

        public async Task<Cart> GetCart(int id)
        {
            return await _context.Carts.Include(c=>c.Items).FirstOrDefaultAsync(c => c.Id == id);
        }

        public Task<Cart> GetCartItem(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Delivery> GetDelivery(int deliveryId)
        {
            return await _context.Deliveries.FirstOrDefaultAsync(c => c.Id == deliveryId);
        }

        public async Task<Order> GetOrder(int orderId)
        {
            return await _context.Orders.Include(c => c.Items).Include(c => c.Address).Include(c=>c.User).Include(c=>c.Delivery).FirstOrDefaultAsync(c => c.Id == orderId);
        }

        public async Task<IEnumerable<Address>> GetUserAddressesAsync(string userId)
        {
            return await _context.Addresses.Where(a=>a.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Cart>> GetUserCarts(string userId)
        {
           return await _context.Carts.Include(c=>c.Items).Where(c=>c.UserId == userId).ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool Update(Cart item)
        {
            _context.Update(item);
            return Save();
        }

        public bool Update(CartItem item)
        {
            _context.Update(item);
            return Save();
        }

        public bool Update(Address address)
        {
            _context.Update(address);
            return Save();
        }

        public bool Update(Order order)
        {
            _context.Update(order);
            return Save();
        }

        Task<IEnumerable<Cart>> IShopingCartRepository.GetAllCartsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
