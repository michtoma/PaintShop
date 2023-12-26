using PaintShopMVC.Models.Orders;
using PaintShopMVC.Models.Products;
using System.ComponentModel.DataAnnotations;

namespace PaintShopMVC.Models.Orders
{
    public class OrderItem
    {
        public int Id { get; set; }
        public Product? Product { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public Order? Order { get; set; }
        public int OrderId { get; set; }
        public double TotalItemCost
        {
            get
            {
                return Price * Quantity;
            }
        }

    }
}
