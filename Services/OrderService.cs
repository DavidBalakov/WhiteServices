using Diploma.Data;
using Diploma.DTO;
using Diploma.Entities;
using Diploma.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddOrderAsync(OrderViewModel orderViewModel, string userId)
        {
            Repair repair = new Repair()
            {
                ProductSerialNumber = orderViewModel.ProductId,
                RepairType = orderViewModel.RepairType,
            };

            Order order = new Order()
            {
                ClientId = userId,
                RepairId = repair.Id,
                ProductId = orderViewModel.ProductId,
                RegistrationDate = DateOnly.FromDateTime(DateTime.Now),
                OrderStatus = OrderStatus.Незавършена,
            };

            await _context.Repairs.AddAsync(repair);
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<string> AssignOrderAsync(string orderId, string employeeId)
        {
            throw new NotImplementedException();
        }

        public List<Order> GetAll()
        {
            return _context.Orders.Include(x => x.Product).Include(x => x.Client).Include(x => x.Repair).ToList();
        }
        public List<OrdersDTO> GetAllDTO()
        {
            return GetAll().Select(x => new OrdersDTO()
            {
                Id = x.Id,
                ClientName = x.Client.FirstName,
                ProductBrand = x.Product.Brand,
                ProductModel = x.Product.Model,
                RegistrationDate = x.RegistrationDate,
                RepairType = x.Repair.RepairType.ToString(),
                OrderStatus = x.OrderStatus.ToString(),
                AdditionalNotes = x.AdditionalNotes
            }).ToList();
        }
        public List<OrdersDTO> GetAllDTOForUser(string clientId)
        {
            return GetAll().Where(x => x.ClientId == clientId).Select(x => new OrdersDTO()
            {
                Id = x.Id,
                ClientName = x.Client.FirstName,
                ProductBrand = x.Product.Brand,
                ProductModel = x.Product.Model,
                RegistrationDate = x.RegistrationDate,
                RepairType = x.Repair.RepairType.ToString(),
                OrderStatus = x.OrderStatus.ToString(),
                AdditionalNotes = x.AdditionalNotes
            }).ToList();
        }

        public List<Order> GetUserOrders(string userId)
        {
            return _context.Orders.Where(x => x.ClientId == userId).Include(x => x.Product).ToList();
        }

        public SearchResult<OrdersDTO> Search(OrdersSearch searchModel, string sortColumn, int start, int length)
        {
            SearchResult<OrdersDTO> result = new SearchResult<OrdersDTO>();
            result.Data = GetAllDTO();
            if (!String.IsNullOrEmpty(searchModel.ClientName))
                result.Data = result.Data.Where(s => s.ClientName!.ToUpper().Contains(searchModel.ClientName.ToUpper())).ToList();
            if (searchModel.Date != null)
                result.Data = result.Data.Where(s => s.RegistrationDate == searchModel.Date).ToList();
            if (!String.IsNullOrEmpty(searchModel.ProductModel))
                result.Data = result.Data.Where(s => s.ProductModel!.ToUpper().Contains(searchModel.ProductModel.ToUpper())).ToList();
            if (!String.IsNullOrEmpty(searchModel.ProductBrand))
                result.Data = result.Data.Where(s => s.ProductBrand!.ToUpper().Contains(searchModel.ProductBrand.ToUpper())).ToList();

            switch (sortColumn)
            {
                case "registrationDate":
                    result.Data = result.Data.OrderBy(s => s.RegistrationDate).ToList();
                    break;
                case "-registrationDate":
                    result.Data = result.Data.OrderByDescending(s => s.RegistrationDate).ToList();
                    break;
            }

            return result;
        }
        public SearchResult<OrdersDTO> SearchForUser(OrdersSearch searchModel, string sortColumn, int start, int length, string userId)
        {
            SearchResult<OrdersDTO> result = new SearchResult<OrdersDTO>();
            result.Data = GetAllDTOForUser(userId);
            if (!String.IsNullOrEmpty(searchModel.ClientName))
                result.Data = result.Data.Where(s => s.ClientName!.ToUpper().Contains(searchModel.ClientName.ToUpper())).ToList();
            if (searchModel.Date != null)
                result.Data = result.Data.Where(s => s.RegistrationDate == searchModel.Date).ToList();
            if (!String.IsNullOrEmpty(searchModel.ProductModel))
                result.Data = result.Data.Where(s => s.ProductModel!.ToUpper().Contains(searchModel.ProductModel.ToUpper())).ToList();
            if (!String.IsNullOrEmpty(searchModel.ProductBrand))
                result.Data = result.Data.Where(s => s.ProductBrand!.ToUpper().Contains(searchModel.ProductBrand.ToUpper())).ToList();

            switch (sortColumn)
            {
                case "registrationDate":
                    result.Data = result.Data.OrderBy(s => s.RegistrationDate).ToList();
                    break;
                case "-registrationDate":
                    result.Data = result.Data.OrderByDescending(s => s.RegistrationDate).ToList();
                    break;
            }

            return result;
        }
        public void Update(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }
        public void Delete(string id)
        {
            var order = GetAll().FirstOrDefault(x => x.Id == id);
            _context.Orders.Remove(order);
            _context.SaveChanges();
        }
    }
}