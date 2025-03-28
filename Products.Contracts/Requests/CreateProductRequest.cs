namespace Products.Contracts.Requests;

public class CreateProductRequest
{
	public required string Name { get; init; }
	public required string ImageUrl { get; init; }
}
