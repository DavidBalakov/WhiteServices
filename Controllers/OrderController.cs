using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Web;
using Diploma.DTO;
using Diploma.Entities;
using Diploma.Extensions;
using Diploma.Models.ViewModels;
using Diploma.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

namespace Diploma.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productsService;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly JsonSerializerOptions _options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public OrderController(IOrderService orderService, IHttpContextAccessor httpContextAccessor
        , IProductService productsService)
        {
            _orderService = orderService;
            _httpContextAccessor = httpContextAccessor;
            _productsService = productsService;
        }

        public IActionResult Orders()
        {
            return View();
        }
        public IActionResult UserOrders()
        {
            return View();
        }

        public IActionResult Create()
        {
            OrderViewModel viewModel = new OrderViewModel();
            List<Product> userProducts = _productsService.GetProductsForUser(User.Id());
            ViewData["Products"] = new SelectList(userProducts, "SerialNumber", "Brand");
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderViewModel viewModel)
        {
            Product product = new Product();
            if (await _orderService.AddOrderAsync(viewModel, User.Id()))
            {
                TempData["success"] = $"Продукт {product.Model} бе добавен успешно!";
                return RedirectToAction("UserOrders");
            }
            else if (ModelState.IsValid)
            {
                TempData["error"] = "Невъзможно е да се създаде продуктът!";
            }

            return View(viewModel);
        }
        public string GetUserOrders()
        {
            List<Order> users = _orderService.GetUserOrders(User.Id());
            string json = JsonSerializer.Serialize(users, _options);

            return json;
        }

        public async Task<IActionResult> AssignOrder(string orderId, string employeeId)
        {
            string failiureMessage = await _orderService.AssignOrderAsync(orderId, employeeId);
            if (!failiureMessage.IsNullOrEmpty())
            {
                TempData["error"] = failiureMessage;
            }
            return RedirectToAction("Orders", "Order");
        }
        public IActionResult Get(int draw, int start, int length)
        {
            string urlQuery = _httpContextAccessor.HttpContext.Request.QueryString.Value;
            var paramsCollection = HttpUtility.ParseQueryString(urlQuery);

            //Get search params
            string? ClientName = paramsCollection["columns[0][search][value]"];
            DateOnly? Date = null;
            string? Brand = paramsCollection["columns[1][search][value]"];
            string? Model = paramsCollection["columns[2][search][value]"];

            // string? defaultOrder = paramsCollection["columns[1][search][value]"];

            //Get sort
            string? sortColumnIndex = paramsCollection["order[0][column]"];
            string? sortColumnName = paramsCollection["columns[" + sortColumnIndex + "][data]"];
            string? sortDirection = paramsCollection["order[0][dir]"];
            string sortColumn = "";

            OrdersSearch searchModel = new OrdersSearch();
            searchModel.ClientName = ClientName;
            searchModel.Date = Date;
            searchModel.ProductModel = Model;
            searchModel.ProductBrand = Brand;

            if (sortDirection == "asc")
                sortColumn = sortColumnName;
            else
                sortColumn = $"-{sortColumnName}";

            SearchResult<OrdersDTO> result = _orderService.Search(searchModel, sortColumn, start, length);

            return Ok(new
            {
                draw = draw,
                recordsTotal = result.RecordsTotal,
                recordsFiltered = result.RecordsFiltered,
                data = result.Data
            });
        }
        public IActionResult GetUser(int draw, int start, int length)
        {
            string urlQuery = _httpContextAccessor.HttpContext.Request.QueryString.Value;
            var paramsCollection = HttpUtility.ParseQueryString(urlQuery);

            //Get search params
            string? ClientName = paramsCollection["columns[0][search][value]"];
            DateOnly? Date = null;
            string? Brand = paramsCollection["columns[1][search][value]"];
            string? Model = paramsCollection["columns[2][search][value]"];

            // string? defaultOrder = paramsCollection["columns[1][search][value]"];

            //Get sort
            string? sortColumnIndex = paramsCollection["order[0][column]"];
            string? sortColumnName = paramsCollection["columns[" + sortColumnIndex + "][data]"];
            string? sortDirection = paramsCollection["order[0][dir]"];
            string sortColumn = "";

            OrdersSearch searchModel = new OrdersSearch();
            searchModel.ClientName = ClientName;
            searchModel.Date = Date;
            searchModel.ProductModel = Model;
            searchModel.ProductBrand = Brand;

            if (sortDirection == "asc")
                sortColumn = sortColumnName;
            else
                sortColumn = $"-{sortColumnName}";

            SearchResult<OrdersDTO> result = _orderService.SearchForUser(searchModel, sortColumn, start, length, User.Id());

            return Ok(new
            {
                draw = draw,
                recordsTotal = result.RecordsTotal,
                recordsFiltered = result.RecordsFiltered,
                data = result.Data
            });
        }
        public IActionResult Update(string id)
        {
            Order order = _orderService.GetAll().FirstOrDefault(x => x.Id == id);
            OrderViewModel viewModel = new OrderViewModel()
            {
                Id = order.Id,
                RepairType = order.Repair.RepairType,
                ProductId = order.ProductId,
                AdditionalNotes = order.AdditionalNotes,
                OrderStatus = order.OrderStatus,
            };
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Update(OrderViewModel orderViewModel)
        {
            ModelState.Remove("ProductId");
            if (ModelState.IsValid)
            {

                Order order1 = _orderService.GetAll().FirstOrDefault(x => x.Id == orderViewModel.Id);

                order1.AdditionalNotes = orderViewModel.AdditionalNotes;
                order1.Repair.RepairType = orderViewModel.RepairType;
                if (orderViewModel.OrderStatus is OrderStatus theOrderStatus)
                {
                    order1.OrderStatus = theOrderStatus;
                }

                _orderService.Update(order1);
                return RedirectToAction("Orders", "Order");

            }
            return View(orderViewModel);
        }
        public IActionResult Delete(string id)
        {
            _orderService.Delete(id);
            return RedirectToAction("Orders", "Order");
        }
    }
}