using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KhumaloCraft.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

public class UserController : Controller
{
    private readonly Context _context;
    private const string EmployeePasscode = "0000"; 

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
            return View("Confirmation", new ConfirmationViewModel { Message = "Invalid email or password.", Success = false });
        }

        // Here, you can set up authentication cookies or tokens if needed

        return View("Confirmation", new ConfirmationViewModel { Message = "User logged in successfully.", Success = true });
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegisterUser([FromForm] User user, [FromForm] string Role, [FromForm] string EmployeePasscode)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return BadRequest(string.Join(", ", errors));
        }

        // Check if the user already exists
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == user.UserEmail);
        if (existingUser != null)
        {
            return View("Confirmation", new ConfirmationViewModel { Message = "User with this email already exists.", Success = false });
        }

        // Validate role and passcode
        if (Role == "Employee" && EmployeePasscode != EmployeePasscode)
        {
            return View("Confirmation", new ConfirmationViewModel { Message = "Invalid passcode for employee registration.", Success = false });
        }

        // Set the role
        user.Role = Role;

        // Add the new user to the database
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return View("Confirmation", new ConfirmationViewModel { Message = "User/Employee registered successfully.", Success = true });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        // Sign out the user
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "User");
    }

}
