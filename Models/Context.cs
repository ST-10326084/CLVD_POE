using KhumaloCraft.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace KhumaloCraft.Models;

public class Context : DbContext
{
    public DbSet<User> user {  get; set; }
    public DbSet<Product> products { get; set; }
    public DbSet<ContactMessage> contactMessages { get; set; }

    public Context(DbContextOptions options) : base(options) 
    { 

    }

}
