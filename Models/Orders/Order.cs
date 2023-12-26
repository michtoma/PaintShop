using PaintShopMVC.Models.Orders;
using System.ComponentModel.DataAnnotations;

namespace PaintShopMVC.Models.Orders
{
    public class Order
    {
        public int Id { get; set; }
        [Display(Name = "Produkty")]
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        [Display(Name = "Użytkownik")]
        public AppUsers? User { get; set; }
        [Required]
        public string UserId { get; set; } = String.Empty;
        [Display(Name = "Opłacone?")]
        public bool IsPaid { get; set; } = false;
        [Display(Name = "Data utworzenia zamówienia")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [Display(Name = "Data opłacenia zamówienia")]
        public DateTime PaymentDate { get; set; }
        [Display(Name ="Adres dostawy")]
        public Address? Address { get; set; }
        public int? AddressId { get; set; }
        [Display(Name ="Dostawca")]
        public Delivery Delivery { get; set; }
        public int DeliveryId { get; set; }
        [Display(Name ="Koszt dostawy")]
        public double DeliveryCost { get; set; }
        [Display(Name = "Wartość koszyka"), DataType(DataType.Currency)]
        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }
        [Display(Name ="Wartość z dostawą")]
        public double TotalOrderValue
        {
            get
            {
                return TotalCartValue + DeliveryCost;
            }
        }
        [Display(Name ="Wartość produktów")]
        public double TotalCartValue
        {
            get
            {
                return (float)Items.Sum(i => i.Quantity * i.Price);
            }
        }

    }
}
