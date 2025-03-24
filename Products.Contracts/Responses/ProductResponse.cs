namespace Products.Contracts.Responses;

public class ProductResponse
{
	public Guid Id { get; init; }
	public string Name { get; init; } = String.Empty;
	public string ImageUrl { get; init; } = String.Empty;
	public decimal Price { get; init; }
	public string? Description { get; init; }
	public int QuantityInStock { get; init; }
}
