using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Diploma.Services;
using Diploma.Entities;
using Diploma.DTO;
using Microsoft.AspNetCore.Identity;
using Diploma.Models.ViewModels.Products;
using Diploma.Extensions;

namespace Diploma.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly UserManager<RegisteredUser> _userManager;

        public ProductsController(
            IProductService productService,
            UserManager<RegisteredUser> userManager)
        {
            _productService = productService;
            _userManager = userManager;
        }

        public async Task<IActionResult> UserProducts()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var products = await _productService.GetProductsForUser(userId);
                return View(products ?? new List<Product>());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading products: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");

                return View(new List<Product>());
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = _userManager.GetUserId(User);

                    var product = new Product
                    {
                        SerialNumber = model.SerialNumber,
                        Brand = model.Brand,
                        Model = model.Modelz,
                        UserId = User.Id()
                    };

                    Console.WriteLine($"Adding product: {product.Brand} {product.Model}");

                    await _productService.AddProduct(product);

                    Console.WriteLine($"Product added successfully with ID: {product.Id}");

                    TempData["SuccessMessage"] = "Продуктът е добавен успешно!";

                    return Redirect("/Products/Products");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating product: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }

                ModelState.AddModelError("", "Възникна грешка при създаване на продукта. Моля, опитайте отново.");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] ProductSearch search, int start = 0, int length = 10)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var allProducts = await _productService.GetProductsForUser(userId);

                var filteredProducts = allProducts.AsQueryable();

                if (!string.IsNullOrEmpty(search.SerialNumber))
                {
                    filteredProducts = filteredProducts.Where(p => p.SerialNumber.Contains(search.SerialNumber));
                }

                if (!string.IsNullOrEmpty(search.Brand))
                {
                    filteredProducts = filteredProducts.Where(p => p.Brand.Contains(search.Brand));
                }

                if (!string.IsNullOrEmpty(search.Model))
                {
                    filteredProducts = filteredProducts.Where(p => p.Model.Contains(search.Model));
                }

                var totalCount = filteredProducts.Count();

                // Apply pagination
                var pagedProducts = filteredProducts
                    .Skip(start)
                    .Take(length)
                    .ToList();

                var result = new SearchResult<Product>
                {
                    RecordsTotal = allProducts.Count(),
                    RecordsFiltered = totalCount,
                    Start = start,
                    TotalPages = (int)Math.Ceiling((double)totalCount / length),
                    Data = pagedProducts
                };

                return Json(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching products: {ex.Message}");
                return Json(new SearchResult<Product>
                {
                    RecordsTotal = 0,
                    RecordsFiltered = 0,
                    Start = start,
                    TotalPages = 0,
                    Data = new List<Product>()
                });
            }
        }

        public async Task<IActionResult> Update(string id)
        {
            var product = await _productService.GetProductById(int.Parse(id));
            if (product == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (product.UserId != userId && !User.IsInRole("Admin") && !User.IsInRole("Employee"))
            {
                return Forbid();
            }

            var viewModel = new ProductViewModel
            {
                Id = product.Id.ToString(),
                SerialNumber = product.SerialNumber,
                Brand = product.Brand,
                Modelz = product.Model,
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var product = await _productService.GetProductById(int.Parse(model.Id));
                    if (product == null)
                    {
                        return NotFound();
                    }

                    var userId = _userManager.GetUserId(User);
                    if (product.UserId != userId && !User.IsInRole("Admin") && !User.IsInRole("Employee"))
                    {
                        return Forbid();
                    }

                    product.SerialNumber = model.SerialNumber;
                    product.Brand = model.Brand;
                    product.Model = model.Modelz;

                    await _productService.UpdateProduct(product);
                    TempData["SuccessMessage"] = "Продуктът е обновен успешно!";
                    return Redirect("/Products/Products");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating product: {ex.Message}");
                    ModelState.AddModelError("", "Възникна грешка при обновяване на продукта. Моля, опитайте отново.");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var product = await _productService.GetProductById(int.Parse(id));
                if (product == null)
                {
                    return NotFound();
                }

                var userId = _userManager.GetUserId(User);
                if (product.UserId != userId && !User.IsInRole("Admin") && !User.IsInRole("Employee"))
                {
                    return Forbid();
                }

                await _productService.DeleteProduct(id);
                TempData["SuccessMessage"] = "Продуктът е изтрит успешно!";
                return Redirect("/Products/Products");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting product: {ex.Message}");
                TempData["ErrorMessage"] = "Възникна грешка при изтриване на продукта.";
                return Redirect("/Products/Products");
            }
        }

        public async Task<IActionResult> Details(string id)
        {
            var product = await _productService.GetProductById(int.Parse(id));
            if (product == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (product.UserId != userId && !User.IsInRole("Admin") && !User.IsInRole("Employee"))
            {
                return Forbid();
            }

            return View(product);
        }

        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Products()
        {
            var products = await _productService.GetAllProducts();
            return View(products ?? new List<Product>());
        }
    }
}
