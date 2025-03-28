using Products.Contracts.Requests;
using Products.Domain.Entities;

namespace Products.Tests.MockData;

public static class MockProductData
{
	public static Product GetLaptopProduct()
	{
		return new Product("Laptop", "https://image.jpg", 25000, "Lenovo laptop", 10);
	}

	public static Product GetPhoneProduct(int initialStock = 5)
	{
		return new Product("Phone", "https://image2.jpg", 8000, "Samsung smartphone", initialStock);
	}

	public static Product GetTabletProduct()
	{
		return new Product("Tablet", "https://image3.jpg", 9000, "Xiaomi tablet", 3);
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

	public static GetAllProductsRequest GetAllProductsRequest()
	{
		return new GetAllProductsRequest
		{ 
			Page = 1, 
			PageSize = 10 
		};
	}

	public static CreateProductRequest GetCreateProductRequest()
	{
		return new CreateProductRequest
		{
			Name = "Laptop",
			ImageUrl = "https://image.jpg"
		};
	}

	public static PatchProductStockRequest GetPatchProductStockRequest(int? stock = 20)
	{
		return new PatchProductStockRequest
		{
			QuantityInStock = stock
		};
	}

	public static PatchProductStockRequest GetPatchProductStockRequest()
	{
		return new PatchProductStockRequest
		{
			QuantityInStock = 15
		};
	}
}
