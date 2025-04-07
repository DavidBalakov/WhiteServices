using Diploma.Entities;
using Diploma.Models.ViewModels;

namespace Diploma.Services
{
    public interface IOrderService
    {
        // Get all orders (for admin/technician)
        Task<IEnumerable<Order>> GetAllOrders();
        
        // Get orders for a specific user
        Task<IEnumerable<Order>> GetOrdersByUserId(string userId);
        
        // Get a specific order by ID
        Task<Order> GetOrderById(string id);
        
        // Create a new order
        Task<string> CreateOrder(OrderViewModel model, string userId);
        
        // Update an existing order
        Task UpdateOrder(OrderViewModel model);
        
        // Delete an order
        Task DeleteOrder(string id);
    }
}