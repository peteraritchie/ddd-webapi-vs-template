using Domain.Builders.Validators;
using PRI.Messaging.Primitives;

namespace Domain.Commands
{
	public class CreateOrder : ICommand
	{
		public CreateOrder(Guid correlationId, DateTime dateTime, IEnumerable<OrderLineItem> orderItems,
			PostalAddress shippingAddress)
			: this(correlationId, dateTime, orderItems, shippingAddress, null)
		{
		}

		public CreateOrder(Guid correlationId, Order order)
			: this(correlationId, order.DateTime, order.OrderItems, order.ShippingAddress, order.BillingAddress)
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

			if (!PostalAddressParametersValidator.TryValidate(
				    shippingAddress.StreetAddress,
				    shippingAddress.CityName,
				    shippingAddress.StateName,
				    shippingAddress.PostalCodeText,
				    shippingAddress.AlternateLocationText,
				    shippingAddress.AttentionText, out var shippingAddressValidationResult))
			{
				throw new ArgumentException(shippingAddressValidationResult!.ErrorMessage, shippingAddressValidationResult.MemberNames.First());
			}

			if (billingAddress != null && !PostalAddressParametersValidator.TryValidate(
				    billingAddress.StreetAddress,
				    billingAddress.CityName,
				    billingAddress.StateName,
				    billingAddress.PostalCodeText,
				    billingAddress.AlternateLocationText,
					billingAddress.AttentionText, out var billingAddressValidationResult))
			{
				throw new ArgumentException(billingAddressValidationResult!.ErrorMessage, billingAddressValidationResult.MemberNames.First());
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
}
