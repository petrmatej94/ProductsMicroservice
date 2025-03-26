using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Products.Application.Interfaces;
using Products.Application.Services;
using Products.Contracts.Messages;
using Products.Domain.Entities;

namespace Products.Infrastructure.Queue;

public class StockUpdateBackgroundService : BackgroundService
{
	private readonly IStockUpdateQueue _queue;
	private readonly IServiceProvider _serviceProvider;
	private readonly ILogger<StockUpdateBackgroundService> _logger;

	public StockUpdateBackgroundService(IStockUpdateQueue queue, IServiceProvider serviceProvider, ILogger<StockUpdateBackgroundService> logger)
	{
		_queue = queue;
		_serviceProvider = serviceProvider;
		_logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken token)
	{
		while (!token.IsCancellationRequested)
		{
			if (_queue.TryDequeue(out StockUpdateMessage? message))
			{
				using var scope = _serviceProvider.CreateScope();
				var productService = scope.ServiceProvider.GetRequiredService<IProductService>();

				Product? product = await productService.GetByIdAsync(message.ProductId, token);
				if (product != null)
				{
					product.UpdateStock(message.NewQuantity);
					await productService.UpdateAsync(product, token);
					_logger.LogInformation("Updated stock for product {ProductId} to {Quantity}", message.ProductId, message.NewQuantity);
				}
			}

			await Task.Delay(200, token);
		}
	}
}
