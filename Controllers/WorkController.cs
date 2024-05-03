using Microsoft.AspNetCore.Mvc;
using KhumaloCraft.Models;
using System.Collections.Generic;
using System.Linq;

public class ProductController : Controller
{
    private readonly MyDbContext _context;

    public ProductController(MyDbContext context)
    {
        _context = context;
    }

    public IActionResult MyWork()
    {
        // Fetch all products from the database
        List<Product> products = _context.Products.ToList();

        if (products == null || !products.Any())
        {
            return View(new List<Product>());
        }
        // Pass the products to the view
        return View(products);
    }
}
