using Microsoft.AspNetCore.Mvc;
using KhumaloCraft.Models;
using System.Threading.Tasks;

public class ContactController : Controller
{
    private readonly MyDbContext _context;

    public ContactController(MyDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> SubmitContact([FromForm] ContactMessage message)
    {
        if (!ModelState.IsValid) // Validate the model
        {
            return BadRequest("Invalid form submission.");
        }

        _context.ContactMessages.Add(message); // Add to the DbContext
        await _context.SaveChangesAsync(); // Save the data to the database

        return Ok("Your message has been submitted successfully."); // Return a success message
    }
}
