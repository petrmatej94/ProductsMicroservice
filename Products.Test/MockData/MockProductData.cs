using Products.Contracts.Requests;
using Products.Domain.Entities;

namespace Products.Tests.MockData;

public static class MockProductData
{
	public static Product GetLaptopProduct()
	{
		return new Product("Laptop", "image.jpg", 25000, "Lenovo laptop", 10);
	}

	public static Product GetPhoneProduct()
	{
		return new Product("Phone", "image2.jpg", 8000, "Samsung smartphone", 5);
	}

	public static Product GetTabletProduct()
	{
		return new Product("Tablet", "image3.jpg", 9000, "Xiaomi tablet", 3);
	}

	public static IEnumerable<Product> GetAllProducts()
	{
		return new List<Product>
		{
			GetLaptopProduct(),
			GetPhoneProduct(),
			GetTabletProduct()
		};
	}

	public static CreateProductRequest GetCreateProductRequest()
	{
		return new CreateProductRequest
		{
			Name = "Laptop",
			ImageUrl = "image.jpg",
			Price = 25000,
			Description = "Lenovo laptop",
			QuantityInStock = 10
		};
	}

	public static PatchProductStockRequest GetPatchProductStockRequest(int? stock = 20)
	{
		return new PatchProductStockRequest
		{
			QuantityInStock = stock
		};
	}
}
