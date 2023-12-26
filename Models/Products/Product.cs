using PaintShopMVC.Models.Orders;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaintShopMVC.Models.Products
{
    public abstract class Product
    {
        public int Id { get; set; }
        [Display(Name = "Obraz")]
        public string Img { get; set; } = "~/img/paint-g310fb8932_1280.png";
        [Required(ErrorMessage = "Pole jest wymagane"), StringLength(50, ErrorMessage = "Maksymalna długość to 50 znaków"), MinLength(3, ErrorMessage = "Minimalna długość to 3 znaki"), Display(Name = "Nazwa")]
        public string Name { get; set; } = string.Empty;
        [Required, Range(0.01, double.MaxValue, ErrorMessage = "Cena musi być większa od zera"), Display(Name = "Cena"), DataType(DataType.Currency)]
        public double Price { get; set; }
        [Display(Name = "Opakowanie")]
        public Package? Package { get; set; }
        public int PackageId { get; set; }
        [Display(Name = "Ilość")]
        public int Quantity { get; set; } = 0;
        [Display(Name = "Opis produktu"), StringLength(400, ErrorMessage = "Podany tekst jest za długi, maksymalna długość tekstu to 400 znaków")]
        public string Description { get; set; } = string.Empty;
        public bool IsAvailable
        {
            get { return Quantity > 0; }
        }
        public bool IsPromoted { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public ICollection<CartItem>? CartItems { get; set; }
    }
}
