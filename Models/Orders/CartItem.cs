using PaintShopMVC.Models.Products;
using System.ComponentModel.DataAnnotations;

namespace PaintShopMVC.Models.Orders
{
    public class CartItem
    {
        public int Id { get; set; } 
        public Product? Product { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public Cart? Cart { get; set; }
        public int CartId { get; set; }
        public double TotalItemCost
        {
            get
            {
                return Price * Quantity;
            }
        }
    }
}
