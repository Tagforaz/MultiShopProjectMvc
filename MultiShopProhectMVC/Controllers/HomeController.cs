using Microsoft.AspNetCore.Mvc;

namespace MultiShopProhectMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
