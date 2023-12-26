namespace PaintShopMVC.Interfaces
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Address>> GetAllAddressAsync();
        Task<Address> GetAddressAsync(int addressId);
        bool Delete(Address address);
        bool Add(Address address);
        bool Update(Address address);
        bool Save();
        Task<IEnumerable<AppUsers>> GetAllUserAsync();

    }
}
