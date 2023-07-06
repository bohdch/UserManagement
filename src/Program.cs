using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UserManagement.Services;
using UserManagement.Services.Interfaces;
using UserManagement.Models;
using UserManagement.Data;
using UserManagement.Controllers.Api;

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
        services.AddTransient<IUserAuthService, UserAuthService>();
        services.AddTransient<IUserApiController, UserApiController>();

        services.AddControllersWithViews();
        services.AddHttpContextAccessor();

        // User authentication based on cookies
        services.AddAuthentication("UserAuth")
            .AddCookie("UserAuth", options =>
            {
                options.Cookie.Name = "UserCookie";
                options.LoginPath = "/";
            });

        // Admin authentication based on cookies
        services.AddAuthentication("AdminAuth")
            .AddCookie("AdminAuth", options =>
            {
                options.Cookie.Name = "AdminCookie";
                options.LoginPath = "/admin";
            });
    }

    private static void Configure(WebApplication app)
    {
        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseCookiePolicy();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute("default", "{controller=UserAuth}/{action=Index}/{id?}");
    }
}
