using Microsoft.AspNetCore.Mvc.Rendering;
using PaintShopMVC.Models.Enums;
using PaintShopMVC.Models.Products;
using System.ComponentModel.DataAnnotations;

namespace PaintShopMVC.ModelView
{
    public class ProductViewModel
    {
        [Required(ErrorMessage = "Pole jest wymagane"), StringLength(50, ErrorMessage = "Maksymalna długość to 50 znaków"), MinLength(3, ErrorMessage = "Minimalna długość to 3 znaki"), Display(Name = "Nazwa")]
        public string Name { get; set; } = string.Empty;
        [Required, Range(0.01, double.MaxValue, ErrorMessage = "Cena musi być większa od zera"), Display(Name = "Cena"), DataType(DataType.Currency)]
        public double Price { get; set; }
        [Display(Name = "Wydajność")]
        public double Efficiency { get; set; }
        [Display(Name ="Grupa produktów")]
        public List<SelectListItem>? ProductCategoryToSelect { get; set; } = new List<SelectListItem>() { new SelectListItem("Farby", "1"), new SelectListItem("Akcesoria", "2"), new SelectListItem("Rozpuszczalniki", "3") };
        public string ProductGroup { get; set; } = string.Empty;
        public List<SelectListItem>? PaintCategoriesSelect { get; set; } = new List<SelectListItem>();
        public List<SelectListItem>? SolventCategoriesSelect { get; set; } = new List<SelectListItem>();
        public List<SelectListItem>? AccessoryCategoriesSelect { get; set; } = new List<SelectListItem>();
        public List<SelectListItem>? SurfacesSelect { get; set; } = new List<SelectListItem>();
        public List<SelectListItem>? PacagesSelect { get; set; } = new List<SelectListItem>();
        [Display(Name = "Powierzchnia")]
        public List<int>? SurfacesSelected { get; set; } = new List<int>();
        [Display(Name = "Kategoria Farby")]
        public List<int>? PaintCategoriesSelected { get; set; } = new List<int>();
        [Display(Name = "Kategoria Rozpuszczalnika")]
        public int? SolventCategoriesSelected { get; set; } 
        [Display(Name = "Kategoria Akcesoriów")]
        public int? AccessoryCategorySelected { get; set; } 
        [Display(Name = "Opakowanie")]
        public int? PacagesSelected { get; set; } 
        [Display(Name = "Zdjęcie")]
        public IFormFile? formFile { get; set; }
        [Display(Name = "Opis produktu"), StringLength(400, ErrorMessage ="Podany tekst jest za długi, maksymalna długość tekstu to 400 znaków")]
        public string Description { get; set; } = string.Empty;
    }
}

