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
    private const string EmployeePasscode = "0000"; // for accessing the employee role
    

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

        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == user.UserEmail && u.PasswordHash == user.PasswordHash);

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

        var userClaims = User.Claims;
        foreach (var claim in userClaims)
        {
            Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
        }

        return View("Confirmation", new ConfirmationViewModel { Message = "User logged in successfully.", Success = true });
    }



    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }



    [HttpPost]
    public async Task<IActionResult> RegisterUser([FromForm] User user)
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

        // Default role for new users
        user.Role = "User";

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return View("Confirmation", new ConfirmationViewModel { Message = "User registered successfully.", Success = true });
    }

    [HttpPost]
    public async Task<IActionResult> ChangeRole([FromForm] string Passcode)
    {
        var userEmail = User.Identity.Name;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == userEmail);
        

        if (user == null)
        {
            return View("Confirmation", new ConfirmationViewModel { Message = "User not found.", Success = false });
        }

        if (Passcode == EmployeePasscode)
        {
            user.Role = user.Role == "Employee" ? "User" : "Employee";
            await _context.SaveChangesAsync();

            // Set the role change status in TempData
            TempData["RoleChanged"] = 1;

            return View("Confirmation", new ConfirmationViewModel { Message = $"Role updated successfully. User is now a {user.Role}.", Success = true });
        }

        return View("Confirmation", new ConfirmationViewModel { Message = "Invalid passcode.", Success = false });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "User");
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Settings()
    {
        var userEmail = User.Identity.Name;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == userEmail);

        if (user == null)
        {
            return RedirectToAction("Error", "Home");
        }

        var viewModel = new UserViewModel
        {
            Email = user?.UserEmail, // Use null-conditional operator to handle potential null user
            Role = User.Identity.Name // Assuming Role is stored in claims
        };
        return View(viewModel);
    }
}