using Domain;
using Domain.Abstractions;
using Domain.Builders;

namespace Application.UseCases
{
	public class OrderService
	{
		private readonly IOrderRepository repository;

		public OrderService(IOrderRepository repository)
		{
			this.repository = repository;
		}

		private static Order GenerateExampleOrder()
		{
			var builder = new OrderBuilder()
				.At(DateTime.UtcNow)
				.ShippingTo(new PostalAddress
				(
					streetAddress: "14544 ROGUE RIVER DR",
					cityName: "CHESTERFIELD",
					stateName: "MO",
					postalCodeText: "63017"
				))
				.WithProduct("someSku", 9.99m, 1);

			return builder.Build();
		}

		public async Task<(Guid, Order)> Create(DateTime dateTime, IEnumerable<OrderLineItem> orderItems, PostalAddress shippingAddress)
		{
			var orderId = Guid.NewGuid();
			var order = new Order(dateTime, orderItems, shippingAddress);
			await repository.CreateAsync(orderId, order);
			return (orderId, order);
		}
	}
}
