using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using PaintShopMVC.Data;
using PaintShopMVC.Interfaces;
using PaintShopMVC.Models.Enums;
using PaintShopMVC.Models.Products;
using PaintShopMVC.ModelView;

namespace PaintShopMVC.Controllers
{
        [Authorize]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _repo;
        private readonly IWebHostEnvironment _webHost;
        public ProductsController(IProductRepository repo, IWebHostEnvironment webHost)
        {
            _repo = repo;
            _webHost = webHost;
        }
        [HttpGet]
        public async Task<IActionResult> ProductList()
        {
            var products = await _repo.GetProductsAsync();
            return View(products);
        }
        public async Task<IActionResult> PaintList()
        {
            var products = await _repo.GetPaintsAsync();
            return View(products);
        }
        public async Task<IActionResult> AccessoriesList()
        {
            var products = await _repo.GetAccessoriesAsync();
            return View(products);
        }
        public async Task<IActionResult> SolventsList()
        {
            var products = await _repo.GetSolventsAsync();
            return View(products);
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> AddProduct()
        {
            ProductViewModel model = new ProductViewModel();
            var surfaces = await _repo.GetSurfacesAsync();
            var packages = await _repo.GetPacagesAsync();
            var paintCategories = await _repo.GetPaintCategoriesAsync();
            var solventCategories = await _repo.GetSolventCategoriesAsync();
            var accesoryCategories = await _repo.GetAccessoryCategoriesAsync();
            if (surfaces.Any())
                foreach (var surface in surfaces)
                {
                    model.SurfacesSelect.Add(new SelectListItem(surface.Name, surface.Id.ToString()));
                }
            if (packages.Any())
                foreach (var pack in packages)
                {
                    model.PacagesSelect.Add(new SelectListItem(pack.Name, pack.Id.ToString()));
                }
            if (paintCategories.Any())
                foreach (var cat in paintCategories)
                {
                    model.PaintCategoriesSelect.Add(new SelectListItem(cat.Name, cat.Id.ToString()));
                }
            if (solventCategories.Any())
                foreach (var cat in solventCategories)
                {
                    model.SolventCategoriesSelect.Add(new SelectListItem(cat.Name, cat.Id.ToString()));
                }
            if (accesoryCategories.Any())
                foreach (var cat in accesoryCategories)
                {
                    model.AccessoryCategoriesSelect.Add(new SelectListItem(cat.Name, cat.Id.ToString()));
                }
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Coś poszło nie tak, prawidłowo uzupełnij wszystkie pola";
                return View(model);
            }
            if (model.ProductGroup == "1")
            {
                Paint product = new Paint();
                product.Name = model.Name;
                product.Price = model.Price;
                product.Efficiency = model.Efficiency;
                product.Description = model.Description;
                product.PackageId = (int)model.PacagesSelected;
                product.Img = UploadedFile(model);
                if (_repo.Add(product)) TempData["Powiadomienie"] = "Dodano Produkt";
                else TempData["Error"] = "Błąd podczas zapisu w bazie danych";
                if (model.PaintCategoriesSelected.Any())
                    for (int i = 0; i < model.PaintCategoriesSelected.Count; i++)
                    {
                        PaintCategoryPaint cat = new PaintCategoryPaint();
                        cat.PaintId = product.Id;
                        cat.CategoryId = model.PaintCategoriesSelected[i];
                        _repo.Add(cat);
                    }
                if (model.SurfacesSelected.Any())
                    for (int i = 0; i < model.SurfacesSelected.Count; i++)
                    {
                        SurfacePaint surfacePaint = new SurfacePaint();

                        surfacePaint.PaintId = product.Id;
                        surfacePaint.SurfaceId = model.SurfacesSelected[i];
                        _repo.Add(surfacePaint);
                    }

                return RedirectToAction("ProductList");
            }
            if (model.ProductGroup == "2")
            {
                Accessory product = new Accessory();
                product.Name = model.Name;
                product.Price = model.Price;
                product.Description = model.Description;
                product.PackageId = (int)model.PacagesSelected;
                product.CategoryId = (int)model.AccessoryCategorySelected;

                product.Img = UploadedFile(model);
                _repo.Add(product);

                return RedirectToAction("ProductList");
            }
            if (model.ProductGroup == "3")
            {
                Solvent product = new Solvent();
                product.Name = model.Name;
                product.Price = model.Price;
                product.PackageId = (int)model.PacagesSelected;
                product.CategoryId = (int)model.SolventCategoriesSelected;
                product.Img = UploadedFile(model);
                _repo.Add(product);

                return RedirectToAction("ProductList");
            }
            return View(model);
            //TODO: W widoku przy wyborze lepsze będą checkbox

        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _repo.GetProductAsync(id);

            if (product is Paint)
            {
                return RedirectToAction("PaintDetails", new { id = id });
            }
            else if (product is Solvent)
            {
                return RedirectToAction("SolventDetails", new { id = id });
            }
            else if (product is Accessory)
            {
                return RedirectToAction("AccessoryDetails", new { id = id });
            }

            return NotFound();
        }
        public async Task<IActionResult> PaintDetails(int id)
        {
            if (id == null || await _repo.GetPaintAsync(id) == null)
            {
                return NotFound();
            }

            var paint = await _repo.GetPaintAsync(id);

            if (paint == null)
            {
                return NotFound();
            }

            return View(paint);
        }
        public async Task<IActionResult> SolventDetails(int id)
        {
            if (id == null || await _repo.GetSolventAsync(id) == null)
            {
                return NotFound();
            }

            var solvent = await _repo.GetSolventAsync(id);

            if (solvent == null)
            {
                return NotFound();
            }

            return View(solvent);
        }
        public async Task<IActionResult> AccessoryDetails(int id)
        {
            if (id == null || await _repo.GetAccessoryAsync(id) == null)
            {
                return NotFound();
            }

            var product = await _repo.GetAccessoryAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _repo.GetProductAsync(id);

            if (product is Paint)
            {
                return RedirectToAction("PaintEdit", new { id = id });
            }
            else if (product is Solvent)
            {
                return RedirectToAction("SolventEdit", new { id = id });
            }
            else if (product is Accessory)
            {
                return RedirectToAction("AccessoryEdit", new { id = id });
            }

            return NotFound();
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PaintEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paint = await _repo.GetPaintAsync(id);
            if (paint == null)
            {
                return NotFound();
            }

            ViewBag.PackageList = new SelectList(await _repo.GetPacagesAsync(), "Id", "Name", paint.PackageId);
            ViewBag.SurfaceList = new SelectList(await _repo.GetSurfacesAsync(), "Id", "Name");
            ViewBag.CategoryList = new SelectList(await _repo.GetPaintCategoriesAsync(), "Id", "Name");

            return View(paint);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PaintEdit(int id, [Bind("Id,Name,Price,Quantity,Description,IsPromoted,IsActive,PackageId,Efficiency,Img")] Paint product, int[] selectedSurfaces, int[] selectedCategories)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    _repo.Edit(product);
                    product = await _repo.GetPaintAsync(id);

                    // Update selected surfaces
                    if (selectedSurfaces != null)
                    {
                        //removing old relations
                        if (product.Surfaces != null)
                        {
                            foreach (var old in product.Surfaces)
                            {
                                _repo.Delete(old);
                            }
                        }
                        // creating new relations
                        foreach (var newId in selectedSurfaces)
                        {
                            SurfacePaint surfacePaint = new SurfacePaint();
                            surfacePaint.SurfaceId = newId;
                            surfacePaint.PaintId = product.Id;
                            _repo.Add(surfacePaint);
                        }
                    }

                    // Update selected categories
                    if (selectedCategories != null)
                    {
                        //removing old relations
                        if (product.Categories != null)
                        {
                            foreach (var old in product.Categories)
                            {

                                PaintCategoryPaint toDelete=await _repo.GetPaintCategoryPaintAsync(old.Id);
                                _repo.Delete(toDelete);
                            }
                        }
                        // creating new relations
                        foreach (var newId in selectedCategories)
                        {
                            PaintCategoryPaint category = new PaintCategoryPaint();
                            category.CategoryId = newId;
                            category.PaintId = product.Id;
                            _repo.Add(category);
                        }
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = product.Id });
            }

            ViewBag.PackageList = new SelectList(await _repo.GetPacagesAsync(), "Id", "Name", product.PackageId);
            ViewBag.SurfaceList = new SelectList(await _repo.GetSurfacesAsync(), "Id", "Name");
            ViewBag.CategoryList = new SelectList(await _repo.GetPaintCategoriesAsync(), "Id", "Name");

            return View(product);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AccessoryEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _repo.GetAccessoryAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.PackageList = new SelectList(await _repo.GetPacagesAsync(), "Id", "Name", product.PackageId);
            ViewBag.CategoryList = new SelectList(await _repo.GetAccessoryCategoriesAsync(), "Id", "Name", product.CategoryId);

            return View(product);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]    
        public async Task<IActionResult> AccessoryEdit(int id, [Bind("Id,Name,Price,Quantity,Description,IsPromoted,IsActive,PackageId,Img,CategoryId")] Accessory product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repo.Edit(product);
                 
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = product.Id });
            }

            ViewBag.PackageList = new SelectList(await _repo.GetPacagesAsync(), "Id", "Name", product.PackageId);
            ViewBag.CategoryList = new SelectList(await _repo.GetAccessoryCategoriesAsync(), "Id", "Name", product.CategoryId);

            return View(product);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SolventEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _repo.GetSolventAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.PackageList = new SelectList(await _repo.GetPacagesAsync(), "Id", "Name", product.PackageId);
            ViewBag.CategoryList = new SelectList(await _repo.GetSolventCategoriesAsync(), "Id", "Name", product.CategoryId);

            return View(product);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]    
        public async Task<IActionResult> SolventEdit(int id, [Bind("Id,Name,Price,Quantity,Description,IsPromoted,IsActive,PackageId,Img, CategoryId")] Solvent product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repo.Edit(product);
                 
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = product.Id });
            }

            ViewBag.PackageList = new SelectList(await _repo.GetPacagesAsync(), "Id", "Name", product.PackageId);
            ViewBag.CategoryList = new SelectList(await _repo.GetSolventCategoriesAsync(), "Id", "Name", product.CategoryId);

            return View(product);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddPackage()
        {
            
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddPackage(Package package)
        {
            if(ModelState.IsValid)
            {
            _repo.Add(package);
            return Redirect("AddProduct");
            }
            return View(package);

        }
        public async Task<IActionResult> AddSurface()
        {
            
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddSurface(Surface surface)
        {
            if(ModelState.IsValid)
            {
            _repo.Add(surface);
            return Redirect("AddProduct");
            }
            return View(surface);

        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddPaintCategory()
        {
            
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPaintCategory(PaintCategory paintCategory)
        {
            if(ModelState.IsValid)
            {
            _repo.Add(paintCategory);
            return Redirect("AddProduct");
            }
            return View(paintCategory);

        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAccessoryCategory()
        {
            
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAccessoryCategory(AccessoryCategory Category)
        {
            if(ModelState.IsValid)
            {
            _repo.Add(Category);
            return Redirect("AddProduct");
            }
            return View(Category);

        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddSolventCategory()
        {
            
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddSolventCategory(SolventCategory Category)
        {
            if(ModelState.IsValid)
            {
            _repo.Add(Category);
            return Redirect("AddProduct");
            }
            return View(Category);

        }
        private bool ProductExists(int id)
        {
            if( _repo.GetProductAsync(id)!=null)
            
            return true;

            return false;
        }



        private string UploadedFile(ProductViewModel product)
        {
            string fileName = "logo.jpg";
            if (product.formFile != null)
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "IMG");
                fileName = Guid.NewGuid().ToString() + "_" + product.formFile.FileName;
                string filePath = Path.Combine(uploadsFolder, fileName);
                using (var fileSream = new FileStream(filePath, FileMode.Create))
                {
                    product.formFile.CopyTo(fileSream);
                }
            }
            return fileName;
        }
    }
}
