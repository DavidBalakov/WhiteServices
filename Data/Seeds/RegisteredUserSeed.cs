using Diploma.Entities;
using Microsoft.AspNetCore.Identity;

namespace Diploma.Data.Seed
{
    class UserSeed
    {
        public static async Task<string> Seed(IApplicationBuilder applicationBuilder)
        {
            string adminId = null;
            using (var scope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var _context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                var _userManager = scope.ServiceProvider.GetService<UserManager<RegisteredUser>>();
                var _userStore = scope.ServiceProvider.GetService<IUserStore<RegisteredUser>>();

                // if (_context.Users.Any())
                if (_userManager.FindByNameAsync("Admin").GetAwaiter().GetResult() == null)
                {
                    RegisteredUser admin = new RegisteredUser()
                    {
                        UserName = "Admin",
                        Password = "123%Ab",
                        Email = "admin@gmail.com",
                        FirstName = "Admin",
                        LastName = "Admin",
                        PhoneNumber = "0885321426",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true
                    };

                    await _userStore.SetUserNameAsync(admin, admin.UserName, CancellationToken.None);
                    var result = await _userManager.CreateAsync(admin, "123%Ab");
                    await _userManager.AddToRoleAsync(admin, "Admin");
                    adminId = admin.Id;
                }
                adminId = _userManager.FindByNameAsync("Admin").GetAwaiter().GetResult().Id;

                if (_userManager.FindByNameAsync("Employee").GetAwaiter().GetResult() == null)
                {
                    RegisteredUser employeeUser = new RegisteredUser()
                    {
                        UserName = "Employee",
                        Password = "345%Bb",
                        Email = "employee@gmail.com",
                        FirstName = "Employee",
                        LastName = "Employee",
                        PhoneNumber = "0885321426",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true
                    };

                    await _userStore.SetUserNameAsync(employeeUser, employeeUser.UserName, CancellationToken.None);
                    var result1 = await _userManager.CreateAsync(employeeUser, "345%Bb");
                    await _userManager.AddToRoleAsync(employeeUser, "Employee");
                }

                if (_userManager.FindByNameAsync("User").GetAwaiter().GetResult() == null)
                {
                    RegisteredUser user = new RegisteredUser()
                    {
                        UserName = "User",
                        Password = "678%Cb",
                        Email = "user@gmail.com",
                        FirstName = "User",
                        LastName = "User",
                        PhoneNumber = "0885321426",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true
                    };

                    await _userStore.SetUserNameAsync(user, user.UserName, CancellationToken.None);
                    await _userManager.CreateAsync(user, "678%Cb");
                    await _userManager.AddToRoleAsync(user, "User");

                }
            }
            return adminId;
        }
    }
}