using KhumaloCraft.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Server=tcp:khumalocraft-poe.database.windows.net,1433;Initial Catalog=KhumaloCraft;Persist Security Info=False;User ID=ST10326084@vcconnect.edu.za;Password=Summ2003@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Authentication=Active Directory Password;")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
   
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "MyWork",
    pattern: "MyWork",
    defaults: new { controller = "Work", action = "MyWork" });

app.MapControllerRoute(
    name: "Contact",
    pattern: "Contact/SubmitContact",
    defaults: new { controller = "Contact", action = "SubmitContact" });

app.Run();
