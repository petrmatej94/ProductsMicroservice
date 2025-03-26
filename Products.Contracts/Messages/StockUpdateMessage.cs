namespace Products.Contracts.Messages;

public class StockUpdateMessage
{
	public Guid ProductId { get; set; }
	public int NewQuantity { get; set; }
}
