using Application.UseCases;
using Domain.Commands;
using Domain.Events;
using PRI.Messaging.Patterns.Extensions.Bus;
using PRI.Messaging.Primitives;

namespace Application;

public class CreateOrderHandler : IConsumer<CreateOrder>
{
	private readonly IBus bus;
	private readonly OrderService orderService;

	public CreateOrderHandler(IBus bus, OrderService orderService)
	{
		this.bus = bus;
		this.orderService = orderService;
	}

	public void Handle(CreateOrder command)
	{
		var (orderId, order) = orderService.Create(DateTime.UtcNow, command.OrderItems, command.ShippingAddress).Result;
		bus.Publish(
			new OrderCreated(Guid.Parse(command.CorrelationId), orderId, order));
	}
}
