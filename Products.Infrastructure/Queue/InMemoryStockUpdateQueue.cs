using Microsoft.Extensions.Logging;
using Products.Application.Interfaces;
using Products.Contracts.Messages;
using System.Collections.Concurrent;

namespace Products.Infrastructure.Queue;

public class InMemoryStockUpdateQueue : IStockUpdateQueue
{
	private readonly ConcurrentQueue<StockUpdateMessage> _queue = new();
	private readonly ILogger<InMemoryStockUpdateQueue> _logger;

	public InMemoryStockUpdateQueue(ILogger<InMemoryStockUpdateQueue> logger)
	{
		_logger = logger;
	}

	public void Enqueue(StockUpdateMessage message)
	{
		_queue.Enqueue(message);
		_logger.LogInformation("Enqueued stock update: ProductId={ProductId}, Quantity={Quantity}", message.ProductId, message.NewQuantity);
	}

	public bool TryDequeue(out StockUpdateMessage? message)
	{
		bool success = _queue.TryDequeue(out message);
		if (success && message != null)
		{
			_logger.LogInformation("Dequeued stock update: ProductId={ProductId}, Quantity={Quantity}", message.ProductId, message.NewQuantity);
		}
		return success;
	}
}
