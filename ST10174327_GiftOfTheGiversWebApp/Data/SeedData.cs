using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ST10174327_GiftOfTheGiversWebApp.Models;

namespace ST10174327_GiftOfTheGiversWebApp.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Get services
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Create roles if they don't exist
                string[] roleNames = { "Admin", "User", "Volunteer", "Donor" };
                IdentityResult roleResult;

                foreach (var roleName in roleNames)
                {
                    var roleExist = await roleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                // Create admin user if it doesn't exist
                string adminEmail = "admin@giftofthegivers.com";
                string adminPassword = "Admin@123";

                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    var admin = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        FirstName = "Admin",
                        LastName = "User",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        IsActive = true
                    };

                    var createAdmin = await userManager.CreateAsync(admin, adminPassword);
                    if (createAdmin.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, "Admin");
                    }
                }

                // Create test users for other roles
                await CreateUserWithRole(userManager, "volunteer@giftofthegivers.com", "Volunteer@123", "Volunteer", "Test", "Volunteer");
                await CreateUserWithRole(userManager, "donor@giftofthegivers.com", "Donor@123", "Donor", "Test", "Donor");
                await CreateUserWithRole(userManager, "user@giftofthegivers.com", "User@123", "Regular", "User", "User");
            }
        }

        private static async Task CreateUserWithRole(UserManager<ApplicationUser> userManager, 
            string email, string password, string firstName, string lastName, string role)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                var newUser = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    IsActive = true
                };

                var createUser = await userManager.CreateAsync(newUser, password);
                if (createUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, role);
                }
            }
        }
    }
}
