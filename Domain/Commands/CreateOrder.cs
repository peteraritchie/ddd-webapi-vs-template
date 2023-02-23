using PRI.Messaging.Primitives;

namespace Domain.Commands;

public class CreateOrder : ICommand
{
	public CreateOrder(Guid correlationId, DateTime dateTime, IEnumerable<OrderLineItem> orderItems,
		PostalAddress shippingAddress)
		: this(correlationId, dateTime, orderItems, shippingAddress, null)
	{
	}

	public CreateOrder(Guid correlationId, DateTime dateTime, IEnumerable<OrderLineItem> orderItems,
		PostalAddress shippingAddress,
		PostalAddress? billingAddress)
	{
		var orderItemList = orderItems.ToList();
		if (dateTime > DateTime.UtcNow.Date)
		{
			throw new ArgumentException("Order date/time should not be after today.", nameof(dateTime));
		}

		if (correlationId == Guid.Empty)
		{
			throw new ArgumentException("Order ID should not be empty.", nameof(correlationId));
		}

		if (!orderItemList.Any())
		{
			throw new ArgumentException("Order should include at least one line item.", nameof(orderItems));
		}

		if (!PostalAddress.TryValidate(shippingAddress))
		{
			throw new ArgumentException("Shipping address should be valid and not missing.",
				nameof(shippingAddress));
		}

		if (billingAddress != null && !PostalAddress.TryValidate(billingAddress))
		{
			throw new ArgumentException("Shipping address should be valid and not missing.",
				nameof(shippingAddress));
		}

		CorrelationId = correlationId.ToString();
		DateTime = dateTime;
		OrderItems = orderItemList;
		ShippingAddress = shippingAddress;
		BillingAddress = billingAddress;
	}

	public DateTime DateTime { get; }
	public IEnumerable<OrderLineItem> OrderItems { get; }
	public PostalAddress ShippingAddress { get; }
	public PostalAddress? BillingAddress { get; }
	public string CorrelationId { get; set; }
}
