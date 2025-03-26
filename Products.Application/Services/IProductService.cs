using Products.Domain.Entities;

namespace Products.Application.Services;

public interface IProductService
{
	Task<Product?> GetByIdAsync(Guid id, CancellationToken token = default);
	Task<IEnumerable<Product>> GetAllAsync(CancellationToken token = default);
	Task<Product> CreateAsync(Product product, CancellationToken token = default);
	Task<Product?> UpdateAsync(Product product, CancellationToken token = default);
}
