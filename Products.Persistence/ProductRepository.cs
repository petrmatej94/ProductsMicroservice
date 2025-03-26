using Microsoft.EntityFrameworkCore;
using Products.Application.Interfaces;
using Products.Domain.Entities;
using Products.Persistence.Database;

namespace Products.Persistence;

public class ProductRepository : IProductRepository
{
	private readonly ApplicationDbContext _context;

	public ProductRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Product?> GetByIdAsync(Guid id, CancellationToken token = default)
	{
		return await _context.Products.FindAsync([id], token);
	}

	public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken token = default)
	{
		return await _context.Products.ToListAsync(token);
	}

	public async Task<Product> CreateAsync(Product product, CancellationToken token = default)
	{
		_context.Products.Add(product);
		await _context.SaveChangesAsync(token);
		return product;
	}

	public async Task<Product?> PatchAsync(Product product, CancellationToken token = default)
	{
		Product? existingProduct = await _context.Products.FindAsync([product.Id], token);
		if (existingProduct == null)
			return null;

		if (product.QuantityInStock >= 0) 
			existingProduct.UpdateStock(product.QuantityInStock);

		await _context.SaveChangesAsync(token);
		return existingProduct;
	}

	public async Task<IEnumerable<Product>> GetAllPagedAsync(GetAllProductsOptions options, CancellationToken token = default)
	{
		IQueryable<Product> query = _context.Products.AsQueryable();

		return await query
			.OrderBy(p => p.Name)
			.Skip((options.Page - 1) * options.PageSize)
			.Take(options.PageSize)
			.ToListAsync(token);
	}
}
