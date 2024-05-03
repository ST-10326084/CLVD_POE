namespace KhumaloCraft

open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.EntityFrameworkCore 
open KhumaloCraft.Entities
open KhumaloCraft.Data

module Program =
    [<EntryPoint>]
    let main args =
        // Create a new web application builder
        let builder = WebApplication.CreateBuilder(args)

        // Load configuration from appsettings.json and environment variables
        builder.Configuration.AddJsonFile("appsettings.json", optional = false, reloadOnChange = true)
                             .AddEnvironmentVariables()

        // Retrieve the connection string from the configuration
        let connectionString = builder.Configuration.GetConnectionString("DefaultConnection")

        // Configure database context with the connection string
        builder.Services.AddDbContext<MyDbContext>(fun options ->
            options.UseSqlServer(connectionString)
        ) // fs0041

        // Add other services and configurations here
        builder.Services.AddControllersWithViews() // For MVC/Razor Pages

        // Build the application
        let app = builder.Build()

        // Middleware and routing configuration
        if not app.Environment.IsDevelopment() then
            app.UseExceptionHandler("/Home/Error")
            app.UseHsts()
            // errors here aswell

        app.UseHttpsRedirection()
        app.UseStaticFiles()

        app.UseRouting()
        app.UseAuthorization()

        app.MapDefaultControllerRoute() // Default MVC route

        app.Run()

        0 // Exit code
