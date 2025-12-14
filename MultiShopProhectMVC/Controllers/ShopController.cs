using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShopProjectMVC.DAL;
using MultiShopProjectMVC.Models;
using MultiShopProjectMVC.ViewModels;

namespace MultiShopProhectMVC.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;

        public ShopController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            ShopVM shopVM = new ShopVM()
            {
                Products = await _context.Products.ToListAsync()
            
            };
            return View(shopVM);
        }
        public async Task<IActionResult> Details(int? id)
        {
         if(id is  null||id<1)
            {
                return BadRequest();
            }   
         Product? product = await _context.Products
                .Include(p=>p.ProductColors)
                .ThenInclude(pc=>pc.Color)
                .Include(ps=>ps.ProductSizes)
                .ThenInclude(ps=>ps.Size)
                .Include(p=>p.Category)
                .FirstOrDefaultAsync(p=>p.Id==id);
            if(product is null)
            {
                return NotFound();
            }
            List<Product> relatedProducts=await _context.Products
                .Where(p=>p.CategoryId==product.CategoryId&& p.Id!=id)
                .ToListAsync();
            DetailsVM detailVM = new DetailsVM()
            {
                Product = product,
                Products = relatedProducts
            };
            return View(detailVM);
        }
    }
}
