
namespace PaintShopMVC.Models.Products
{
    public class Surface
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<SurfacePaint>? Paints { get; set; }

    }
}
