using Microsoft.AspNetCore.Identity;
using PaintShopMVC.Models.Accounts;
using System.ComponentModel.DataAnnotations;

namespace PaintShopMVC.Models.Orders
{
    public class Cart
    {
        public int Id { get; set; }
        [Display(Name = "Produkty")]
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        [Display(Name ="Użytkownik")]
        public AppUsers? User { get; set; }
        [Required]
        public string UserId { get; set; } = String.Empty;
        [Display(Name = "Można edytować?")]
        public bool IsEditable { get; set; } = true;
        [Display(Name = "Data utworzenia koszyka")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [Display(Name = "Data utworzenia zamówienia")]
        public DateTime OrderDate { get; set; }
        [Display(Name ="Adres dostawy")]
        public Address? Address { get; set; }
        public int? AddressId { get; set; }
        [Display(Name ="Wartość koszyka"), DataType(DataType.Currency)]
        public double TotalCartValue
        {
            get
            {
                return (float) Items.Sum(i => i.Quantity * i.Price);
            }
        }
        public void RemoveItem(int id)
        {
            var item = Items.FirstOrDefault(i => i.Id == id);
            if (item != null)
            {
                Items.Remove(item);
            }
        }
    }
}
