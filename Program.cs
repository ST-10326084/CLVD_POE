using KhumaloCraft.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.AddDbContext<Context>(options =>
            options.UseSqlServer("Server=tcp:khumalocraft-poe.database.windows.net,1433;Initial Catalog=KhumaloCraft;Persist Security Info=False;User ID=dean;PasswordUser=Summ2003@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;", sqlServerOptions =>
            {
                sqlServerOptions.EnableRetryOnFailure();
            }));

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

        app.MapControllerRoute(
            name: "Login",
            pattern: "User/LoginUser",
            defaults: new { controller = "User", action = "LoginUser" });

        app.Run();
    }
}