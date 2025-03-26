using Microsoft.AspNetCore.Mvc;
using Products.Application.Mapping;
using Products.Application.Services;
using Products.Contracts.Requests;
using Products.Contracts.Responses;
using Products.Domain.Entities;

namespace Products.Api.Controllers;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/products")]
public class ProductsControllerV2 : ControllerBase
{
	private readonly IProductService _productService;

	public ProductsControllerV2(IProductService productService)
	{
		_productService = productService;
	}

	/// <summary>
	/// Get all products with pagination (default page size 10)
	/// </summary>
	/// <param name="request"></param>
	/// <param name="token">Cancellation token</param>
	/// <returns>Paged list of products</returns>
	[HttpGet]
	[ProducesResponseType(typeof(IEnumerable<ProductResponse>), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetAll([FromQuery] GetAllProductsRequest request, CancellationToken token = default)
	{
		GetAllProductsOptions options = request.MapToOptions();
		IEnumerable<Product> products = await _productService.GetAllPagedAsync(options, token);
		IEnumerable<ProductResponse> response = products.MapToResponse();
		return Ok(response);
	}
}
