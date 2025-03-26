using Products.Domain.Entities;

namespace Products.Infrastructure.Repositories;

public interface IProductRepository
{
	Task<IEnumerable<Product>> GetAllAsync(CancellationToken token = default);
	Task<Product?> GetByIdAsync(Guid id, CancellationToken token = default);
	Task<Product> CreateAsync(Product product, CancellationToken token = default);
	Task<Product?> PatchAsync(Product product, CancellationToken token = default);
	Task<IEnumerable<Product>> GetAllPagedAsync(GetAllProductsOptions options, CancellationToken token = default);
}
