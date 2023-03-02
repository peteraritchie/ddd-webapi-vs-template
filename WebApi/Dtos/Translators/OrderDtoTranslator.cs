using Domain;
using Domain.Builders;
using WebApi.Dtos.Validators;

namespace WebApi.Dtos.Translators
{
	public static class OrderDtoTranslator
	{
		/// <summary>
		///     Translate an OrderTo to a domain Order
		/// </summary>
		/// <param name="order">The DTO</param>
		/// <returns>A domain object</returns>
		/// <exception cref="ArgumentException">When the <paramref name="order" /> properties are not valid.</exception>
		public static Order ToDomain(this OrderDto order)
		{
			if (!OrderDtoValidator.TryValidate(
				    order,
				    out var result))
			{
				throw new ArgumentException(
					result!.ErrorMessage,
					nameof(order));
			}

			var builder = new OrderBuilder()
				.At(
					DateTime.SpecifyKind(
						order.OrderDate!.Value.DateTime,
						DateTimeKind.Utc))
				.ShippingTo(order.ShippingAddress!.ToDomain());

			if (order.BillingAddress is not null)
			{
				builder.BillingTo(order.BillingAddress.ToDomain());
			}

			foreach (var p in order.OrderItems!)
			{
				var domain = p.ToDomain();
				builder.WithProduct(
					domain.SkuText,
					domain.UnitPrice,
					domain.UnitQuantity);
			}

			return builder.Build();
		}

		public static OrderDto FromDomain(this Order order)
		{
			return new OrderDto
			{
				OrderDate = order.DateTime,
				OrderItems = order.OrderItems.Select(i => i.FromDomain()),
				ShippingAddress = order.ShippingAddress.FromDomain(),
				BillingAddress = order.BillingAddress?.FromDomain()
			};
		}

		public static OrderLineItem ToDomain(OrderItemDto item)
		{
			return item.ToDomain();
		}

		public static PostalAddress ToDomain(PostalAddressDto address)
		{
			return address.ToDomain();
		}
	}
}
