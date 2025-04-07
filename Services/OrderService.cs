using Diploma.Data;
using Diploma.Entities;
using Diploma.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductService _productService;

        public OrderService(ApplicationDbContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _context.Orders
                .Include(o => o.Product)
                .Include(o => o.Repair)
                .Include(o => o.User)
                .OrderByDescending(o => o.RegistrationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserId(string userId)
        {
            return await _context.Orders
                .Include(o => o.Product)
                .Include(o => o.Repair)
                .Include(o => o.User)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.RegistrationDate)
                .ToListAsync();
        }

        public async Task<Order> GetOrderById(string id)
        {
            return await _context.Orders
                .Include(o => o.Product)
                .Include(o => o.Repair)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<string> CreateOrder(OrderViewModel model, string userId)
        {
            // Convert string ID to int for product
            int productId = int.Parse(model.ProductId);
            if (!int.TryParse(model.ProductId, out productId))
            {
                throw new ArgumentException("Invalid product ID format");
            }

            var product = await _productService.GetProductById(productId);
            if (product == null)
            {
                throw new ArgumentException("Product not found");
            }

            var repair = new Repair
            {
                RepairType = model.RepairType,
                AdditionalNotes = model.AdditionalNotes
            };

            var order = new Order
            {
                Id = Guid.NewGuid().ToString(),
                ProductId = productId,
                UserId = userId,
                RegistrationDate = DateTime.Now,
                OrderStatus = OrderStatus.Незапочната,
                Repair = repair
            };

            // Save to database
            _context.Repairs.Add(repair);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order.Id;
        }

        public async Task UpdateOrder(OrderViewModel model)
        {
            // Find the existing order
            var order = await _context.Orders
                .Include(o => o.Repair)
                .FirstOrDefaultAsync(o => o.Id == model.Id);

            if (order == null)
            {
                throw new ArgumentException("Order not found");
            }

            // Update order properties
            if (model.OrderStatus.HasValue)
            {
                order.OrderStatus = model.OrderStatus.Value;
            }

            // Update repair properties if needed
            if (order.Repair != null)
            {
                order.Repair.RepairType = model.RepairType;
                order.Repair.AdditionalNotes = model.AdditionalNotes;
            }

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrder(string id)
        {
            var order = await _context.Orders
                .Include(o => o.Repair)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order != null)
            {
                if (order.Repair != null)
                {
                    _context.Repairs.Remove(order.Repair);
                }

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}