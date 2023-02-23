using Domain;
using Domain.Builders;
using Infrastructure.Dtos.Validators;

namespace Infrastructure.Dtos.Translators;

internal static class OrderDtoTranslator
{
	public static Order ToDomain(this OrderDto order)
	{
		if (!OrderDtoValidator.TryValidate(order, out var result))
		{
			throw new ArgumentException(result!.ErrorMessage, nameof(order));
		}

		var builder = new OrderBuilder()
			.At(DateTime.SpecifyKind(order.DateTime!.Value, DateTimeKind.Utc))
			.ShippingTo(order.ShippingAddress!.ToDomain());

		if (order.BillingAddress is not null)
		{
			builder.BillingTo(order.BillingAddress.ToDomain());
		}

		foreach (var p in order.OrderItems!)
		{
			var domain = p.ToDomain();
			builder.WithProduct(domain.SkuText, domain.UnitPrice, domain.UnitQuantity);
		}

		return builder.Build();
	}

	public static OrderDto FromDomain(Guid orderId, Order order)
	{
		return new OrderDto
		{
			Id = orderId.ToString(),
			DateTime = order.DateTime,
			OrderItems = order.OrderItems.FromDomain(),
			ShippingAddress = order.ShippingAddress.FromDomain(),
			BillingAddress = order.BillingAddress.FromDomain(),
		};
	}

	public static IEnumerable<OrderItemDto> FromDomain(this IEnumerable<OrderLineItem> orderItems)
	{
		return orderItems.Select(e => new OrderItemDto
			{
				SkuText = e.SkuText,
				Quantity = e.UnitQuantity,
				Price = e.UnitPrice
			})
			.ToList();
	}

	public static PostalAddressDto FromDomain(PostalAddress postalAddress)
	{
		return new PostalAddressDto
		{
			StreetAddress = postalAddress.StreetAddress,
			CityName = postalAddress.CityName,
			StateName = postalAddress.StateName,
			PostalCodeText = postalAddress.PostalCodeText,
			AttentionText = postalAddress.AttentionText,
			AlternateLocationText = postalAddress.AlternateLocationText
		};
	}
}
