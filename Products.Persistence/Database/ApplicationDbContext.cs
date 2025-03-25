using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;

namespace Products.Persistence.Database;

public class ApplicationDbContext : DbContext
{
	public DbSet<Product> Products { get; set; }

	public ApplicationDbContext(DbContextOptions options)
		: base(options)
	{ }

	public static void Seed(ApplicationDbContext context)
	{
		if (!context.Products.Any())
		{
			context.Products.AddRange(
				new Product("Laptop Lenovo", "https://image.alza.cz/products/NT220f6e9c/NT220f6e9c.jpg", 20000, "Lenovo", 10),
				new Product("Phone Samgung", "https://image.alza.cz/products/SAMO0273c2/SAMO0273c2.jpg", 15000, "Samsung Galaxy", 20)
			);
			context.SaveChanges();
		}
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Product>().ToTable("Products");
	}
}
