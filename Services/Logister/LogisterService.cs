using Diploma.Data;
using Diploma.Entities;
using Diploma.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Diploma.Services.Register
{
    class LogisterService : ILogisterService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<RegisteredUser> _userManager;
        private readonly SignInManager<RegisteredUser> _signInManager;
        private readonly IUserStore<RegisteredUser> _userStore;

        public LogisterService(ApplicationDbContext context, UserManager<RegisteredUser> userManager, SignInManager<RegisteredUser> signInManager, IUserStore<RegisteredUser> userStore)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _userStore = userStore;
        }

        public async Task<IdentityResult> Register(RegisterViewModel registerRequest)
        {
            RegisteredUser user = new RegisteredUser()
            {
                UserName = registerRequest.UserName,
                Email = registerRequest.Email,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                PhoneNumber = registerRequest.PhoneNumber,
            };
            await _userStore.SetUserNameAsync(user, registerRequest.UserName, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, registerRequest.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                await _signInManager.SignInAsync(user, true);
                // if (!(registerRequest is RegisterViewModel))
                // {
                //     await _signInManager.SignInAsync(user, isPersistent: false);
                // }
                return result;
            }
            return IdentityResult.Failed();
        }
        
        public async Task<SignInResult> LogIn(LogInViewModel logInViewModel)
        {
            RegisteredUser user = await _userManager.FindByNameAsync(logInViewModel.UserName);
            if (user != null)
            {
                var passwordCheck = await _userManager
                .CheckPasswordAsync(user, logInViewModel.Password);
                if (passwordCheck)
                {
                    return await _signInManager.PasswordSignInAsync(user, logInViewModel.Password, false, false);
                }
            }
            return SignInResult.Failed;
        }

        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
        }
    }
}