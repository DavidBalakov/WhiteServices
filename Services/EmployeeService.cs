using Diploma.Entities;
using Diploma.Models.ViewModels;
using Diploma.Data;
using Microsoft.AspNetCore.Identity;
using Diploma.Services;
using Diploma.Services.Register;

namespace JewelryShop.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ILogisterService _userService;
        private readonly UserManager<RegisteredUser> _userManager;
        private readonly ApplicationDbContext _context;
        public EmployeeService(ILogisterService userService, ApplicationDbContext context, UserManager<RegisteredUser> userManager)
        {
            _userService = userService;
            _context = context;
            _userManager = userManager;
        }

        public async Task EditAsync(RegisterViewModel employeeEditRequest)
        {
            RegisteredUser employeeUser = _context.RegisteredUsers.Find(employeeEditRequest.UserId);

            employeeUser.UserName = employeeEditRequest.UserName;
            employeeUser.Email = employeeEditRequest.Email;
            employeeUser.FirstName = employeeEditRequest.FirstName;
            employeeUser.LastName = employeeEditRequest.LastName;
            employeeUser.PhoneNumber = employeeEditRequest.PhoneNumber;

            var roles = await _userManager.GetRolesAsync(employeeUser);
            for (int i = 0; i < roles.Count; i++)
            {
                await _userManager.RemoveFromRoleAsync(employeeUser, roles[i]);
            }
            _userManager.AddToRoleAsync(employeeUser, employeeEditRequest.Role);

            if (employeeEditRequest.Password != null)
            {
                await _userManager.RemovePasswordAsync(employeeUser);
                await _userManager.AddPasswordAsync(employeeUser, employeeEditRequest.Password);
            }

            await _userManager.UpdateAsync(employeeUser);
        }

        public RegisteredUser Get(string id)
        {
            return _context.RegisteredUsers.FirstOrDefault(x => x.Id == id);
        }

        public async Task<bool> RegisterEmployee(RegisterViewModel employeeRegisterRequest)
        {
            var result = await _userService.Register(employeeRegisterRequest);
            if (!result.Succeeded)
            {
                return false;
            }

            RegisteredUser user = _context.RegisteredUsers.Where(x => x.UserName == employeeRegisterRequest.UserName).FirstOrDefault();
            await _userManager.RemoveFromRoleAsync(user, "User");
            await _userManager.AddToRoleAsync(user, "Employee");
            return true;
        }
    }
}