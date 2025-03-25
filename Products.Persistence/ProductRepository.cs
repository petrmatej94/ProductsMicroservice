using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;
using Products.Infrastructure.Repositories;
using Products.Persistence.Database;

namespace Products.Persistence;

public class ProductRepository : IProductRepository
{
	private readonly ApplicationDbContext _context;

	public ProductRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Product?> GetByIdAsync(Guid id)
	{
		return await _context.Products.FindAsync(id);
	}

	public async Task<IEnumerable<Product>> GetAllAsync()
	{
		return await _context.Products.ToListAsync();
	}

	public async Task<Product> CreateAsync(Product product)
	{
		_context.Products.Add(product);
		await _context.SaveChangesAsync();
		return product;
	}

	public async Task<Product?> PatchAsync(Product product)
	{
		Product? existingProduct = await _context.Products.FindAsync(product.Id);
		if (existingProduct == null)
			return null;

		if (!string.IsNullOrWhiteSpace(product.Name)) 
			existingProduct.UpdateName(product.Name);

		if (!string.IsNullOrWhiteSpace(product.ImageUrl)) 
			existingProduct.UpdateImageUrl(product.ImageUrl);

		if (product.Price >= 0) 
			existingProduct.UpdatePrice(product.Price);

		if (product.Description != null) 
			existingProduct.UpdateDescription(product.Description);

		if (product.QuantityInStock >= 0) 
			existingProduct.UpdateStock(product.QuantityInStock);

		await _context.SaveChangesAsync();
		return existingProduct;
	}
}
