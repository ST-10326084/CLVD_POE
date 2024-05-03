using KhumaloCraft.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace KhumaloCraft.Models;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
    : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ContactMessage> ContactMessages { get; set; }
}
