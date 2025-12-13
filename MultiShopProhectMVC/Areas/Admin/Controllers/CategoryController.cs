using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShopProjectMVC.DAL;
using MultiShopProjectMVC.Models;
using MultiShopProjectMVC.Utilities.Extensions;
using MultiShopProjectMVC.ViewModels;



namespace MultiShopProjectMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CategoryController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {

            var categoriesVMs = await _context.Categories.Include(c => c.Products).Select(c => new GetCategoryVM
            {
                Id = c.Id,
                Name = c.Name,
                Image = c.Image,
                Products = c.Products.ToList()
            }).ToListAsync();
            return View(categoriesVMs);

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null | id < 1)
            {
                return BadRequest();
            }
            Category? category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category is null)
            {
                return NotFound();
            }
            category.Image.DeleteFile(_env.WebRootPath, "assets", "img");
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id < 1)
            {
                return BadRequest();
            }
            Category? existedCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (existedCategory is null)
            {
                return NotFound();
            }
            DetailsCategoryVM categoryVM = new()
            {
                Name = existedCategory.Name,
                Image = existedCategory.Image
            };
            return View(categoryVM);

        }
        public IActionResult Create()
        {
            CreateCategoryVM categoryVM = new()
            {

            };
            return View(categoryVM);

        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryVM categoryVM)
        {
            if (!ModelState.IsValid)
            {
                return View(categoryVM);
            }
            bool result = await _context.Categories.AnyAsync(c => c.Name == categoryVM.Name);
            if (result)
            {
                ModelState.AddModelError(nameof(CreateCategoryVM.Name), $"{categoryVM.Name} category already exist");
                return View(categoryVM);
            }
            if (!categoryVM.Photo.ValidateType("image/"))
            {
                ModelState.AddModelError(nameof(CreateCategoryVM.Photo), "Invalid type.");
                return View(categoryVM);
            }
            if (!categoryVM.Photo.ValidateSize(Utilities.Enums.FileSize.MB, 2))
            {
                ModelState.AddModelError(nameof(CreateCategoryVM.Photo), "Invalid size.");
                return View(categoryVM);
            }
            Category category = new()
            {
                Name = categoryVM.Name,
                Image = await categoryVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img"),
                CreatedAt = DateTime.Now
            };


            _context.Add(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1)
            {
                return BadRequest();
            }
            Category? existed = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (existed is null)
            {
                return NotFound();
            }
            UpdateCategoryVM categoryVM = new()
            {
                Name = existed.Name,
                Image = existed.Image
            };
            return View(categoryVM);

        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateCategoryVM categoryVM)
        {
            if (id == null || id < 1) return BadRequest();

            if (!ModelState.IsValid)
            {
                return View(categoryVM);
            }

            bool result = await _context.Categories.AnyAsync(c => c.Name == categoryVM.Name && c.Id != id);
            if (result)
            {
                ModelState.AddModelError(nameof(UpdateCategoryVM.Name), $"{categoryVM.Name} category already exist");
                return View(categoryVM);
            }

            if (categoryVM.Photo is not null)
            {
                if (!categoryVM.Photo.ValidateType("image/"))
                {
                    ModelState.AddModelError(nameof(UpdateCategoryVM.Photo), "Invalid type.");
                    return View(categoryVM);
                }
                if (!categoryVM.Photo.ValidateSize(Utilities.Enums.FileSize.MB, 2))
                {
                    ModelState.AddModelError(nameof(UpdateCategoryVM.Photo), "Invalid size.");
                    return View(categoryVM);
                }
            }
            Category? existed = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (existed == null)
            {
                return NotFound();
            }

            existed.Name = categoryVM.Name;
            if (categoryVM.Photo is not null)
            {
                existed.Image.DeleteFile(_env.WebRootPath, "assets", "img");
                existed.Image = await categoryVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img");
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}