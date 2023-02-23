using Domain.Abstractions;
using Domain.Commands;
using Domain.Events;
using PRI.Messaging.Patterns.Extensions.Bus;
using PRI.Messaging.Primitives;

namespace Infrastructure;

public class AddOrderItemHandler : IConsumer<AddOrderItem>
{
	private readonly IBus bus;
	private readonly IOrderRepository orderRepository;

	public AddOrderItemHandler(IBus bus, IOrderRepository orderRepository)
	{
		this.bus = bus;
		this.orderRepository = orderRepository;
	}

	public void Handle(AddOrderItem command)
	{
		var order = orderRepository.GetAsync(command.OrderId).Result;

		order.AddItem(command.SkuText, command.UnitQuantity, command.UnitPrice);

		_ = orderRepository.UpdateAsync(command.OrderId, order).Result;
		bus.Publish(new OrderItemAdded(command.CorrelationId, order));
	}
}
