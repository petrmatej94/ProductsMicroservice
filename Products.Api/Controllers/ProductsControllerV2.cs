using Microsoft.AspNetCore.Mvc;
using Products.Application.Interfaces;
using Products.Application.Mapping;
using Products.Application.Services;
using Products.Contracts.Messages;
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
	private readonly IStockUpdateQueue _stockUpdateQueue;

	public ProductsControllerV2(IProductService productService, IStockUpdateQueue stockUpdateQueue)
	{
		_productService = productService;
		_stockUpdateQueue = stockUpdateQueue;
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

	/// <summary>
	/// Updates product stock information
	/// </summary>
	/// <param name="id">ID of the product to update</param>
	/// <param name="request">Stock update payload</param>
	/// <param name="token">Cancellation token.</param>
	/// <returns>The updated product or appropriate error</returns>
	[HttpPatch("{id:guid}")]
	[ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> PatchStock([FromRoute] Guid id, [FromBody] PatchProductStockRequest request, CancellationToken token)
	{
		if (request == null || request.QuantityInStock == null)
		{
			return BadRequest("Nothing to update.");
		}

		_stockUpdateQueue.Enqueue(new StockUpdateMessage
		{
			ProductId = id,
			NewQuantity = request.QuantityInStock.Value
		});

		return Accepted();
	}
}
