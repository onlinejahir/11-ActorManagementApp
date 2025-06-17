using _11_ActorManagementApp.EmailServices;
using _11_ActorManagementApp.EmailServices.Contracts;
using _11_ActorManagementApp.ProjectModels;
using _11_ActorManagementApp.ProjectModels.Contracts;
using ActorManagement.Database.Data;
using ActorManagement.Database.DbSeeder;
using ActorManagement.Models.EntityModels;
using ActorManagement.Repositories.AllRepositories;
using ActorManagement.Repositories.Contracts.AllContracts;
using ActorManagement.Services.AllServices;
using ActorManagement.Services.Contracts.AllContracts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace _11_ActorManagementApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.             
            var connectionStringPath = builder.Configuration.GetConnectionString("AppConnectionString")
                ?? throw new InvalidOperationException("Connection string not found.");
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionStringPath));
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddScoped<IUnitRepository, UnitRepository>();
            builder.Services.AddScoped<IUnitService, UnitService>();
            builder.Services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, AppUserClaimsPrincipalFactory>();
            builder.Services.AddScoped<ILoggedInUserInfo, LoggedInUserInfo>();
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 0;
            });
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.Cookie.Name = "ActorManagementAppCookie";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/Account/Login";
                // ReturnUrlParameter requires 
                //using Microsoft.AspNetCore.Authentication.Cookies;
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

            //Call SeedIdentityRolesAsync() method to seed role/admin data
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await IdentityRolesSeeder.SeedIdentityRolesAsync(services);
            }

            app.Run();
        }
    }
}
