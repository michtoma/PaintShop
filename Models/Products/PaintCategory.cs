namespace PaintShopMVC.Models.Products
{
    public class PaintCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<PaintCategoryPaint>? Paints { get; set;}

    }
}
