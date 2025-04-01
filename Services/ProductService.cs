using Diploma.Data;
using Diploma.DTO;
using Diploma.Entities;
using Diploma.Helpers;
using Diploma.Models.ViewModels.Products;
using Microsoft.EntityFrameworkCore;

namespace Diploma.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddProduct(ProductViewModel productAddRequest, string userId)
        {
            Product product = new Product()
            {
                SerialNumber = productAddRequest.SerialNumber,
                Brand = productAddRequest.Brand,
                Model = productAddRequest.Model,
                DateOfPurchase = DateOnly.FromDateTime(DateTime.Now),
                WarrantyExpirationDate = DateOnly.FromDateTime(DateTime.Now.AddYears(4)),
                UserId = userId,
            };

            await _context.Products.AddAsync(product);

            await _context.SaveChangesAsync();
            return true;
        }

        public List<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public List<Product> GetSearched(string brand, string model)
        {
            List<Product> searchedProducts = GetAllProducts();

            if (brand != null)
            {
                searchedProducts = searchedProducts.Where(x => x.Brand.ToLower().Contains(brand.ToLower())).ToList();
            }
            if (model != null)
            {
                searchedProducts = searchedProducts.Where(x => x.Model.ToLower().Contains(model.ToLower())).ToList();
            }
            return searchedProducts;
        }
        public SearchResult<Product> Search(ProductSearch searchModel, string sortColumn, int start, int length)
        {
            SearchResult<Product> result = new SearchResult<Product>();
            result.Data = GetAllProducts();
            if (!String.IsNullOrEmpty(searchModel.SerialNumber))
                result.Data = result.Data.Where(s => s.SerialNumber!.ToUpper().Contains(searchModel.SerialNumber.ToUpper())).ToList();
            if (!String.IsNullOrEmpty(searchModel.Brand))
                result.Data = result.Data.Where(s => s.Brand!.ToUpper().Contains(searchModel.Brand.ToUpper())).ToList();
            if (!String.IsNullOrEmpty(searchModel.Model))
                result.Data = result.Data.Where(s => s.Model!.ToUpper().Contains(searchModel.Model.ToUpper())).ToList();

            return result;
        }
        public SearchResult<Product> SearchForUser(ProductSearch searchModel, string sortColumn, int start, int length, string userId)
        {
            SearchResult<Product> result = new SearchResult<Product>();
            result.Data = GetAllProducts().Where(x => x.UserId == userId).ToList();
            if (!String.IsNullOrEmpty(searchModel.SerialNumber))
                result.Data = result.Data.Where(s => s.SerialNumber!.ToUpper().Contains(searchModel.SerialNumber.ToUpper())).ToList();
            if (!String.IsNullOrEmpty(searchModel.Brand))
                result.Data = result.Data.Where(s => s.Brand!.ToUpper().Contains(searchModel.Brand.ToUpper())).ToList();
            if (!String.IsNullOrEmpty(searchModel.Model))
                result.Data = result.Data.Where(s => s.Model!.ToUpper().Contains(searchModel.Model.ToUpper())).ToList();

            return result;
        }

        public Task<bool> UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            throw new NotImplementedException();
        }

        public Product? FindById(string? id)
        {
            if (id == null)
                return null;

            Product? Product = _context.Products.Find(id);
            return Product;
        }

        public bool DeleteProduct(string id)
        {
            Product? product = FindById(id);
            if (product == null)
                return false;

            try
            {
                _context.Products.Remove(product);
                int stateNumber = _context.SaveChanges();
                return stateNumber > 0;
            }
            catch
            {
                return false;
            }
        }

        public Product? GetProductById(string id)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetPatientsInUseAsc()
        {
            return _context.Products.OrderBy(x => x).ToList();

        }

        Task<bool> IProductService.DeleteProduct(string id)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetProductsForUser(string id)
        {
            return GetAllProducts().Where(x => x.UserId == id).ToList();
        }
    }
}