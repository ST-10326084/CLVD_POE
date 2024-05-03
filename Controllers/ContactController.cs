using KhumaloCraft.Models;
using Microsoft.AspNetCore.Mvc;


namespace KhumaloCraft.Controllers
{

    public class ContactController : Controller
    {
        private readonly MyDbContext _context;

        public ContactController(MyDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult SubmitContactMessage(ContactMessage message)
        {
            if (ModelState.IsValid)
            {
                _context.ContactMessages.Add(message);
                _context.SaveChanges();
                return Ok("Message submitted successfully!");
            }
            return BadRequest("Failed to submit message.");
        }
    }
}