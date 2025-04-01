using Diploma.Data;
using Diploma.Entities;
using Diploma.Repositories.Users;
using Microsoft.AspNetCore.Identity;
namespace HealthCenter.Repositories.Users
{
    class UsersRepository : IUsersRepository
    {
        private readonly ApplicationDbContext _patientsContext;
        private readonly UserManager<RegisteredUser> _userManager;
        private readonly SignInManager<RegisteredUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UsersRepository(ApplicationDbContext _patientsContext, UserManager<RegisteredUser> userManager,
     SignInManager<RegisteredUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this._patientsContext = _patientsContext;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
        }
        public async Task AddToRoleAsync(RegisteredUser user, string role)
        {
            await _userManager.AddToRoleAsync(user, role);
        }
        public async Task<bool> CheckPasswordAsync(RegisteredUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }
        public async Task<IdentityResult> CreateAsync(RegisteredUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }
        public async Task<RegisteredUser> FindByNameAsync(string name)
        {
            return await _userManager.FindByNameAsync(name);
        }
        public async Task<SignInResult> PasswordSignInAsync(RegisteredUser user, string password)
        {
            return await _signInManager.PasswordSignInAsync(user, password, false, false);
        }
        public async Task SaveChangesAsync()
        {
            await _patientsContext.SaveChangesAsync();
        }
        public async Task SignInAsync(RegisteredUser user)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
        }
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        public Task SignoutAsync()
        {
            throw new NotImplementedException();
        }
    }
}