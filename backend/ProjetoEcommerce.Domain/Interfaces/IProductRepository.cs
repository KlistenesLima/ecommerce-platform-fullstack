using ProjetoEcommerce.Domain.Entities;

namespace ProjetoEcommerce.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> GetByCategoryAsync(Guid categoryId);
        Task<Product> CreateAsync(Product product);
        Task<Product> UpdateAsync(Product product);
        Task<bool> DeleteAsync(Guid id);
    }
}
