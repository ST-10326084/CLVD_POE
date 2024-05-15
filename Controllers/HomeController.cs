using Microsoft.AspNetCore.Mvc;
using KhumaloCraft.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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

        public IActionResult Login() { return View(); }

        public IActionResult MyWork()
        {
            var products = _context.Products.ToList(); // Fetch all Products from the database
            return View(products); // Pass the list of Products to the view
        }
       
        public IActionResult Privacy() { return View(); }

        public IActionResult ProductDetails(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Purchase(int productId, int quantity)
        {
            // Retrieve the product from the database
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            // Check if there is enough stock to fulfill the request
            if (product.Stock < quantity)
            {
                return BadRequest("Not enough stock available.");
            }

            // Decrease the stock count
            product.Stock -= quantity;

            try
            {
                // Save changes to the database
                await _context.SaveChangesAsync();
                return RedirectToAction("MyWork");
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
