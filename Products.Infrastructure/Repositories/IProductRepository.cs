using Products.Domain.Entities;

namespace Products.Infrastructure.Repositories;

public interface IProductRepository
{
	Task<IEnumerable<Product>> GetAllAsync();
	Task<Product?> GetByIdAsync(Guid id);
	Task<Product> CreateAsync(Product product);
	Task<Product?> PatchAsync(Product product);
}
