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
using UserManagement.Services.Interfaces.Api;
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

        // Auth based on cookies
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options => options.LoginPath = "/");
        services.AddAuthorization();
    }

    private static void Configure(WebApplication app)
    {
        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute("default", "{controller=UserAuth}/{action=Index}/{id?}");
    }
}
