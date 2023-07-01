using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Services;
using UserManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;
using UserManagement.Data;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        //var dbService = new DatabaseService();
        var Efdb = new UserManagementDbContext();

        //IUserManagerService userManagerService = app.Services.GetService<IUserManagerService>();


        // Endpoint for getting all users
        app.MapGet("/users", async (HttpContext context) =>
        {
            await Get.GetAllUsers(context.Response, Efdb);
        });

        // Endpoint for getting a specific user
        app.MapGet("/users/{id}", async (HttpContext context, string id) =>
        {
            await Get.GetUser(id, context.Response, Efdb);
        });

        // Endpoint for creating a user
        app.MapPost("/users", async (HttpContext context) =>
        {
            await Post.CreateUser(context.Request, context.Response, Efdb);
        });

        // Endpoint for updating a user
        app.MapPut("/users", async (HttpContext context) =>
        {
            await Put.UpdateUser(context.Request, context.Response, Efdb);
        });

        // Endpoint for deleting a user
        app.MapDelete("/users/{id}", async (HttpContext context, string id) =>
        {
            await Delete.DeleteUser(id, context.Response, Efdb);
        });

        app.UseStaticFiles();

        app.Use(async (context, next) =>
        {
            var request = context.Request;
            var response = context.Response;
            var path = request.Path;

            // If the request starts with "/", send the index.html file
            if (path.StartsWithSegments("/") && !path.StartsWithSegments("/api"))
            {
                response.ContentType = "text/html; charset=utf-8";
                await response.SendFileAsync("index.html");
                return;
            }

            await next();
        });

        app.Run();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IUserManagerService, UserManagerService>();


        // For EF
        /*string connection = Configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<UserManagementDbContext>(options => options.UseSqlServer(connection));*/
    }
}
