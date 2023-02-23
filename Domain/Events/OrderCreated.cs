using PRI.Messaging.Primitives;

namespace Domain.Events;

public class OrderCreated : IEvent
{
	public OrderCreated(Guid correlationId, Guid orderId, Order order)
	{
		OccurredDateTime = DateTime.UtcNow;
		CorrelationId = correlationId.ToString();
		OrderId = orderId;
		Order = order;
	}

	public Guid OrderId { get; }

	public Order Order { get; }
	public string CorrelationId { get; set; }
	public DateTime OccurredDateTime { get; set; }
}
