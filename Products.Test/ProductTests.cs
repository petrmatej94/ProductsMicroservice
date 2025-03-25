using Products.Domain.Entities;

namespace Products.Tests;

public class ProductTests
{
	[Theory]
	[InlineData(10, 20)]
	[InlineData(0, 0)]
	[InlineData(-1, -1)]
	public void UpdateStock_ShouldUpdateStock_WhenValid(int initialStock, int newStock)
	{
		var product = new Product("Phone", "image.jpg", 500, "Smartphone", initialStock);

		if (newStock < 0)
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => product.UpdateStock(newStock));
		}
		else
		{
			product.UpdateStock(newStock);

			Assert.Equal(newStock, product.QuantityInStock);
		}
	}
}
