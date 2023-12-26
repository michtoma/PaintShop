using Microsoft.AspNetCore.Identity;

namespace PaintShopMVC.Models.Accounts
{
    public class AppUsers : IdentityUser
    {
        public string FirstName { get; set; } =string.Empty;
        public string LastName { get; set; } = string.Empty;
        public ICollection<Address> Address { get; set; } = new List<Address>();

    }
}
