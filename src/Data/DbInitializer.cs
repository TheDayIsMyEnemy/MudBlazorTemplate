using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazorTemplate.Data.Entities;

namespace MudBlazorTemplate.Data
{
    public static class DbInitializer
    {
        private const string AdminEmail = "admin@admin.com";
        private const string AdminPassword = "Password11!";

        public static async Task ApplyMigrationsAndSeedData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<AppDbContext>();
                var userManager = services.GetRequiredService<UserManager<User>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                await dbContext.Database.MigrateAsync();

                await CreateRoles(roleManager);
                await CreateAdminUser(userManager);
            }
        }

        private static async Task CreateAdminUser(UserManager<User> userManager)
        {
            var admin = await userManager.FindByEmailAsync(AdminEmail);
            if (admin == null)
            {
                admin = new User
                {
                    UserName = AdminEmail,
                    Email = AdminEmail
                };
                await userManager.CreateAsync(admin, AdminPassword);
            }
            if (!await userManager.IsInRoleAsync(admin, Roles.Admin))
                await userManager.AddToRoleAsync(admin, Roles.Admin);
        }

        private static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Roles.List)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole { Name = role });
            }
        }
    }
}
