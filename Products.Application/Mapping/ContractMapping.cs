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
			imageUrl: request.ImageUrl
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

	public static void UpdateFromRequest(this Product product, PatchProductStockRequest request)
	{
		if (request.QuantityInStock.HasValue)
			product.UpdateStock(request.QuantityInStock.Value);
	}

	public static GetAllProductsOptions MapToOptions(this GetAllProductsRequest request)
	{
		return new GetAllProductsOptions
		{
			Page = request.Page,
			PageSize = request.PageSize
		};
	}
}
