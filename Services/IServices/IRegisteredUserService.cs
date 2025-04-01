using Diploma.DTO;
using Diploma.Entities;

namespace Diploma.Services
{
    public interface IRegisteredUserService
    {
        Task<List<object>> GetUsersAndRoles(List<RegisteredUser> users);
        Task DeleteAsync(string id);
        Task<string> GetUserRole(string id);
        Task ChangeRole(string userId);
        Task<SearchResult<RegisteredUserSearch>> Search(RegisteredUserSearch searchModel, string sortColumn, int start, int length);
    }
}