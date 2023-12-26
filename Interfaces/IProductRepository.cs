using Microsoft.EntityFrameworkCore;
using PaintShopMVC.Data;
using PaintShopMVC.Models.Products;

namespace PaintShopMVC.Interfaces
{
    public interface IProductRepository 
    {

        Task<IEnumerable<Product>> GetProductsAsync();
        Task<IEnumerable<Paint>> GetPaintsAsync();
        Task<Product> GetProductAsync(int productId);
        Task<IEnumerable<Paint>> GetPaintsByCategoryAsync(int categoryId);
        Task<IEnumerable<Paint>> GetPaintsBySurfaceAsync(int surfaceId);
        Task<IEnumerable<Paint>> GetPaintsByPackageAsync(int packageId);
        Task<Paint> GetPaintAsync(int? paintId);
        Task<Package> GetPacageAsync(int pacageId);
        Task<PaintCategory> GetPaintCategoryAsync(int catId);
        Task<IEnumerable<PaintCategory>> GetPaintCategoriesAsync();
        Task<IEnumerable<Package>> GetPacagesAsync();
        Task<IEnumerable<Accessory>> GetAccessoriesAsync();
        Task<IEnumerable<Surface>> GetSurfacesAsync();
        Task<IEnumerable<Solvent>> GetSolventsAsync();
        Task<IEnumerable<Solvent>> GetSolventsByCategoryAsync(int categoryId);
        Task<IEnumerable<Solvent>> GetSolventsByPackageAsync(int packageId);
        Task<Solvent> GetSolventAsync(int? id);
        Task<Surface> GetSurfaceAsync(int id);
        Task<SurfacePaint> GetSurfacPainteAsync(int? id);
        Task<PaintCategoryPaint> GetPaintCategoryPaintAsync(int id);
        Task<IEnumerable<SolventCategory>> GetSolventCategoriesAsync();
        Task<IEnumerable<AccessoryCategory>> GetAccessoryCategoriesAsync(); 
        Task<Accessory> GetAccessoryAsync(int? accessoryId);
        bool Add(Paint paint);
        bool Add(Accessory accessory);
        bool Add(Package package);
        bool Add(Solvent solvent);
        bool Add(Surface surface);
        bool Add(SolventCategory solventCategory);
        bool Add(AccessoryCategory accessoryCategory);
        bool Add(PaintCategory paintCategory);
        bool Add(PaintCategoryPaint paintCategory);
        bool Add(SurfacePaint surfacePaint);
        bool Edit(Paint paint);
        bool Edit(Accessory accessory);
        bool Edit(Package package);
        bool Edit(Solvent solvent);
        bool Edit(Surface surface);
        bool Edit(SolventCategory solventCategory);
        bool Edit(PaintCategory paintCategory);
        bool Edit(PaintCategoryPaint paintCategory);
        bool Edit(SurfacePaint surfacePaint);
        bool Delete(Paint paint);
        bool Delete(Accessory accessory);
        bool Delete(Package package);
        bool Delete(Solvent solvent);
        bool Delete(Surface surface);
        bool Delete(SolventCategory solventCategory);
        bool Delete(PaintCategory paintCategory);
        bool Delete(PaintCategoryPaint paintCategory);
        bool Delete(SurfacePaint surfacePaint);
        bool Save();
    }
}
