using PRI.Messaging.Primitives;

namespace Domain.Events;

public class OrderItemAdded : IEvent
{
	public OrderItemAdded(string correlationId, Order order)
	{
		CorrelationId = correlationId;
		Order = order;
		OccurredDateTime = DateTime.UtcNow;
	}

	public Order Order { get; }

	public string CorrelationId { get; set; }
	public DateTime OccurredDateTime { get; set; }
}
