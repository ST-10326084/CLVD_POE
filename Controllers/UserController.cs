using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KhumaloCraft.Models;

public class UserController : Controller
{
    private readonly Context _context;

    public UserController(Context context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> LoginUser([FromForm] User user)
    {
        Console.WriteLine($"UserEmail: {user.UserEmail}, PasswordHash: {user.PasswordHash}");
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return BadRequest(string.Join(", ", errors));
        }

        _context.Users.Add(user); // Add to the DbContext
        await _context.SaveChangesAsync(); // Save the data to the database

        return Ok("User logged in successfully."); // Return a success message
    }
}
//  The PasswordHash field is required. ERROR

