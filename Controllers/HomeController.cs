using Microsoft.AspNetCore.Mvc;
using KhumaloCraft.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KhumaloCraft.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Context _context;

        public HomeController(ILogger<HomeController> logger, Context context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index() { return View(); }

        public IActionResult About() { return View(); }

        public IActionResult Contact() { return View(); }

        public IActionResult MyWork()
        {
            var products = _context.products.ToList(); // Fetch all products from the database
            return View(products); // Pass the list of products to the view
        }

        public IActionResult product1() { return View(); }

        public IActionResult product2() { return View(); }

        public IActionResult product3() { return View(); }

        public IActionResult Privacy() { return View(); }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
