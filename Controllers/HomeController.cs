using Microsoft.AspNetCore.Mvc;
using POE_CLVD.Models;
using System.Diagnostics;

namespace POE_CLVD.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index() { return View(); }

        public IActionResult About() { return View(); }

        public IActionResult Contact() { return View(); }

        public IActionResult MyWork() { return View(); }

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
