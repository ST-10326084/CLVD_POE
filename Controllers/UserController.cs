using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KhumaloCraft.Models;
using Microsoft.EntityFrameworkCore;

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
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return BadRequest(string.Join(", ", errors));
        }

        // Check if user exists in the database
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.UserEmail == user.UserEmail && u.PasswordHash == user.PasswordHash);

        if (existingUser == null)
        {
            return Unauthorized("Invalid email or password.");
        }

        // Here, you can set up authentication cookies or tokens if needed

        return Ok("User logged in successfully.");
    }
}
