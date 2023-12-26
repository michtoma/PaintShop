using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PaintShopMVC.Models.Orders
{
    public class Delivery
    {
        public int Id { get; set; }
        [Display(Name = "Nazwa")]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Display(Name="Koszt dostawy")]
        [DataType(DataType.Currency)]
        public double Cost { get; set; }
        public bool IsCashOnDelivery { get; set; } = true;
    }
}
