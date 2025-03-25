namespace Products.Domain.Entities;

public class Product
{
	public Guid Id { get; private set; }
	public string Name { get; private set; }
	public string ImageUrl { get; private set; }
	public decimal Price { get; private set; }
	public string? Description { get; private set; }
	public int QuantityInStock { get; private set; }

	public Product(string name, string imageUrl, decimal price, string? description, int quantityInStock)
	{
		Id = Guid.NewGuid();
		Name = name;
		ImageUrl = imageUrl;
		Price = price;
		Description = description;
		QuantityInStock = quantityInStock;
	}

	public void UpdateStock(int quantity)
	{
		if (quantity < 0)
			throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity cannot be negative.");

		QuantityInStock = quantity;
	}
}
