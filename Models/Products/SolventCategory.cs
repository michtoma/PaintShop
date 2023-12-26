using System.ComponentModel.DataAnnotations;

namespace PaintShopMVC.Models.Products
{
    public class SolventCategory
    {
        public int Id { get; set; }
        [Required, StringLength(50), MinLength(3), Display(Name = "Nazwa")]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<Solvent>? Solvents { get; set; }
    }
}
