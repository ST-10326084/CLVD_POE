namespace KhumaloCraft.Data

open Microsoft.EntityFrameworkCore
open KhumaloCraft.Entities

type MyDbContext (options: DbContextOptions<MyDbContext>) =
  inherit DbContext(options)

  // Define DbSet properties for each entity
  member val Users: DbSet<User> = this.Set<User>()
  member val Products: DbSet<Product> = this.Set<Product>()
  member val ContactMessages: DbSet<ContactMessage> = this.Set<ContactMessage>()
