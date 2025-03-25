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

	[HttpGet(ApiEndpoints.Products.Get)]
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

	[HttpGet(ApiEndpoints.Products.GetAll)]
	public async Task<IActionResult> GetAll()
	{
		IEnumerable<Product> products = await _productService.GetAllAsync();
		IEnumerable<ProductResponse> response = products.MapToResponse();
		return Ok(response);
	}

	[HttpPost(ApiEndpoints.Products.Create)]
	public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
	{
		Product? product = request.MapToProduct();
		await _productService.CreateAsync(product);

		ProductResponse response = product.MapToResponse();
		return CreatedAtAction(nameof(Get), new { id = product.Id }, response);
	}

	[HttpPost(ApiEndpoints.Products.Patch)]
	public async Task<IActionResult> Patch([FromRoute] Guid id, [FromBody]PatchProductRequest request)
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
