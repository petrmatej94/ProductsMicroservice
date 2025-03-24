namespace Products.Contracts.Requests;

public class PatchProductRequest
{
	public string? Name { get; init; }
	public string? ImageUrl { get; init; }
	public decimal? Price { get; init; }
	public string? Description { get; init; }
	public int? QuantityInStock { get; init; }
}
