using System.ComponentModel.DataAnnotations;

namespace PaintShopMVC.Models.Products
{
    public class Paint:Product
    {
        [Display(Name = "Wydajność")]
        [Required, Range(0.1, double.MaxValue, ErrorMessage = "Wydajność musi być większa od zera")]
        public double Efficiency { get; set; }
        [Display(Name = "Podłoża")]
        public ICollection<SurfacePaint>? Surfaces { get; set;}
        [Display(Name ="Kategoria produktu")]
        public ICollection<PaintCategoryPaint>? Categories { get; set;}
    }
}
