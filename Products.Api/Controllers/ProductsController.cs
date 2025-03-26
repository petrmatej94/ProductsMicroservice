using Microsoft.AspNetCore.Mvc;
using Products.Application.Mapping;
using Products.Application.Services;
using Products.Contracts.Requests;
using Products.Contracts.Responses;
using Products.Domain.Entities;

namespace Products.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/products")]
public class ProductsController : ControllerBase
{
	private readonly IProductService _productService;

	public ProductsController(IProductService productService)
	{
		_productService = productService;
	}

	/// <summary>
	/// Get product by ID
	/// </summary>
	/// <param name="id">Product ID</param>
	/// <param name="token">Cancellation token.</param>
	/// <returns>Product if found, 404 otherwise</returns>
	[HttpGet("{id:guid}")]
	[ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken token)
	{
		Product? product = await _productService.GetByIdAsync(id, token);
		if (product == null)
		{
			return NotFound();
		}

		var response = product.MapToResponse();
		return Ok(response);
	}

	/// <summary>
	/// Get all products
	/// </summary>
	/// <param name="token">Cancellation token.</param>
	/// <returns>List of products (can be empty)</returns>
	[HttpGet]
	[ProducesResponseType(typeof(IEnumerable<ProductResponse>), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetAll(CancellationToken token)
	{
		IEnumerable<Product> products = await _productService.GetAllAsync(token);
		IEnumerable<ProductResponse> response = products.MapToResponse();
		return Ok(response);
	}

	/// <summary>
	/// Creates a new product
	/// </summary>
	/// <param name="request">Product creation data</param>
	/// <param name="token">Cancellation token.</param>
	/// <returns>The newly created product.</returns>
	[HttpPost]
	[ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Create([FromBody] CreateProductRequest request, CancellationToken token)
	{
		Product? product = request.MapToProduct();
		await _productService.CreateAsync(product, token);

		ProductResponse response = product.MapToResponse();
		return CreatedAtAction(nameof(Get), new { id = product.Id }, response);
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

		Product? product = await _productService.GetByIdAsync(id, token);
		if (product == null)
		{
			return NotFound();
		}

		product.UpdateFromRequest(request);

		Product? updated = await _productService.UpdateAsync(product, token);
		if (updated == null)
		{
			return NotFound();
		}

		ProductResponse response = updated.MapToResponse();
		return Ok(response);
	}
}
