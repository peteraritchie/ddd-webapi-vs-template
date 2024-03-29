﻿namespace Domain;

public record Order
{
	private readonly List<OrderLineItem> orderItems;

	public Order(DateTime dateTime, IEnumerable<OrderLineItem> orderItems, PostalAddress shippingAddress,
		PostalAddress? billingAddress = default)
	{
		DateTime = dateTime;
		this.orderItems = new List<OrderLineItem>(orderItems);
		ShippingAddress = shippingAddress;
		BillingAddress = billingAddress;
	}

	public DateTime DateTime { get; }
	public IEnumerable<OrderLineItem> OrderItems => orderItems.AsReadOnly();
	public PostalAddress ShippingAddress { get; }
	public PostalAddress? BillingAddress { get; }

	public void AddItem(string sku, ushort quantity, decimal price)
	{
		orderItems.Add(
			new OrderLineItem(
				sku,
				quantity,
				price));
	}

	public void AddItem(OrderLineItem item)
	{
		orderItems.Add(item with { });
	}
}