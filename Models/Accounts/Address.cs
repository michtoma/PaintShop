using Microsoft.AspNetCore.Identity;
using PaintShopMVC.Models.Orders;
using System.ComponentModel.DataAnnotations;

namespace PaintShopMVC.Models.Accounts
{
    public class Address
    {
        public int Id { get; set; }
        public AppUsers? User { get; set; }
        public string? UserId { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "{0} musi mieć co najmniej {2} znaków i max {1} długości.", MinimumLength = 2)]
        [Display(Name = "Imię")]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [StringLength(100, ErrorMessage = "{0} musi mieć co najmniej {2} znaków i max {1} długości.", MinimumLength = 2)]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [StringLength(100, ErrorMessage = "{0} musi mieć co najmniej {2} znaków i max {1} długości.", MinimumLength = 2)]
        [Display(Name = "Ulica")]
        public string Street { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "{0} musi mieć co najmniej {2} znaków i max {1} długości.", MinimumLength = 2)]
        [Display(Name = "Miasto")]
        public string City { get; set; } = string.Empty;

        [Required]
        [StringLength(20, ErrorMessage = "{0} musi mieć co najmniej {2} znaków i max {1} długości.", MinimumLength = 2)]
        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; } = string.Empty;

        [Required]
        [StringLength(50, ErrorMessage = "{0} musi mieć co najmniej {2} znaków i max {1} długości.", MinimumLength = 3)]
        [Display(Name = "Kraj")]
        public string Country { get; set; } = string.Empty;

        [Required]
        [StringLength(10, ErrorMessage = "{0} musi mieć co najmniej {2} znaków i max {1} długości.", MinimumLength = 2)]
        [Display(Name = "Numer domu")]
        public string HouseNumber { get; set; } = string.Empty;

        [StringLength(10, ErrorMessage = "{0} musi mieć co najmniej {2} znaków i max {1} długości.", MinimumLength = 2)]
        [Display(Name = "Numer mieszkania")]
        public string ApartmentNumber { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; } = string.Empty;
        [DataType(DataType.EmailAddress,ErrorMessage ="Wprowadź poprawny adres Email przyklad@pryzklad.przyklad")]
        public string? Email { get; set; } = string.Empty;
        public ICollection<Order>Orders { get; set; } = new List<Order>();
        public string DisplayName
        {
            get
            {
                return LastName + " " + City + " " + Street;
            }
        }
    }
}
