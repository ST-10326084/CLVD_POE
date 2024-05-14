using System.ComponentModel.DataAnnotations;

namespace KhumaloCraft.Models
{
    public class User
    {
        public int UserID { get; set; }
        public required string UserEmail { get; set; }
        public required string PasswordUser { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }

    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public string Category { get; set; } 
        public int Stock { get; set; } 
    }

    public class ContactMessage
    {
        [Key] public int MessageID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
