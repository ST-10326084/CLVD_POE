using KhumaloCraft.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace KhumaloCraft.Models;

public class Context : DbContext
{
    public DbSet<User> Users {  get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ContactMessage> ContactMessages { get; set; }

    public DbSet<PurchasedItem> PurchasedItems { get; set; }

    public Context(DbContextOptions<Context> options) : base(options) 
    { 

    }

}
