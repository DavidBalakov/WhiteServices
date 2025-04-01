using Diploma.DTO;
using Diploma.Entities;
using Diploma.Helpers;
using Diploma.Models.ViewModels.Products;

namespace Diploma.Services
{
    public interface IProductService
    {
        Task<bool> AddProduct(ProductViewModel productAddRequest, string userId);
        List<Product> GetProductsForUser(string id);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(string id);
        Product? GetProductById(string id);
        SearchResult<Product> Search(ProductSearch searchModel, string sortColumn, int start, int length);
        SearchResult<Product> SearchForUser(ProductSearch searchModel, string sortColumn, int start, int length, string userId);
        List<Product> GetAllProducts();
        List<Product> GetSearched(string brand, string model);
    }
}