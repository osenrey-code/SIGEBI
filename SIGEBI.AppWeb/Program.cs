using Microsoft.AspNetCore.Authentication.Cookies;
using SIGEBI.AppWeb.Services;
using SIGEBI.AppWeb.Handlers;

namespace SIGEBI.AppWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<JwtBearerHandler>();
            var apiBase = builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:54538/";

            builder.Services.AddHttpClient<IApiClient, ApiClient>(client =>
            {
                client.BaseAddress = new Uri(apiBase);
            })
            .AddHttpMessageHandler<JwtBearerHandler>();

            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Autenticacion/Login";
                    options.AccessDeniedPath = "/Autenticacion/AccessDenied";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                    options.SlidingExpiration = true;
                });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}