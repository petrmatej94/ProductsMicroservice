using Microsoft.AspNetCore.Mvc;
using Moq;
using Products.Api.Controllers;
using Products.Application.Interfaces;
using Products.Application.Services;
using Products.Contracts.Messages;
using Products.Contracts.Requests;
using Products.Contracts.Responses;
using Products.Domain.Entities;
using Products.Tests.MockData;

namespace Products.Tests;

public class ProductsControllerV2Tests
{
	private readonly Mock<IProductService> _mockProductService;
	private readonly Mock<IStockUpdateQueue> _mockQueue;
	private readonly ProductsControllerV2 _controller;
	private readonly CancellationToken _token = CancellationToken.None;

	public ProductsControllerV2Tests()
	{
		_mockProductService = new Mock<IProductService>();
		_mockQueue = new Mock<IStockUpdateQueue>();
		_controller = new ProductsControllerV2(_mockProductService.Object, _mockQueue.Object);
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

	[Fact]
	public async Task PatchStock_ValidRequest_EnqueuesMessage_ReturnsAccepted()
	{
		var productId = Guid.NewGuid();
		var controller = new ProductsControllerV2(_mockProductService.Object, _mockQueue.Object);
		var request = MockProductData.GetPatchProductStockRequest();

		var result = await controller.PatchStock(productId, request, _token);

		_mockQueue.Verify(q => q.Enqueue(It.Is<StockUpdateMessage>(
			m => m.ProductId == productId && m.NewQuantity == 15)), Times.Once);

		Assert.IsType<AcceptedResult>(result);
	}

	[Fact]
	public async Task PatchStock_NullQuantity_ReturnsBadRequest()
	{
		var controller = new ProductsControllerV2(_mockProductService.Object, _mockQueue.Object);
		var request = new PatchProductStockRequest { QuantityInStock = null };
		
		var result = await controller.PatchStock(Guid.NewGuid(), request, _token);

		Assert.IsType<BadRequestObjectResult>(result);
	}
}
