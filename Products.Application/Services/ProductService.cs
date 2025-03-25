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

	public async Task<IEnumerable<Product>> GetAllAsync()
	{
		return await _productRepository.GetAllAsync();
	}

	public async Task<Product?> GetByIdAsync(Guid id)
	{
		return await _productRepository.GetByIdAsync(id);
	}

	public async Task<Product> CreateAsync(Product product)
	{
		return await _productRepository.CreateAsync(product);
	}

	public async Task<Product?> UpdateAsync(Product product)
	{
		return await _productRepository.PatchAsync(product);
	}
}
