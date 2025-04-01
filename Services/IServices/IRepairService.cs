using Diploma.Models.ViewModels;

namespace Diploma.Services
{
    public interface IRepairService
    {
        Task<bool> RegisterEmployee(RegisterViewModel employeeRegisterRequest);
        RegisterViewModel Get(string id);
        Task EditAsync(RegisterViewModel employeeEditRequest);
    }
}