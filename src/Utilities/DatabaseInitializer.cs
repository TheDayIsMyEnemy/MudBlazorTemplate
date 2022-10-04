using MudBlazorTemplate.Data;
using MudBlazorTemplate.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MudBlazorTemplate.Utilities
{
    public static class DatabaseInitializer
    {
        private const string AdminEmail = "admin@admin.com";
        private const string AdminPassword = "Password11!";

        public static async Task SeedData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<AppDbContext>();
                var userManager = services.GetRequiredService<UserManager<User>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                await dbContext.Database.MigrateAsync();

                foreach (var role in Roles.List)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole { Name = role });
                }

                var admin = await userManager.FindByEmailAsync(AdminEmail);
                if (admin == null)
                {
                    admin = new User
                    {
                        UserName = AdminEmail,
                        Email = AdminEmail,
                        FirstName = "Almighty",
                        LastName = "Admin"
                    };
                    await userManager.CreateAsync(admin, AdminPassword);
                }
                if (!await userManager.IsInRoleAsync(admin, Roles.Admin))
                    await userManager.AddToRoleAsync(admin, Roles.Admin);
            }
        }
    }
}
