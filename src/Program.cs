using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Services;
using UserManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;
using UserManagement.Data;
using Microsoft.AspNetCore.Mvc;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder.Services, builder.Configuration);
        var app = builder.Build();
        Configure(app);
        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UserManagementDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddTransient<IUserManagerService, UserManagerService>();
        services.AddControllersWithViews();
    }

    private static void Configure(WebApplication app)
    {
        app.UseStaticFiles();

        app.UseRouting();

        app.MapControllerRoute("default", "{controller=User}/{action=Index}/{id?}");
    }
}
