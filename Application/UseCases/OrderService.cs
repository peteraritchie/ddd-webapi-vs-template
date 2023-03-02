using Domain;
using Domain.Abstractions;

namespace Application.UseCases;

public class OrderService
{
	private readonly IOrderRepository repository;

	public OrderService(IOrderRepository repository)
	{
		this.repository = repository;
	}

	public async Task<(Guid, Order)> Create(DateTime dateTime, IEnumerable<OrderLineItem> orderItems,
		PostalAddress shippingAddress)
	{
		var orderId = Guid.NewGuid();
		var order = new Order(
			dateTime,
			orderItems,
			shippingAddress);
		await repository.CreateAsync(
			orderId,
			order);
		return (orderId, order);
	}
}