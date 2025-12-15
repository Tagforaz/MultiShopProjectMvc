using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShopProjectMVC.DAL;
using MultiShopProjectMVC.Models;
using MultiShopProjectMVC.ViewModels;

namespace MultiShopProhectMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.Products.ToListAsync();

            if (products.Count > 8)
            {
                products = await _context.Products.Take(8).ToListAsync();
            }

            HomeVM homeVM = new HomeVM()
            {
                Products = products,
                Categories = await _context.Categories.Include(c => c.Products).ToListAsync()
            };
            return View(homeVM);
        }
    }
}
