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
		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentException("Name is required", nameof(name));

		if (!Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
			throw new ArgumentException("Invalid image URL", nameof(imageUrl));

		if (price < 0)
			throw new ArgumentOutOfRangeException(nameof(price), "Price must be greater or equal to 0");

		if (quantityInStock < 0)
			throw new ArgumentOutOfRangeException(nameof(quantityInStock), "Quantity cannot be negative");

		Id = Guid.NewGuid();
		Name = name;
		ImageUrl = imageUrl;
		Price = price;
		Description = description;
		QuantityInStock = quantityInStock;
	}

	public Product(string name, string imageUrl) 
		: this(name, imageUrl, 0m, null, 0)
	{ }

	public void UpdateStock(int quantity)
	{
		if (quantity < 0)
			throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity cannot be negative.");

		QuantityInStock = quantity;
	}
}
