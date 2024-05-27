using System.ComponentModel.DataAnnotations;

namespace KhumaloCraft.Models
{
    public class User
    {
        public int UserID { get; set; }

        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        public string Role { get; set; } 
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

    public class PurchasedItem
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }

        // Navigation property to the Product entity
        public Product Product { get; set; }
    }
}
