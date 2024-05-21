// UserController.cs
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KhumaloCraft.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

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

        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.UserEmail == user.UserEmail && u.PasswordHash == user.PasswordHash);

        if (existingUser == null)
        {
            return View("Confirmation", new ConfirmationViewModel { Message = "Invalid email or password.", Success = false });
        }

        // Create claims
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, existingUser.UserEmail),
        new Claim(ClaimTypes.Role, existingUser.Role)
    };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            // Allow refreshing the authentication session
            AllowRefresh = true,
            // Expire time for the authentication ticket
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
            // Is persistent
            IsPersistent = true
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

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

        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == user.UserEmail);
        if (existingUser != null)
        {
            return View("Confirmation", new ConfirmationViewModel { Message = "User with this email already exists.", Success = false });
        }

        if (Role == "Employee" && EmployeePasscode != EmployeePasscode)
        {
            return View("Confirmation", new ConfirmationViewModel { Message = "Invalid passcode for employee registration.", Success = false });
        }

        user.Role = Role;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return View("Confirmation", new ConfirmationViewModel { Message = "User/Employee registered successfully.", Success = true });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "User");
    }
}

