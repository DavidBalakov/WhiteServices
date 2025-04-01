using Diploma.DTO;
using Diploma.Entities;

namespace Diploma.Repositories.IRepositories
{
    public interface IPatientsRepository
    {
        bool Add(Product entity);
        List<Product> ToList();
        Product? FindById(string id);
        public bool Update(Product category);
        public bool Delete(string id);
        public Product? FindByName(string? name);
        public SearchResult<Product> GetPageData(ProductSearch searchModel, string sortColumn, int start, int length);
        public List<Product> GetPatientsInUseAsc();
    }
}