using Microsoft.AspNetCore.Identity;

namespace Diploma.Data.Seed
{
    class RolesSeed
    {
        public static async Task Initialize(IServiceScope scope)
        {
            // Add your role seeding logic here
        }
        public static async Task Seed(IApplicationBuilder applicationBuilder)
        {
            using (var scope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                await CreateRoles(roleManager);
            }
        }
        public static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
                await roleManager.CreateAsync(new IdentityRole("Employee"));
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
        }
    }
}