using Products.Domain.Entities;
using Products.Tests.MockData;

namespace Products.Tests;

public class ProductTests
{
	[Fact]
	public void Constructor_ShouldThrowException_WhenInitialStockIsNegative()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() => new Product("Test", "https://image.jpg", 1000, "Desc", -1));
	}

	[Theory]
	[InlineData(10, 20)]
	[InlineData(0, 0)]
	[InlineData(5, 0)]
	public void UpdateStock_ShouldUpdateStock_WhenValid(int initialStock, int newStock)
	{
		var product = MockProductData.GetPhoneProduct(initialStock);

		product.UpdateStock(newStock);

		Assert.Equal(newStock, product.QuantityInStock);
	}

	[Fact]
	public void UpdateStock_ShouldThrowException_WhenNewStockIsNegative()
	{
		var product = MockProductData.GetPhoneProduct(5);

		Assert.Throws<ArgumentOutOfRangeException>(() => product.UpdateStock(-1));
	}
}
