using Products.Domain.Entities;

namespace Products.Application.Services;

public interface IProductService
{
	Task<Product?> GetByIdAsync(Guid id);
	Task<IEnumerable<Product>> GetAllAsync();
	Task<Product> CreateAsync(Product product);
	Task<Product?> UpdateAsync(Product product);
}
