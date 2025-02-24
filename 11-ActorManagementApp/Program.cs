using ActorManagement.Database.Data;
using ActorManagement.Repositories.AllRepositories;
using ActorManagement.Repositories.Contracts.AllContracts;
using ActorManagement.Services.AllServices;
using ActorManagement.Services.Contracts.AllContracts;
using Microsoft.EntityFrameworkCore;

namespace _11_ActorManagementApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            var connectionStringPath = builder.Configuration.GetConnectionString("AppConnectionString");
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionStringPath));
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddScoped<IUnitRepository, UnitRepository>();
            builder.Services.AddScoped<IUnitService, UnitService>();
            builder.Services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
