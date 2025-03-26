using Products.Contracts.Messages;

namespace Products.Application.Interfaces;

public interface IStockUpdateQueue
{
	void Enqueue(StockUpdateMessage message);
	bool TryDequeue(out StockUpdateMessage? message);
}
