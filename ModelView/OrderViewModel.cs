using Microsoft.AspNetCore.Mvc.Rendering;
using PaintShopMVC.Models.Orders;
using System.ComponentModel.DataAnnotations;

namespace PaintShopMVC.ModelView
{
    public class OrderViewModel
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public List<SelectListItem>? AddressesToSelect { get; set; } = new List<SelectListItem>() { new SelectListItem("Dodaj Nowy", "nd") };
        public List<SelectListItem>? DeliveriesToSelect { get; set; } = new List<SelectListItem>();
        public AppUsers? User { get; set; }
        public int OrderId { get; set; }
        public int CartId { get; set; }
        public int DeliveryId { get; set; }
        public double DeliveryCost { get; set; }

        [Required]
        public string UserId { get; set; } = String.Empty;
        [Display(Name = "Można edytować?")]
        public bool IsEditable { get; set; } = true;
        [Display(Name = "Data utworzenia koszyka")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [Display(Name = "Data utworzenia zamówienia")]
        public DateTime OrderDate { get; set; }
        public Address? Address { get; set; }
        public int? AddressId { get; set; }
        [Display(Name = "Wartość koszyka"), DataType(DataType.Currency)]
        public double TotalCartValue
        {
            get
            {
                return (float)Items.Sum(i => i.Quantity * i.Price);
            }
        }
    }
}
