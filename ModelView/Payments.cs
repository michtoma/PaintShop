using System.ComponentModel.DataAnnotations;

namespace PaintShopMVC.ModelView
{
    public class Payments
    {
        [Required]
        [StringLength(16, MinimumLength = 16)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Numer karty powinien zawierać tylko cyfry.")]
        public string CardNumber { get; set; }

        [Required]
        [StringLength(2, MinimumLength = 2)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Nieprawidłowy format miesiąca. Podaj dwie cyfry.")]
        public string ExpirationMonth { get; set; }

        [Required]
        [StringLength(2, MinimumLength = 2)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Nieprawidłowy format roku. Podaj dwie cyfry.")]
        public string ExpirationYear { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Nieprawidłowy format CVV. Podaj trzy cyfry.")]
        public string CVV { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu e-mail.")]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string AddressLine1 { get; set; }

        [StringLength(100)]
        public string AddressLine2 { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(10)]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(2, MinimumLength = 2)]
        [RegularExpression("^[A-Z]*$", ErrorMessage = "Nieprawidłowy format kraju. Podaj dwie wielkie litery.")]
        public string Country { get; set; }
    }
}

