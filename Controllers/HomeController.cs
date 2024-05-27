using Microsoft.AspNetCore.Mvc;
using KhumaloCraft.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

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

        //[CustomAuthorize("Please log in to access Contact Page.")] i have disabled authorization on contact page so that if there is issues the user can email, and ask for assistance in app
        public IActionResult Contact() { return View(); }

        public IActionResult Settings() { return View(); }

        public IActionResult Login() { return View(); }

        [CustomAuthorize("Please log in to access My Work Page.")]
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

        //[Authorize(Roles = "Employee")] will fix this for part 3
        public IActionResult ManageStock()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        [HttpPost]
        //[Authorize(Roles = "Employee")] will fix for part 3
        public async Task<IActionResult> UpdateStock(int productId, int newStock) 
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            product.Stock = newStock;

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction("ManageStock");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)  // add confirmation to items added to cart
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(item => item.ProductID == productId);

            if (cartItem == null)
            {
                cart.Add(new ShoppingCartItem
                {
                    ProductID = product.ProductID,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Quantity = quantity
                });
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            SaveCart(cart);

            return RedirectToAction("MyWork");
        }

        public IActionResult ViewCart()
        {
            var cart = GetCart();
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout() // add confirmation to items purchased
        {
            var cart = GetCart();
            foreach (var cartItem in cart)
            {
                var product = await _context.Products.FindAsync(cartItem.ProductID);
                if (product == null || product.Stock < cartItem.Quantity)
                {
                    return BadRequest("Not enough stock available for product: " + cartItem.ProductName);
                }

                product.Stock -= cartItem.Quantity;
            }

            try
            {
                await _context.SaveChangesAsync();
                ClearCart();
                return RedirectToAction("MyWork");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        private List<ShoppingCartItem> GetCart()
        {
            var cart = HttpContext.Session.GetString("Cart");
            return cart == null ? new List<ShoppingCartItem>() : JsonConvert.DeserializeObject<List<ShoppingCartItem>>(cart);
        }

        private void SaveCart(List<ShoppingCartItem> cart)
        {
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
        }

        private void ClearCart()
        {
            HttpContext.Session.Remove("Cart");
        }
    }
}

