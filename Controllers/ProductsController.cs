using Diploma.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Diploma.DTO;
using Diploma.Models.ViewModels.Products;
using Diploma.Services;
using Diploma.Extensions;

namespace Diploma.Controllers
{
    public class ProductsController : Controller
    {
        private IProductService _productService;
        private IHttpContextAccessor _httpContextAccessor;
        public ProductsController(IProductService ProductService, IHttpContextAccessor httpContextAccessor)
        {
            _productService = ProductService;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Diploma()
        {
            return View();
        }

        public IActionResult Products()
        {
            return View();
        }
        public IActionResult UserProducts()
        {
            return View();
        }
        
        public IActionResult Create()
        {
            ProductViewModel viewModel = new ProductViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel viewModel)
        {
            Product product = new Product();
            if (await _productService.AddProduct(viewModel, User.Id()))
            {
                TempData["success"] = $"Продукт {product.Model} бе създаден успешно!";
                return RedirectToAction("UserProducts");
            }
            else if (ModelState.IsValid)
            {
                TempData["error"] = "Невъзможно е да се създаде продуктът!";
            }

            return View(viewModel);
        }

        public IActionResult Update(string id)
        {
            Product? Product = _productService.GetProductById(id);
            if (Product == null)
            {
                TempData["error"] = "Продукт с номер " + id + " не бе открит!";
                return RedirectToAction("Diploma");
            }
            ProductViewModel viewModel = new ProductViewModel();
            viewModel.PopulateFromProduct(Product);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProductViewModel viewModel)
        {
            Product? Product = _productService.GetProductById(viewModel.Id);
            if (Product == null)
            {
                TempData["error"] = "Невъзможно е да се открие продуктът!";
                return RedirectToAction("Diploma");
            }
            viewModel.PopulateProduct(Product);
            if (await _productService.UpdateProduct(Product))
            {
                TempData["success"] = $"Продукт {Product.Model} беше обновен успешно!";
                return RedirectToAction("Diploma");
            }
            else if (ModelState.IsValid)
            {
                TempData["error"] = "Не може да бъде редактиран.";
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (await _productService.DeleteProduct(id))
            {
                TempData["success"] = "Продуктът беше изтрит успешно.";
            }
            else
            {
                TempData["error"] = "Не може да бъде изтрит.";
            }
            return RedirectToAction("Diploma");
        }
        public IActionResult Get(int draw, int start, int length)
        {
            length = 1;
            string urlQuery = _httpContextAccessor.HttpContext.Request.QueryString.Value;
            var paramsCollection = HttpUtility.ParseQueryString(urlQuery);

            //Get search params
            string? SerialNumber = paramsCollection["columns[0][search][value]"];
            string? Brand = paramsCollection["columns[1][search][value]"];
            string? Model = paramsCollection["columns[2][search][value]"];

            // string? defaultOrder = paramsCollection["columns[1][search][value]"];

            //Get sort
            string? sortColumnIndex = paramsCollection["order[0][column]"];
            string? sortColumnName = paramsCollection["columns[" + sortColumnIndex + "][data]"];
            string? sortDirection = paramsCollection["order[0][dir]"];
            string sortColumn = "";

            ProductSearch searchModel = new ProductSearch();
            searchModel.SerialNumber = SerialNumber;
            searchModel.Brand = Brand;
            searchModel.Model = Model;

            if (sortDirection == "asc")
                sortColumn = sortColumnName;
            else
                sortColumn = $"-{sortColumnName}";

            SearchResult<Product> result = _productService.Search(searchModel, sortColumn, start, length);

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
            string? SerialNumber = paramsCollection["columns[0][search][value]"];
            string? Brand = paramsCollection["columns[1][search][value]"];
            string? Model = paramsCollection["columns[2][search][value]"];

            // string? defaultOrder = paramsCollection["columns[1][search][value]"];

            //Get sort
            string? sortColumnIndex = paramsCollection["order[0][column]"];
            string? sortColumnName = paramsCollection["columns[" + sortColumnIndex + "][data]"];
            string? sortDirection = paramsCollection["order[0][dir]"];
            string sortColumn = "";

            ProductSearch searchModel = new ProductSearch();
            searchModel.SerialNumber = SerialNumber;
            searchModel.Brand = Brand;
            searchModel.Model = Model;

            if (sortDirection == "asc")
                sortColumn = sortColumnName;
            else
                sortColumn = $"-{sortColumnName}";

            SearchResult<Product> result = _productService.SearchForUser(searchModel, sortColumn, start, length, User.Id());

            return Ok(new
            {
                draw = draw,
                recordsTotal = result.RecordsTotal,
                recordsFiltered = result.RecordsFiltered,
                data = result.Data
            });
        }
    }
}
