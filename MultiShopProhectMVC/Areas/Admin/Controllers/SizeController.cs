using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShopProjectMVC.DAL;
using MultiShopProjectMVC.Models;
using MultiShopProjectMVC.ViewModels;

namespace MultiShopProjectMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SizeController : Controller
    {
        private readonly AppDbContext _context;

        public SizeController(AppDbContext context)
        {
            _context = context;
        }
   
        public async Task<IActionResult> Index()
        {
            List<GetSizeVM> sizeVMs = await _context.Sizes
                .Select(s => new GetSizeVM
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToListAsync();
            return View(sizeVMs);
        }
        
        public IActionResult Create()
        {
            CreateSizeVM sizeVM = new CreateSizeVM();
            return View(sizeVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSizeVM sizeVM)
        {
            if (!ModelState.IsValid)
            {
                return View(sizeVM);
            }

            bool result = await _context.Sizes.AnyAsync(s => s.Name == sizeVM.Name);
            if (result)
            {
                ModelState.AddModelError(nameof(CreateSizeVM.Name), $"Size {sizeVM.Name} already exists.");
                return View(sizeVM);
            }

            Size size = new Size()
            {
                Name = sizeVM.Name
            };

            _context.Sizes.Add(size);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1)
            {
                return BadRequest();
            }

            Size existedSize = await _context.Sizes.FirstOrDefaultAsync(s => s.Id == id);

            if (existedSize is null)
            {
                return NotFound();
            }

            UpdateSizeVM sizeVM = new UpdateSizeVM()
            {
                Name = existedSize.Name
            };

            return View(sizeVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateSizeVM sizeVM)
        {
            if (id is null || id < 1)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(sizeVM);
            }
            bool result = await _context.Sizes.AnyAsync(s => s.Name == sizeVM.Name && s.Id != id);
            if (result)
            {
                ModelState.AddModelError(nameof(UpdateSizeVM.Name), $"Size {sizeVM.Name} already exists.");
                return View(sizeVM);
            }

            Size? existedSize = await _context.Sizes
                .FirstOrDefaultAsync(c => c.Id == id);

            existedSize.Name = sizeVM.Name;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id < 1)
            {
                return BadRequest();
            }
            Size? existedSize = await _context.Sizes.FirstOrDefaultAsync(s => s.Id == id);

            if (existedSize is null)
            {
                return NotFound();
            }

            DetailsSizeVM sizeVM = new()
            {
                Name = existedSize.Name
            };

            return View(sizeVM);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null | id < 1)
            {
                return BadRequest();
            }
            Size? size = await _context.Sizes.FirstOrDefaultAsync(s => s.Id == id);
            if (size is null)
            {
                return NotFound();
            }
            _context.Sizes.Remove(size);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
