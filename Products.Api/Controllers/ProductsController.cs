using Microsoft.AspNetCore.Mvc;
using Products.Application.Mapping;
using Products.Application.Services;
using Products.Contracts.Requests;
using Products.Contracts.Responses;
using Products.Domain.Entities;

namespace Products.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
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
	/// <returns>Product if found, 404 otherwise</returns>
	[HttpGet(ApiEndpoints.Products.Get)]
	[ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Get([FromRoute] Guid id)
	{
		Product? product = await _productService.GetByIdAsync(id);
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
	/// <returns>List of products (can be empty)</returns>
	[HttpGet(ApiEndpoints.Products.GetAll)]
	[ProducesResponseType(typeof(IEnumerable<ProductResponse>), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetAll()
	{
		IEnumerable<Product> products = await _productService.GetAllAsync();
		IEnumerable<ProductResponse> response = products.MapToResponse();
		return Ok(response);
	}

	/// <summary>
	/// Creates a new product
	/// </summary>
	/// <param name="request">Product creation data</param>
	/// <returns>The newly created product.</returns>
	[HttpPost(ApiEndpoints.Products.Create)]
	[ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
	{
		Product? product = request.MapToProduct();
		await _productService.CreateAsync(product);

		ProductResponse response = product.MapToResponse();
		return CreatedAtAction(nameof(Get), new { id = product.Id }, response);
	}

	/// <summary>
	/// Updates product stock information
	/// </summary>
	/// <param name="id">ID of the product to update</param>
	/// <param name="request">Stock update payload</param>
	/// <returns>The updated product or appropriate error</returns>
	[HttpPatch(ApiEndpoints.Products.Patch)]
	[ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> PatchStock([FromRoute] Guid id, [FromBody] PatchProductStockRequest request)
	{
		if (request is null)
		{
			return BadRequest("Nothing to update.");
		}

		Product? product = await _productService.GetByIdAsync(id);
		if (product == null)
		{
			return NotFound();
		}

		product.UpdateFromRequest(request);

		Product? updated = await _productService.UpdateAsync(product);
		if (updated == null)
		{
			return NotFound();
		}

		ProductResponse response = updated.MapToResponse();
		return Ok(response);
	}
}
