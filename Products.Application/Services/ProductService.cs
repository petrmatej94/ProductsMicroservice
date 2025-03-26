using Products.Domain.Entities;
using Products.Infrastructure.Repositories;

namespace Products.Application.Services;

public class ProductService : IProductService
{
	private readonly IProductRepository _productRepository;

	public ProductService(IProductRepository productRepository)
	{
		_productRepository = productRepository;
	}

	public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken token = default)
	{
		return await _productRepository.GetAllAsync(token);
	}

	public async Task<Product?> GetByIdAsync(Guid id, CancellationToken token = default)
	{
		return await _productRepository.GetByIdAsync(id, token);
	}

	public async Task<Product> CreateAsync(Product product, CancellationToken token = default)
	{
		return await _productRepository.CreateAsync(product, token);
	}

	public async Task<Product?> UpdateAsync(Product product, CancellationToken token = default)
	{
		return await _productRepository.PatchAsync(product, token);
	}

	public async Task<IEnumerable<Product>> GetAllPagedAsync(GetAllProductsOptions options, CancellationToken token = default)
	{
		return await _productRepository.GetAllPagedAsync(options, token);
	}
}
