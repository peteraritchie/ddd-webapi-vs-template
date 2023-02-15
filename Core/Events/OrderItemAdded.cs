using PRI.Messaging.Primitives;

namespace Core.Events;

public class OrderItemAdded : IEvent
{
    public OrderItemAdded(string correlationId, Order order)
    {
        CorrelationId = correlationId;
        Order = order;
        OccurredDateTime = DateTime.UtcNow;
    }

    public string CorrelationId { get; set; }
    public Order Order { get; }
    public DateTime OccurredDateTime { get; set; }
}
