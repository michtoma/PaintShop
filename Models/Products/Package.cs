using PaintShopMVC.Models.Enums;

namespace PaintShopMVC.Models.Products
{
    public class Package
    {
        public int Id { get; set; }
        public string Name { get {
                return Capacity.ToString() + " " + Unit.ToString();
            } } 
        public double Capacity { get; set; } = 0;
        public Unit Unit { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
