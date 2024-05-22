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
        // Check if the entered code matches the predefined employee passcode
        if (Passcode == EmployeePasscode)
        {
            // Get the current user
            var userEmail = User.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == userEmail);

            if (user != null)
            {
                // Check if the user is currently an employee
                if (user.Role == "Employee")
                {
                    // Update the user's role to "User"
                    user.Role = "User";
                    await _context.SaveChangesAsync();
                    return View("Confirmation", new ConfirmationViewModel { Message = "Role updated successfully. User is now a regular user.", Success = true });
                }
                else
                {
                    user.Role = "Employee";
                    await _context.SaveChangesAsync();
                    return View("Confirmation", new ConfirmationViewModel { Message = "Role updated successfully.", Success = true });
                }
            }
            else
            {
                return View("Confirmation", new ConfirmationViewModel { Message = "User not found.", Success = false });
            }
        }
        else
        {
            return View("Confirmation", new ConfirmationViewModel { Message = "Invalid passcode.", Success = false });
        }
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
    public IActionResult Settings()
    {
        var userEmail = User.Identity.Name;
        var userRole = User.FindFirstValue(ClaimTypes.Role);

        // You can create a view model to hold email and role
        var viewModel = new UserViewModel { Email = userEmail, Role = userRole };

        // Pass the view model to the view
        return View(viewModel);
    }




}