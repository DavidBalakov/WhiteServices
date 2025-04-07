using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Diploma.Services;
using Diploma.Entities;
using Diploma.Models.ViewModels;
using Diploma.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Diploma.Extensions;

namespace Diploma.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly UserManager<RegisteredUser> _userManager;

        public OrderController(
            IOrderService orderService, 
            IProductService productService,
            UserManager<RegisteredUser> userManager)
        {
            _orderService = orderService;
            _productService = productService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                
                // Get products for the current user
                var products = await _productService.GetProductsForUser(User.Id());
                
                // Create SelectList for the dropdown
                ViewBag.Products = new SelectList(
                    products.Select(p => new { Id = p.Id.ToString(), Text = $"{p.Brand} {p.Model} ({p.SerialNumber})" }),
                    "Id", 
                    "Text"
                );
                
                return View(new OrderViewModel());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading create order form: {ex.Message}");
                
                // Return with error message
                TempData["ErrorMessage"] = "Възникна грешка при зареждане на формата. Моля, опитайте отново.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Create the order
                    var userId = _userManager.GetUserId(User);
                    await _orderService.CreateOrder(model, userId);
                    
                    TempData["SuccessMessage"] = "Поръчката е създадена успешно!";
                    return RedirectToAction(nameof(UserOrders));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating order: {ex.Message}");
                    ModelState.AddModelError("", "Възникна грешка при създаване на поръчката. Моля, опитайте отново.");
                }
            }
            
            var products = await _productService.GetProductsForUser(_userManager.GetUserId(User));
            ViewBag.Products = new SelectList(
                products.Select(p => new { Id = p.Id.ToString(), Text = $"{p.Brand} {p.Model} ({p.SerialNumber})" }),
                "Id", 
                "Text"
            );
            
            return View(model);
        }

        public async Task<IActionResult> UserOrders()
        {
            var orders = await _orderService.GetOrdersByUserId(User.Id());
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserOrders([FromQuery] OrdersSearch search, int start = 0, int length = 10)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var orders = await _orderService.GetOrdersByUserId(userId);
                
                // Convert to DTO
                var orderDTOs = orders.Select(o => new OrdersDTO
                {
                    Id = o.Id,
                    ClientName = $"{o.User.FirstName} {o.User.LastName}",
                    RegistrationDate = DateOnly.FromDateTime(o.RegistrationDate),
                    RepairType = o.Repair.RepairType.ToString(),
                    OrderStatus = o.OrderStatus.ToString(),
                    ProductBrand = o.Product?.Brand ?? "",
                    ProductModel = o.Product?.Model ?? "",
                    AdditionalNotes = o.Repair?.AdditionalNotes ?? ""
                }).ToList();
                
                // Apply filters if provided
                var filteredOrders = orderDTOs.AsQueryable();
                
                if (!string.IsNullOrEmpty(search.ClientName))
                {
                    filteredOrders = filteredOrders.Where(o => o.ClientName.Contains(search.ClientName));
                }
                
                if (search.Date.HasValue)
                {
                    filteredOrders = filteredOrders.Where(o => o.RegistrationDate == search.Date.Value);
                }
                
                if (!string.IsNullOrEmpty(search.ProductModel))
                {
                    filteredOrders = filteredOrders.Where(o => o.ProductModel.Contains(search.ProductModel));
                }
                
                if (!string.IsNullOrEmpty(search.ProductBrand))
                {
                    filteredOrders = filteredOrders.Where(o => o.ProductBrand.Contains(search.ProductBrand));
                }
                
                var totalCount = filteredOrders.Count();
                
                // Apply pagination
                var pagedOrders = filteredOrders
                    .Skip(start)
                    .Take(length)
                    .ToList();
                
                var result = new SearchResult<OrdersDTO>
                {
                    RecordsTotal = orderDTOs.Count,
                    RecordsFiltered = totalCount,
                    Start = start,
                    TotalPages = (int)Math.Ceiling((double)totalCount / length),
                    Data = pagedOrders
                };
                
                return Json(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error getting user orders: {ex.Message}");
                return Json(new SearchResult<OrdersDTO>
                {
                    RecordsTotal = 0,
                    RecordsFiltered = 0,
                    Start = start,
                    TotalPages = 0,
                    Data = new List<OrdersDTO>()
                });
            }
        }

        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Orders()
        {
            var orders = await _orderService.GetAllOrders();
            return View(orders);
        }

        [Authorize(Roles = "Admin, Employee")]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] OrdersSearch search, int start = 0, int length = 10)
        {
            try
            {
                var orders = await _orderService.GetAllOrders();
                
                // Convert to DTO
                var orderDTOs = orders.Select(o => new OrdersDTO
                {
                    Id = o.Id,
                    ClientName = $"{o.User.FirstName} {o.User.LastName}",
                    RegistrationDate = DateOnly.FromDateTime(o.RegistrationDate),
                    RepairType = o.Repair.RepairType.ToString(),
                    OrderStatus = o.OrderStatus.ToString(),
                    ProductBrand = o.Product?.Brand ?? "",
                    ProductModel = o.Product?.Model ?? "",
                    AdditionalNotes = o.Repair?.AdditionalNotes ?? ""
                }).ToList();
                
                // Apply filters if provided
                var filteredOrders = orderDTOs.AsQueryable();
                
                if (!string.IsNullOrEmpty(search.ClientName))
                {
                    filteredOrders = filteredOrders.Where(o => o.ClientName.Contains(search.ClientName));
                }
                
                if (search.Date.HasValue)
                {
                    filteredOrders = filteredOrders.Where(o => o.RegistrationDate == search.Date.Value);
                }
                
                if (!string.IsNullOrEmpty(search.ProductModel))
                {
                    filteredOrders = filteredOrders.Where(o => o.ProductModel.Contains(search.ProductModel));
                }
                
                if (!string.IsNullOrEmpty(search.ProductBrand))
                {
                    filteredOrders = filteredOrders.Where(o => o.ProductBrand.Contains(search.ProductBrand));
                }
                
                var totalCount = filteredOrders.Count();
                
                // Apply pagination
                var pagedOrders = filteredOrders
                    .Skip(start)
                    .Take(length)
                    .ToList();
                
                var result = new SearchResult<OrdersDTO>
                {
                    RecordsTotal = orderDTOs.Count,
                    RecordsFiltered = totalCount,
                    Start = start,
                    TotalPages = (int)Math.Ceiling((double)totalCount / length),
                    Data = pagedOrders
                };
                
                return Json(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting all orders: {ex.Message}");
                
                return Json(new SearchResult<OrdersDTO>
                {
                    RecordsTotal = 0,
                    RecordsFiltered = 0,
                    Start = start,
                    TotalPages = 0,
                    Data = new List<OrdersDTO>()
                });
            }
        }

        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var order = await _orderService.GetOrderById(id);
                if (order == null)
                {
                    return NotFound();
                }
                
                // Check if the current user is authorized to view this order
                var userId = _userManager.GetUserId(User);
                if (order.UserId != userId && !User.IsInRole("Admin") && !User.IsInRole("Employee"))
                {
                    return Forbid();
                }
                
                return View(order);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting order details: {ex.Message}");
                
                TempData["ErrorMessage"] = "Възникна грешка при зареждане на детайлите за поръчката.";
                return RedirectToAction(nameof(UserOrders));
            }
        }

        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Update(string id)
        {
            try
            {
                var order = await _orderService.GetOrderById(id);
                if (order == null)
                {
                    return NotFound();
                }
                
                // Map to view model
                var viewModel = new OrderViewModel
                {
                    Id = order.Id,
                    ProductId = order.ProductId.ToString(),
                    ProductModel = order.Product?.Model,
                    ProductBrand = order.Product?.Brand,
                    RegistrationDate = order.RegistrationDate.ToString("yyyy-MM-dd"),
                    RepairType = order.Repair.RepairType,
                    AdditionalNotes = order.Repair.AdditionalNotes,
                    OrderStatus = order.OrderStatus
                };
                
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading update order form: {ex.Message}");
                
                TempData["ErrorMessage"] = "Възникна грешка при зареждане на формата за редактиране.";
                return RedirectToAction(nameof(Orders));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Update(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _orderService.UpdateOrder(model);
                    
                    TempData["SuccessMessage"] = "Поръчката е обновена успешно!";
                    return RedirectToAction(nameof(Orders));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating order: {ex.Message}");
                    ModelState.AddModelError("", "Възникна грешка при обновяване на поръчката. Моля, опитайте отново.");
                }
            }
            
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _orderService.DeleteOrder(id);
                
                TempData["SuccessMessage"] = "Поръчката е изтрита успешно!";
                return RedirectToAction(nameof(Orders));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting order: {ex.Message}");
                
                TempData["ErrorMessage"] = "Възникна грешка при изтриване на поръчката.";
                return RedirectToAction(nameof(Orders));
            }
        }
    }
}

