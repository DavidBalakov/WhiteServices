using Diploma.Entities;
using Diploma.Models.ViewModels;

namespace Diploma.Services
{
    public interface IEmployeeService
    {
        Task<bool> RegisterEmployee(RegisterViewModel masterRegisterRequest);
        RegisteredUser Get(string id);
        Task EditAsync(RegisterViewModel masterEditRequest);
    }
}