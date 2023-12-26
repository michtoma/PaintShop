using System.ComponentModel.DataAnnotations;

namespace PaintShopMVC.Models.Products
{
    public class Solvent:Product
    {
        [Display(Name ="Kategoria")]
        public SolventCategory? Category { get; set; } 
        public int CategoryId { get; set; }

    }
}
