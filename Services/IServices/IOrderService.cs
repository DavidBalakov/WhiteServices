using Diploma.DTO;
using Diploma.Entities;
using Diploma.Models.ViewModels;

namespace Diploma.Services
{
    public interface IOrderService
    {
        List<Order> GetAll();
        List<Order> GetUserOrders(string userId);
        void Update(Order order);
        void Delete(string id);

        Task<bool> AddOrderAsync(OrderViewModel orderViewModel, string userId);
        SearchResult<OrdersDTO> Search(OrdersSearch searchModel, string sortColumn, int start, int length);
        SearchResult<OrdersDTO> SearchForUser(OrdersSearch searchModel, string sortColumn, int start, int length, string userId);
        Task<string> AssignOrderAsync(string orderId, string employeeId);
    }
}