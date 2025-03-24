namespace Products.Contracts.Requests;

public class CreateProductRequest
{
	public required string Name { get; init; }
	public required string ImageUrl { get; init; }
	public required decimal Price { get; init; }
	public string? Description { get; init; }
	public required int QuantityInStock { get; init; }
}
