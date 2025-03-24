using Microsoft.AspNetCore.Mvc;
using Moq;
using Products.Api.Controllers;
using Products.Application.Services;
using Products.Contracts.Requests;
using Products.Contracts.Responses;
using Products.Domain.Entities;

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
		var product = new Product("Laptop", "image.jpg", 1000, "Description", 10);
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
		_mockProductService.Setup(service => service.GetByIdAsync(productId)).ReturnsAsync((Product)null);

		var result = await _controller.Get(productId);

		Assert.IsType<NotFoundResult>(result);
	}

	[Fact]
	public async Task GetAll_ReturnsOk_WhenProductsExist()
	{
		var products = new List<Product>
		{
			new ("Laptop", "image1.jpg", 1000, "Description", 10),
			new ("Phone", "image2.jpg", 500, "Description", 5),
			new ("PC", "image3.jpg", 500, "Description", 1)
		};
		_mockProductService.Setup(service => service.GetAllAsync()).ReturnsAsync(products);

		var result = await _controller.GetAll();

		var actionResult = Assert.IsType<OkObjectResult>(result);
		var response = Assert.IsAssignableFrom<IEnumerable<ProductResponse>>(actionResult.Value);
		Assert.Equal(3, response.Count());
	}

	[Fact]
	public async Task Create_ReturnsCreatedAtAction_WhenProductIsCreated()
	{
		var request = new CreateProductRequest
		{
			Name = "Laptop",
			ImageUrl = "image.jpg",
			Price = 1000,
			Description = "A great laptop",
			QuantityInStock = 10
		};
		var product = new Product("Laptop", "image.jpg", 1000, "A great laptop", 10);
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
		var request = new PatchProductRequest
		{
			Name = "Updated Laptop",
			ImageUrl = "updatedImage.jpg",
			Price = 1500,
			QuantityInStock = 15
		};
		var product = new Product("Laptop", "image.jpg", 1000, "A great laptop", 10);
		_mockProductService.Setup(service => service.GetByIdAsync(productId)).ReturnsAsync(product);
		_mockProductService.Setup(service => service.UpdateAsync(It.IsAny<Product>())).ReturnsAsync(product);

		var result = await _controller.Patch(productId, request);

		var actionResult = Assert.IsType<OkObjectResult>(result);
		var response = Assert.IsType<ProductResponse>(actionResult.Value);
		Assert.Equal("Updated Laptop", response.Name);
		Assert.Equal(1500, response.Price);
	}

	[Fact]
	public async Task Patch_ReturnsNotFound_WhenProductDoesNotExist()
	{
		var productId = Guid.NewGuid();
		var request = new PatchProductRequest
		{
			Name = "Updated Laptop",
			ImageUrl = "updatedImage.jpg",
			Price = 1500,
			QuantityInStock = 15
		};
		_mockProductService.Setup(service => service.GetByIdAsync(productId)).ReturnsAsync((Product)null);

		var result = await _controller.Patch(productId, request);

		Assert.IsType<NotFoundResult>(result);
	}
}