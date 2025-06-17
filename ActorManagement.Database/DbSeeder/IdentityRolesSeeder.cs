using ActorManagement.Models.EntityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorManagement.Database.DbSeeder
{
    public static class IdentityRolesSeeder
    {
        public static async Task SeedIdentityRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            string[] roles = new string[] { "Admin" };

            //Create admin role if not exists
            foreach (var role in roles)
            {
                if (!(await roleManager.RoleExistsAsync(role)))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            //Seed Admin user
            await CreateUserWithRoleAsync(userManager, "admin@gmail.com", "Admin@12345", "Admin");
        }
        private static async Task CreateUserWithRoleAsync(UserManager<AppUser> userManager, string email,
            string password, string role)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                var newUser = new AppUser()
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(newUser, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, role);
                }
            }

        }
    }
}
