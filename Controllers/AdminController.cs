using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Diploma.Entities;
using Diploma.Models.ViewModels;
using Diploma.DTO;

namespace Diploma.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<RegisteredUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(
            UserManager<RegisteredUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult AllUsers()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] RegisteredUserSearch search, int start = 0, int length = 10)
        {
            try
            {
                var users = _userManager.Users.AsQueryable();
                
                // Apply filters if provided
                if (!string.IsNullOrEmpty(search.UserName))
                {
                    users = users.Where(u => u.UserName.Contains(search.UserName));
                }
                
                if (!string.IsNullOrEmpty(search.FirstName))
                {
                    users = users.Where(u => u.FirstName.Contains(search.FirstName));
                }
                
                if (!string.IsNullOrEmpty(search.LastName))
                {
                    users = users.Where(u => u.LastName.Contains(search.LastName));
                }
                
                var totalCount = users.Count();
                
                // Apply pagination
                var pagedUsers = users
                    .Skip(start)
                    .Take(length)
                    .ToList();
                
                var userViewModels = new List<UserViewModel>();
                
                foreach (var user in pagedUsers)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    
                    // Filter by role if provided
                    if (!string.IsNullOrEmpty(search.Role) && !roles.Contains(search.Role))
                    {
                        continue;
                    }
                    
                    var userViewModel = new UserViewModel
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        Roles = roles.ToList()
                    };
                    
                    userViewModels.Add(userViewModel);
                }
                
                var result = new SearchResult<UserViewModel>
                {
                    RecordsTotal = totalCount,
                    RecordsFiltered = userViewModels.Count,
                    Start = start,
                    TotalPages = (int)Math.Ceiling((double)totalCount / length),
                    Data = userViewModels
                };
                
                return Json(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error getting users: {ex.Message}");
                return Json(new SearchResult<UserViewModel>
                {
                    RecordsTotal = 0,
                    RecordsFiltered = 0,
                    Start = start,
                    TotalPages = 0,
                    Data = new List<UserViewModel>()
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableRoles()
        {
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            return Json(roles);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserRole(string userId, string role, bool isAdd)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return Json(new { success = false, message = "Потребителят не е намерен." });
                }

                if (isAdd)
                {
                    // Add role
                    if (!await _userManager.IsInRoleAsync(user, role))
                    {
                        var result = await _userManager.AddToRoleAsync(user, role);
                        if (!result.Succeeded)
                        {
                            return Json(new { success = false, message = "Грешка при добавяне на роля." });
                        }
                    }
                }
                else
                {
                    // Remove role
                    if (await _userManager.IsInRoleAsync(user, role))
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user, role);
                        if (!result.Succeeded)
                        {
                            return Json(new { success = false, message = "Грешка при премахване на роля." });
                        }
                    }
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user role: {ex.Message}");
                return Json(new { success = false, message = "Възникна грешка при обновяване на ролята." });
            }
        }
    }
}
