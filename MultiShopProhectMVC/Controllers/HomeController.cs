using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShopProjectMVC.DAL;
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
            HomeVM homeVM = new HomeVM()
            {
                Products=await _context.Products.ToListAsync(),
                Categories=await _context.Categories.Include(c=>c.Products).ToListAsync()
            };
            return View(homeVM);
        }
    }
}
