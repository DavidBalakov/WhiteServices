using Diploma.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Diploma.Services.Register
{
    public interface ILogisterService
    {
        Task<IdentityResult> Register(RegisterViewModel registerRequest);
        Task<SignInResult> LogIn(LogInViewModel model);
        Task LogOut();
        Task<IdentityResult> ChangeUserRole(string userId, string newRole);
        Task<List<UserViewModel>> GetAllUsers();
    }
}