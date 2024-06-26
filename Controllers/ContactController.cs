﻿using Microsoft.AspNetCore.Mvc;
using KhumaloCraft.Models;
using System.Threading.Tasks;

public class ContactController : Controller
{
    private readonly Context _context;

    public ContactController(Context context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> SubmitContact([FromForm] ContactMessage messages)
    {
        if (!ModelState.IsValid) // Validate the model
        {
            return BadRequest("Invalid form submission.");
        }

        _context.ContactMessages.Add(messages); // Add to the DbContext
        await _context.SaveChangesAsync(); // Save the data to the database

        return View("Confirmation", new ConfirmationViewModel { Message = "Your message has been submitted successfully.", Success = true });
    }
}
