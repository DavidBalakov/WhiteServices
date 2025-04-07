using Diploma.Entities;

namespace Diploma.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(int id);
        Task AddProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(string id);
        Task<IEnumerable<Product>> GetProductsForUser(string userId);
    }
}