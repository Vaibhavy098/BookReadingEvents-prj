using BookReadingEvents.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BookReadingEvents.Infrastructure.Identity;
// this seeder is Idempotent because it can run once, twice, or 100 times and still produce the same result without any adverse effects. It checks for the existence of roles and users before creating them, ensuring that duplicates are not created.
public static class AdminSeeder
{
    public static async Task SeedAsync(
        RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager)
    {
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(
                new IdentityRole("Admin"));
        }

        if (!await roleManager.RoleExistsAsync("User"))
        {
            await roleManager.CreateAsync(
                new IdentityRole("User"));
        }

        var adminUser =
            await userManager.FindByEmailAsync(
                "myadmin@bookevents.com");

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                Email = "myadmin@bookevents.com",
                UserName = "myadmin@bookevents.com",
                FullName = "System Administrator"
            };

            await userManager.CreateAsync(
                adminUser,
                "Admin@123");

            await userManager.AddToRoleAsync(  // addToRoleAsync is used to create a relationship between the user and a role in AspNetUserRoles.
                adminUser,
                "Admin");
        }
    }
}