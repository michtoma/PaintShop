namespace PaintShopMVC.Models.Products
{
    public class AccessoryCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Accessory>? Accessories { get; set; }
    }
}
