using Diploma.Data;
using Diploma.DTO;
using Diploma.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Diploma.Services
{
    public class RegisteredUserService : IRegisteredUserService
    {
        private readonly UserManager<RegisteredUser> _userManager;
        private readonly SignInManager<RegisteredUser> _signInManager;
        private readonly ApplicationDbContext _context;
        public RegisteredUserService(UserManager<RegisteredUser> userManager, SignInManager<RegisteredUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public List<RegisteredUser> GetSearchedUsers(string firstName, string lastName)
        {
            var users = _context.RegisteredUsers.ToList();
            if (!firstName.IsNullOrEmpty())
            {
                users = users.Where(x => x.FirstName.ToLower().Contains(firstName.ToLower())).ToList();
            }
            if (!lastName.IsNullOrEmpty())
            {
                users = users.Where(x => x.LastName.ToLower().Contains(lastName.ToLower())).ToList();
            }

            return users;
        }

        public async Task<List<object>> GetUsersAndRoles(List<RegisteredUser> users)
        {
            var usersWithRoles = new List<object>();
            foreach (var user in users)
            {
                var roleList = await _userManager.GetRolesAsync(user);
                string role = "";

                switch (roleList.FirstOrDefault())
                {
                    case "Admin":
                        role = "Админ";
                        break;
                    case "Employee":
                        role = "Служител";
                        break;
                    case "User":
                        role = "Потребител";
                        break;
                    case "Guest":
                        role = "Гост";
                        break;
                }
                var userWithRole = new
                {
                    user.Id,
                    user.UserName,
                    user.FirstName,
                    user.LastName,
                    user.PhoneNumber,
                    user.Email,
                    Role = role,
                };
                usersWithRoles.Add(userWithRole);
            }
            return usersWithRoles;
        }

        public List<RegisteredUser> GetAll()
        {
            return _context.Users.ToList();
        }
        public async Task<List<RegisteredUserSearch>> GetAllSearchModelAsync()
        {
            var users = _userManager.Users.ToList();
            var userSearchList = new List<RegisteredUserSearch>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault() ?? "No Role"; // Get first role or default to "No Role"

                userSearchList.Add(new RegisteredUserSearch
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName, // Ensure these properties exist in your IdentityUser model
                    LastName = user.LastName,
                    Role = role
                });
            }

            return userSearchList;
        }

        public async Task DeleteAsync(string id)
        {
            RegisteredUser user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);
        }

        public async Task<string> GetUserRole(string id)
        {
            RegisteredUser user = _context.RegisteredUsers.FirstOrDefault(x => x.Id == id);
            var roleList = await _userManager.GetRolesAsync(user);
            string role = roleList.FirstOrDefault().ToString();
            return role;
        }

        public async Task<SearchResult<RegisteredUserSearch>> Search(RegisteredUserSearch searchModel, string sortColumn, int start, int length)
        {
            SearchResult<RegisteredUserSearch> result = new SearchResult<RegisteredUserSearch>();
            result.Data = await GetAllSearchModelAsync();
            if (!String.IsNullOrEmpty(searchModel.UserName))
                result.Data = result.Data.Where(s => s.UserName!.ToUpper().Contains(searchModel.UserName.ToUpper())).ToList();
            if (!String.IsNullOrEmpty(searchModel.FirstName))
                result.Data = result.Data.Where(s => s.FirstName!.ToUpper().Contains(searchModel.FirstName.ToUpper())).ToList();
            if (!String.IsNullOrEmpty(searchModel.LastName))
                result.Data = result.Data.Where(s => s.LastName!.ToUpper().Contains(searchModel.LastName.ToUpper())).ToList();
            if (!String.IsNullOrEmpty(searchModel.Role))
            {
                foreach (var user in result.Data)
                {
                    var role = await GetUserRole(user.Id);
                    if (role != searchModel.Role)
                    {
                        result.Data.Remove(user);
                    }
                }
            }

            return result;
        }

        public async Task ChangeRole(string userId)
        {
            RegisteredUser user = await _userManager.FindByIdAsync(userId);
            if(user != null){
                var roleList = await _userManager.GetRolesAsync(user);
                if (roleList.FirstOrDefault() == "Admin")
                {
                    return;
                }
                await _userManager.AddToRoleAsync(user, roleList.FirstOrDefault() == "Employee" ? "User" : "Employee");
                await _userManager.RemoveFromRoleAsync(user, roleList.FirstOrDefault() == "Employee" ? "Employee" : "User");
            }
        }
    }
}