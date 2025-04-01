using Diploma.Data;
using Diploma.Entities;
using Diploma.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Diploma.Services
{
    public class RepairService : IRepairService
    {
        private readonly IRegisteredUserService _userService;
        private readonly UserManager<RegisteredUser> _userManager;
        private readonly ApplicationDbContext _context;
        public RepairService(IRegisteredUserService userService, ApplicationDbContext context, UserManager<RegisteredUser> userManager)
        {
            _userService = userService;
            _context = context;
            _userManager = userManager;
        }

        public async Task EditAsync(RegisterViewModel employeeEditRequest)
        {

        }

        public RegisterViewModel Get(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RegisterEmployee(RegisterViewModel EmployeeRegisterRequest)
        {
            return true;
        }
    }
}