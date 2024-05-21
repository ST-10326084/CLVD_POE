using KhumaloCraft.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        ConfigureServices(builder.Services);

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
        app.UseAuthentication(); // Add this line to enable authentication
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
    }

    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();

        services.AddDbContext<Context>(options =>
            options.UseSqlServer("Server=tcp:khumalocraft-poe.database.windows.net,1433;Initial Catalog=KhumaloCraft;Persist Security Info=False;User ID=dean;Password=Summ2003@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;", sqlServerOptions =>
            {
                sqlServerOptions.EnableRetryOnFailure();
            }));

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/User/Login"; // Customize the login path as needed
                options.AccessDeniedPath = "/Home/AccessDenied"; // Customize the access denied path as needed
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Set the expiration time for the authentication cookie
            });
    }
}
