using Microsoft.AspNetCore.Mvc;
using Moq;
using Products.Api.Controllers;
using Products.Application.Services;
using Products.Application.Validators;
using Products.Contracts.Responses;
using Products.Domain.Entities;
using Products.Tests.MockData;

namespace Products.Tests;

public class ProductsControllerV2Tests
{
	private readonly Mock<IProductService> _mockProductService;
	private readonly ProductsControllerV2 _controller;
	private readonly CancellationToken _token = CancellationToken.None;
	private readonly GetAllProductsRequestValidator _validator;

	public ProductsControllerV2Tests()
	{
		_mockProductService = new Mock<IProductService>();
		_controller = new ProductsControllerV2(_mockProductService.Object);
		_validator = new GetAllProductsRequestValidator();
	}

	[Fact]
	public async Task GetAll_ReturnsOk_WithMappedProductResponses()
	{
		var products = MockProductData.GetAllProducts();
		var request = MockProductData.GetAllProductsRequest();

		_mockProductService.Setup(s =>
			s.GetAllPagedAsync(It.IsAny<GetAllProductsOptions>(), default))
			.ReturnsAsync(products);

		var result = await _controller.GetAll(request, _token);

		var okResult = Assert.IsType<OkObjectResult>(result);
		var response = Assert.IsAssignableFrom<IEnumerable<ProductResponse>>(okResult.Value);

		Assert.Equal(3, response.Count());
		Assert.Contains(response, r => r.Name == "Laptop");
		Assert.Contains(response, r => r.Name == "Phone");
		Assert.Contains(response, r => r.Name == "Tablet");
	}

	[Fact]
	public async Task GetAll_ReturnsOk_WithEmptyList()
	{
		var products = new List<Product>();
		var request = MockProductData.GetAllProductsRequest();

		_mockProductService.Setup(s =>
			s.GetAllPagedAsync(It.IsAny<GetAllProductsOptions>(), _token))
			.ReturnsAsync(products);

		var result = await _controller.GetAll(request, _token);

		var okResult = Assert.IsType<OkObjectResult>(result);
		var response = Assert.IsAssignableFrom<IEnumerable<ProductResponse>>(okResult.Value);
		Assert.Empty(response);
	}
}
