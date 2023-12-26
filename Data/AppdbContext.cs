using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PaintShopMVC.Models.Products;
using PaintShopMVC.ModelView;
using PaintShopMVC.Models.Accounts;
using PaintShopMVC.Models.Orders;

namespace PaintShopMVC.Data
{
    public class AppdbContext : IdentityDbContext<AppUsers>
    {
        public AppdbContext(DbContextOptions<AppdbContext> options) : base(options)
        {
        }
        public DbSet<Surface> Surfaces { get; set; }
        public DbSet<Paint> Paints { get; set; }
        public DbSet<Solvent> Solvents { get; set; }
        public DbSet<PaintCategory> PaintCategories { get; set; }
        public DbSet<SolventCategory> SolventCategories { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Accessory> Accessories { get; set; }
        public DbSet<PaintCategoryPaint> PaintCategoriesPaint { get; set; }
        public DbSet<SurfacePaint> SurfacePaints { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<AccessoryCategory> AccessoryCategories { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<AppUsers> AppUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
