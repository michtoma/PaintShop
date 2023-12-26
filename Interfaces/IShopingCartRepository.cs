using PaintShopMVC.Models.Orders;

namespace PaintShopMVC.Interfaces
{
    public interface IShopingCartRepository
    {
        Task <IEnumerable<CartItem>> GetAllCartItemsAsync();
        Task <IEnumerable<Cart>> GetAllCartsAsync();
        Task <IEnumerable<Order>> GetAllOrdersAsync();
        Task <IEnumerable<Delivery>> GetAllDeliveryAsync();
        Task <IEnumerable<Address>> GetUserAddressesAsync(string userId);
        Task <Cart> GetCart(int cartId);
        Task <Order> GetOrder(int orderId);
        Task <Delivery> GetDelivery(int deliveryId);
        Task <Address> GetAddress(int addressId);
        Task <IEnumerable<Cart>> GetUserCarts(string userId);
        Task <Cart> GetCartItem(int id);
        bool Add(Cart item);
        bool Add(Order order);
        bool Add(CartItem item);
        bool Add(Address address);
        bool Update(Cart item);
        bool Update(Order order);
        bool Update(Address address);
        bool Update(CartItem item);
        bool Delete(Cart cart);
        bool Delete(CartItem cartItem);
        bool Delete(Address address);
        bool Save();

    }
}
