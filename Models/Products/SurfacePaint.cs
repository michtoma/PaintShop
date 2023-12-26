namespace PaintShopMVC.Models.Products
{
    public class SurfacePaint
    {
        public int Id { get; set; }
        public Surface Surface { get; set; }
        public int SurfaceId { get; set; }
        public Paint Paint { get; set; }
        public int PaintId { get; set; }
    }
}
