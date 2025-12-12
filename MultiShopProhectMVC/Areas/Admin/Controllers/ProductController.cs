using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShopProjectMVC.DAL;
using MultiShopProjectMVC.Models;
using MultiShopProjectMVC.Utilities.Enums;
using MultiShopProjectMVC.Utilities.Extensions;
using MultiShopProjectMVC.ViewModels;

namespace MultiShopProjectMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var productsVms = await _context.Products
                .Select(p => new GetAdminProductVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Image = p.Image,
                    CategoryName = p.Category.Name,
                    CategoryId = p.Category.Id
                }).ToListAsync();
            return View(productsVms);
        }
        public async Task<IActionResult> Create()
        {
            CreateProductVM productVM = new()
            {
                Categories = await _context.Categories.ToListAsync()
            };
            return View(productVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM productVM)
        {
            productVM.Categories = await _context.Categories.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View(productVM);
            }
            if (!productVM.Photo.ValidateType("image/"))
            {
                ModelState.AddModelError(nameof(CreateProductVM.Photo), "File type is incorrect");
                return View(productVM);
            }
            if (!productVM.Photo.ValidateSize(FileSize.MB, 1))
            {
                ModelState.AddModelError(nameof(CreateProductVM.Photo), "File size is incorrect");
                return View(productVM);
            }

            bool result = productVM.Categories.Any(c => c.Id == productVM.CategoryId);
            if (!result)
            {
                ModelState.AddModelError(nameof(CreateProductVM.CategoryId), "Category does not exists");
                return View(productVM);
            }
            bool resultName = await _context.Products.AnyAsync(p => p.Name == productVM.Name);
            if (resultName)
            {
                ModelState.AddModelError(nameof(CreateProductVM.Name), "Product name already exists");
                return View(productVM);
            }


            Product product = new()
            {
                Name = productVM.Name,
                Price = productVM.Price.Value,
                Description = productVM.Description,
                CategoryId = productVM.CategoryId.Value,
                Image = await productVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img"),
                CreatedAt = DateTime.Now
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id < 1) return BadRequest();
            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product is null) return NotFound();
            product.Image.DeleteFile(_env.WebRootPath, "assets", "img");
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1)
            {
                return BadRequest();
            }
            Product? existed = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (existed is null) return NotFound();
            UpdateProductVM productVM = new()
            {
                Name = existed.Name,
                Description = existed.Description,
                CategoryId = existed.CategoryId,
                Price = existed.Price,
                Categories = await _context.Categories.ToListAsync(),
                Image = existed.Image
            };
            return View(productVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateProductVM productVM)
        {
            Product? existed = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            productVM.Categories = await _context.Categories.ToListAsync();

            if (!ModelState.IsValid)
            {
                return View(productVM);
            }
            if (productVM.Photo is not null)
            {
                if (!productVM.Photo.ValidateType("image/"))
                {
                    ModelState.AddModelError(nameof(UpdateProductVM.Photo), "File type is incorrect");
                    return View(productVM);
                }
                if (!productVM.Photo.ValidateSize(FileSize.MB, 1))
                {
                    ModelState.AddModelError(nameof(UpdateProductVM.Photo), "File size is incorrect");
                    return View(productVM);
                }
            }
            bool result = productVM.Categories.Any(c => c.Id == productVM.CategoryId);
            if (!result)
            {
                ModelState.AddModelError(nameof(CreateProductVM.CategoryId), "Category does not exists");
                return View(productVM);
            }
            bool resultName = await _context.Products.AnyAsync(p => p.Name == productVM.Name && p.Id != id);
            if (resultName)
            {
                ModelState.AddModelError(nameof(UpdateProductVM.Name), "Product name already exists");
                return View(productVM);
            }
            if (productVM.Photo is not null)
            {
                existed.Image.DeleteFile(_env.WebRootPath, "assets", "img");
                existed.Image = await productVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img");
            }
            existed.Name = productVM.Name;
            existed.Description = productVM.Description;
            existed.Price = productVM.Price.Value;
            existed.CategoryId = productVM.CategoryId.Value;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id < 1)
            {
                return BadRequest();
            }
            Product? existed = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);
            if (existed is null)
            {
                return NotFound();
            }
            DetailsProductVM productVM = new()
            {
                Name = existed.Name,
                Description = existed.Description,
                CategoryId = existed.CategoryId,
                Price = existed.Price,
                Categories = await _context.Categories.ToListAsync(),
                Image = existed.Image
            };
            return View(productVM);
        }
    }
}
