using Microsoft.AspNetCore.Mvc;
using Moq;
using Products.Api.Controllers;
using Products.Application.Services;
using Products.Contracts.Responses;
using Products.Domain.Entities;
using Products.Tests.MockData;

namespace Products.Tests;

public class ProductsControllerTests
{
	private readonly Mock<IProductService> _mockProductService;
	private readonly ProductsController _controller;

	public ProductsControllerTests()
	{
		_mockProductService = new Mock<IProductService>();
		_controller = new ProductsController(_mockProductService.Object);
	}

	[Fact]
	public async Task Get_ReturnsOk_WhenProductExists()
	{
		var productId = Guid.NewGuid();
		var product = MockProductData.GetLaptopProduct();
		_mockProductService.Setup(service => service.GetByIdAsync(productId)).ReturnsAsync(product);

		var result = await _controller.Get(productId);

		var actionResult = Assert.IsType<OkObjectResult>(result);
		var response = Assert.IsType<ProductResponse>(actionResult.Value);
		Assert.Equal(product.Name, response.Name);
	}

	[Fact]
	public async Task Get_ReturnsNotFound_WhenProductDoesNotExist()
	{
		var productId = Guid.NewGuid();
		_mockProductService.Setup(service => service.GetByIdAsync(productId)).ReturnsAsync((Product)null!);

		var result = await _controller.Get(productId);

		Assert.IsType<NotFoundResult>(result);
	}

	[Fact]
	public async Task GetAll_ReturnsOk_WhenProductsExist()
	{
		var products = MockProductData.GetAllProducts();
		_mockProductService.Setup(service => service.GetAllAsync()).ReturnsAsync(products);

		var result = await _controller.GetAll();

		var actionResult = Assert.IsType<OkObjectResult>(result);
		var response = Assert.IsAssignableFrom<IEnumerable<ProductResponse>>(actionResult.Value);
		Assert.Equal(3, response.Count());
	}

	[Fact]
	public async Task GetAll_ReturnsEmptyList_WhenNoProductsExist()
	{
		_mockProductService.Setup(service => service.GetAllAsync()).ReturnsAsync(new List<Product>());

		var result = await _controller.GetAll();

		var actionResult = Assert.IsType<OkObjectResult>(result);
		var response = Assert.IsAssignableFrom<IEnumerable<ProductResponse>>(actionResult.Value);
		Assert.Empty(response);
	}

	[Fact]
	public async Task Create_ReturnsCreatedAtAction_WhenProductIsCreated()
	{
		var request = MockProductData.GetCreateProductRequest();
		var product = MockProductData.GetLaptopProduct();
		_mockProductService.Setup(service => service.CreateAsync(It.IsAny<Product>())).ReturnsAsync(product);

		var result = await _controller.Create(request);

		var actionResult = Assert.IsType<CreatedAtActionResult>(result);
		Assert.Equal("Get", actionResult.ActionName);

		var response = Assert.IsType<ProductResponse>(actionResult.Value);
		Assert.Equal(request.Name, response.Name);
		Assert.Equal(request.ImageUrl, response.ImageUrl);
		Assert.Equal(request.Price, response.Price);
		Assert.Equal(request.Description, response.Description);
		Assert.Equal(request.QuantityInStock, response.QuantityInStock);
	}

	[Fact]
	public async Task Patch_ReturnsOk_WhenProductIsUpdated()
	{
		var productId = Guid.NewGuid();
		var request = MockProductData.GetPatchProductStockRequest(15);
		var product = MockProductData.GetLaptopProduct();
		_mockProductService.Setup(service => service.GetByIdAsync(productId)).ReturnsAsync(product);
		_mockProductService.Setup(service => service.UpdateAsync(It.IsAny<Product>())).ReturnsAsync(product);

		var result = await _controller.PatchStock(productId, request);

		var actionResult = Assert.IsType<OkObjectResult>(result);
		var response = Assert.IsType<ProductResponse>(actionResult.Value);
		Assert.Equal(15, response.QuantityInStock);
	}

	[Fact]
	public async Task Patch_ReturnsNotFound_WhenProductDoesNotExist()
	{
		var productId = Guid.NewGuid();
		var request = MockProductData.GetPatchProductStockRequest(15);
		_mockProductService.Setup(service => service.GetByIdAsync(productId)).ReturnsAsync((Product)null!);

		var result = await _controller.PatchStock(productId, request);

		Assert.IsType<NotFoundResult>(result);
	}

	[Fact]
	public async Task Patch_ReturnsBadRequest_WhenRequestIsNull()
	{
		var productId = Guid.NewGuid();

		var result = await _controller.PatchStock(productId, null!);

		var badRequest = Assert.IsType<BadRequestObjectResult>(result);
		Assert.Equal("Nothing to update.", badRequest.Value);
	}

	[Fact]
	public async Task Patch_ReturnsBadRequest_WhenStockIsNull()
	{
		var productId = Guid.NewGuid();

		var request = MockProductData.GetPatchProductStockRequest(null);
		var result = await _controller.PatchStock(productId, request);

		var badRequest = Assert.IsType<BadRequestObjectResult>(result);
		Assert.Equal("Nothing to update.", badRequest.Value);
	}

	[Fact]
	public async Task Patch_ReturnsNotFound_WhenUpdateFails()
	{
		var productId = Guid.NewGuid();
		var request = MockProductData.GetPatchProductStockRequest(10);
		var existingProduct = MockProductData.GetLaptopProduct();

		_mockProductService.Setup(s => s.GetByIdAsync(productId)).ReturnsAsync(existingProduct);
		_mockProductService.Setup(s => s.UpdateAsync(It.IsAny<Product>())).ReturnsAsync((Product)null!);

		var result = await _controller.PatchStock(productId, request);

		Assert.IsType<NotFoundResult>(result);
	}
}