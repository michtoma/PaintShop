using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using PaintShopMVC.Data;
using PaintShopMVC.Interfaces;
using PaintShopMVC.Models.Products;

namespace PaintShopMVC.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppdbContext _context;

        public ProductRepository(AppdbContext context)
        {
            _context = context;
        }
        public bool Add(PaintCategoryPaint paintCategory)
        {
            _context.PaintCategoriesPaint.Add(paintCategory);
            return Save();
        }
        public bool Add(SurfacePaint surfacePaint)
        {
            _context.SurfacePaints.Add(surfacePaint);
            return Save();
        }
        public bool Add(Paint paint)
        {
            _context.Paints.Add(paint);
            return Save();
        }
        public bool Add(Accessory accessory)
        {
            _context.Accessories.Add(accessory);
            return Save();
        }
        public bool Add(Package package)
        {
            _context.Packages.Add(package);
            return Save();
        }
        public bool Add(Solvent solvent)
        {
            _context.Solvents.Add(solvent);
            return Save();
        }
        public bool Add(Surface surface)
        {
            _context.Surfaces.Add(surface);
            return Save();
        }
        public bool Add(SolventCategory solventCategory)
        {
            _context.SolventCategories.Add(solventCategory);
            return Save();
        }
        public bool Add(PaintCategory paintCategory)
        {
            _context.PaintCategories.Add(paintCategory);
            return Save();
        }
        public bool Delete(PaintCategoryPaint paintCategory)
        {
            _context.PaintCategoriesPaint.Remove(paintCategory);
            return Save();
        }
        public bool Delete(SurfacePaint surfacePaint)
        {
            _context.SurfacePaints.Remove(surfacePaint);
            return Save();
        }
        public bool Delete(Paint paint)
        {
            _context.Paints.Remove(paint);
            return Save();
        }
        public bool Delete(Accessory accessory)
        {
            _context.Accessories.Remove(accessory);
            return Save();
        }
        public bool Delete(Package package)
        {
            _context.Packages.Remove(package);
            return Save();
        }
        public bool Delete(Solvent solvent)
        {
            _context.Solvents.Remove(solvent);
            return Save();
        }
        public bool Delete(Surface surface)
        {
            _context.Surfaces.Remove(surface);
            return Save();
        }
        public bool Delete(SolventCategory solventCategory)
        {
            _context.SolventCategories.Remove(solventCategory);
            return Save();
        }
        public bool Delete(PaintCategory paintCategory)
        {
            _context.PaintCategories.Remove(paintCategory);
            return Save();
        }
        public bool Edit(PaintCategoryPaint paintCategory)
        {
            _context.PaintCategoriesPaint.Update(paintCategory);
            return Save();
        }
        public bool Edit(SurfacePaint surfacePaint)
        {
            _context.SurfacePaints.Update(surfacePaint);
            return Save();
        }
        public bool Edit(Paint paint)
        {
            _context.Paints.Update(paint);
            return Save();
        }
        public bool Edit(Accessory accessory)
        {
            _context.Accessories.Update(accessory);
            return Save();
        }
        public bool Edit(Package package)
        {
            _context.Packages.Update(package);
            return Save();
        }
        public bool Edit(Solvent solvent)
        {
            _context.Solvents.Update(solvent);
            return Save();
        }
        public bool Edit(Surface surface)
        {
            _context.Surfaces.Update(surface);
            return Save();
        }
        public bool Edit(SolventCategory solventCategory)
        {
            _context.SolventCategories.Update(solventCategory);
            return Save();
        }
        public bool Edit(PaintCategory paintCategory)
        {
            _context.PaintCategories.Update(paintCategory);
            return Save();
        }
        public async Task<IEnumerable<Accessory>> GetAccessoriesAsync()
        {
            return await _context.Accessories.Include(a => a.Package).ToListAsync();
        }
        public async Task<Accessory> GetAccessoryAsync(int? accessoryId)
        {
            return await _context.Accessories.FirstOrDefaultAsync(a => a.Id == accessoryId);
        }
        public async Task<Package> GetPacageAsync(int pacageId)
        {
            return await _context.Packages.FirstOrDefaultAsync(a => a.Id == pacageId);
        }
        public async Task<IEnumerable<Package>> GetPacagesAsync()
        {
            return await _context.Packages.ToListAsync();
        }
        public async Task<Paint> GetPaintAsync(int? paintId)
        {
            return await _context.Paints.Include(p => p.Categories).ThenInclude(c => c.Category).Include(p => p.Surfaces).ThenInclude(s => s.Surface).Include(p => p.Package).FirstOrDefaultAsync(p => p.Id == paintId);
        }
        public async Task<IEnumerable<PaintCategory>> GetPaintCategoriesAsync()
        {
            return await _context.PaintCategories.ToListAsync();
        }
        public async Task<IEnumerable<Paint>> GetPaintsAsync()
        {
            return await _context.Paints.Include(p => p.Categories).ThenInclude(c => c.Category).Include(p => p.Surfaces).ThenInclude(s => s.Surface).Include(p => p.Package).ToListAsync();
        }
        public async Task<IEnumerable<Paint>> GetPaintsByCategoryAsync(int categoryId)
        {
            return await _context.Paints.Include(p => p.Categories).ThenInclude(c => c.Category).Where(p => p.Categories.Any(c => c.CategoryId == categoryId)).ToListAsync();
        }
        public async Task<IEnumerable<Paint>> GetPaintsByPackageAsync(int packageId)
        {
            return await _context.Paints.Include(p => p.Package).Where(p => p.Package.Id == packageId).ToListAsync();
        }
        public async Task<IEnumerable<Paint>> GetPaintsBySurfaceAsync(int surfaceId)
        {
            return await _context.Paints.Include(p => p.Surfaces).ThenInclude(s => s.Surface).Where(s => s.Surfaces.Any(s => s.SurfaceId == surfaceId)).ToListAsync();
        }
        public async Task<Solvent> GetSolventAsync(int? id)
        {
            return await _context.Solvents.Include(s => s.Category).Include(s => s.Package).FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task<Surface> GetSurfaceAsync(int id)
        {
            return await _context.Surfaces.FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task<IEnumerable<SolventCategory>> GetSolventCategoriesAsync()
        {
            return await _context.SolventCategories.ToListAsync();
        }
        public async Task<IEnumerable<Solvent>> GetSolventsAsync()
        {
            return await _context.Solvents.Include(s => s.Category).Include(s => s.Package).ToListAsync();
        }
        public async Task<IEnumerable<Solvent>> GetSolventsByCategoryAsync(int categoryId)
        {
            return await _context.Solvents.Include(s => s.Category).Where(s => s.Category.Id == categoryId).ToListAsync();
        }
        public async Task<IEnumerable<Solvent>> GetSolventsByPackageAsync(int packageId)
        {
            return await _context.Solvents.Include(s => s.Package).Where(s => s.Id == packageId).ToListAsync();
        }
        public async Task<IEnumerable<Surface>> GetSurfacesAsync()
        {
            return await _context.Surfaces.ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products.Include(p=>p.Package).ToListAsync();
        }
        public async Task<PaintCategory> GetPaintCategoryAsync(int catId)
        {
            return await _context.PaintCategories.FirstOrDefaultAsync(c => c.Id == catId);
        }

        public async Task<IEnumerable<AccessoryCategory>> GetAccessoryCategoriesAsync()
        {
            return await _context.AccessoryCategories.ToListAsync();
        }

        public bool Add(AccessoryCategory accessoryCategory)
        {
            _context.AccessoryCategories.Add(accessoryCategory);
            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public async Task<Product> GetProductAsync(int productId)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<SurfacePaint> GetSurfacPainteAsync(int? id)
        {
            return await _context.SurfacePaints.FindAsync(id);
        }

        public async Task<PaintCategoryPaint> GetPaintCategoryPaintAsync(int id)
        {
            return await _context.PaintCategoriesPaint.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
