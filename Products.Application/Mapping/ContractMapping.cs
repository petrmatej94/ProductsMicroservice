using Products.Contracts.Requests;
using Products.Contracts.Responses;
using Products.Domain.Entities;

namespace Products.Application.Mapping;

public static class ContractMapping
{
	public static Product MapToProduct(this CreateProductRequest request)
	{
		return new Product(
			name: request.Name,
			imageUrl: request.ImageUrl,
			price: request.Price,
			description: request.Description,
			quantityInStock: request.QuantityInStock
			);
	}

	public static ProductResponse MapToResponse(this Product product)
	{
		return new ProductResponse
		{
			Id = product.Id,
			Name = product.Name,
			ImageUrl = product.ImageUrl,
			Price = product.Price,
			Description = product.Description,
			QuantityInStock = product.QuantityInStock,
		};
	}

	public static IEnumerable<ProductResponse> MapToResponse(this IEnumerable<Product> products)
	{
		return products.Select(product => product.MapToResponse());
	}

	public static void UpdateFromRequest(this Product product, PatchProductRequest request)
	{
		if (request.Name is not null)
			product.UpdateName(request.Name);

		if (request.ImageUrl is not null)
			product.UpdateImageUrl(request.ImageUrl);

		if (request.Price.HasValue)
			product.UpdatePrice(request.Price.Value);

		if (request.Description is not null)
			product.UpdateDescription(request.Description);

		if (request.QuantityInStock.HasValue)
			product.UpdateStock(request.QuantityInStock.Value);
	}
}
