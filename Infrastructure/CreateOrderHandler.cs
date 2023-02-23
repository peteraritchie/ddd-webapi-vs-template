using Domain;
using Domain.Abstractions;
using Domain.Commands;
using Domain.Events;
using PRI.Messaging.Patterns.Extensions.Bus;
using PRI.Messaging.Primitives;

namespace Infrastructure;

public class CreateOrderHandler : IConsumer<CreateOrder>
{
	private readonly IBus bus;
	private readonly IOrderRepository orderRepository;

	public CreateOrderHandler(IBus bus, IOrderRepository orderRepository)
	{
		this.bus = bus;
		this.orderRepository = orderRepository;
	}

	public void Handle(CreateOrder command)
	{
		var orderId = Guid.NewGuid();
		var order = new Order(DateTime.UtcNow, command.OrderItems, command.ShippingAddress);
		orderRepository.CreateAsync(orderId, order);
		bus.Publish(
			new OrderCreated(Guid.Parse(command.CorrelationId), orderId, order));
	}
}
