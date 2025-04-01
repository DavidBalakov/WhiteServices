using Diploma.Entities;
using Microsoft.AspNetCore.Identity;

namespace Diploma.Repositories.Users
{
    interface IUsersRepository
    {
        Task<RegisteredUser> FindByNameAsync(string name);
        Task<IdentityResult> CreateAsync(RegisteredUser user, string password);
        Task AddToRoleAsync(RegisteredUser user, string role);
        Task SignInAsync(RegisteredUser user);
        Task<bool> CheckPasswordAsync(RegisteredUser user, string password);
        Task<SignInResult> PasswordSignInAsync(RegisteredUser user, string password);
        Task SignOutAsync();
    }
}