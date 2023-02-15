using Core;
using Core.Commands;
using Core.Events;
using Core.Interfaces;
using PRI.Messaging.Patterns.Extensions.Bus;
using PRI.Messaging.Primitives;

namespace Infrastructure
{
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
            orderRepository.Create(orderId, order);
            bus.Publish(
                new OrderCreated(Guid.Parse(command.CorrelationId), orderId, order));
        }
    }
}

public class AddOrderItemHandler: IConsumer<AddOrderItem>
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
        var order = orderRepository.Get(command.OrderId);

        order.AddItem(command.SkuText, command.UnitQuantity, command.UnitPrice);

        orderRepository.Update(command.OrderId, order);
        bus.Publish(new OrderItemAdded(command.CorrelationId, order));
    }
}
