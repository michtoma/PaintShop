namespace PaintShopMVC.Models.Products
{
    public class PaintCategoryPaint
    {
        public int Id { get; set; }
        public Paint Paint { get; set; }
        public int PaintId { get; set; }
        public PaintCategory Category { get; set; }
        public int CategoryId { get; set; }
    }
}
