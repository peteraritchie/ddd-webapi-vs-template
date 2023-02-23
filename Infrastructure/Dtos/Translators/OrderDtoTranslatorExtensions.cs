using Domain;

namespace Infrastructure.Dtos.Translators;

public static class OrderDtoTranslatorExtensions
{
	public static OrderItemDto FromDomain(this OrderLineItem lineItem)
	{
		return new OrderItemDto
		{
			SkuText = lineItem.SkuText,
			Quantity = lineItem.UnitQuantity,
			Price = lineItem.UnitPrice
		};
	}

	public static PostalAddressDto FromDomain(this PostalAddress address)
	{
		return new PostalAddressDto
		{
			AttentionText = address.AttentionText,
			StreetAddress = address.StreetAddress,
			AlternateLocationText = address.AlternateLocationText,
			CityName = address.CityName,
			StateName = address.StateName,
			PostalCodeText = address.PostalCodeText
		};
	}

	public static OrderLineItem ToDomain(this OrderItemDto orderItem)
	{
		if (orderItem.Quantity == null)
		{
			throw new ArgumentException($"{nameof(orderItem)}.{nameof(orderItem.Quantity)} should not be null.",
				nameof(orderItem));
		}

		if (orderItem.Quantity is < 1 or >= ushort.MaxValue)
		{
			throw new ArgumentOutOfRangeException(nameof(orderItem), orderItem.Quantity,
				$"orderItem.UnitQuantity should be greater than 0 and less than {ushort.MaxValue}");
		}

		if (orderItem.SkuText == null)
		{
			throw new ArgumentException($"{nameof(orderItem)}.{nameof(orderItem.SkuText)} should not be null.",
				nameof(orderItem));
		}

		if (orderItem.Price == null)
		{
			throw new ArgumentException($"{nameof(orderItem)}.{nameof(orderItem.Price)} should not be null.",
				nameof(orderItem));
		}

		return new OrderLineItem(orderItem.SkuText, (ushort)orderItem.Quantity,
			orderItem.Price.Value);
	}

	public static PostalAddress ToDomain(this PostalAddressDto address)
	{
		ArgumentNullException.ThrowIfNull(address);

		if (address.StreetAddress == null)
		{
			throw new ArgumentException($"{nameof(address)}.{nameof(address.StreetAddress)} should not be null.",
				nameof(address));
		}

		if (address.CityName == null)
		{
			throw new ArgumentException($"{nameof(address)}.{nameof(address.CityName)} should not be null.",
				nameof(address));
		}

		if (address.StateName == null)
		{
			throw new ArgumentException($"{nameof(address)}.{nameof(address.StateName)} should not be null.",
				nameof(address));
		}

		if (address.PostalCodeText == null)
		{
			throw new ArgumentException($"{nameof(address)}.{nameof(address.PostalCodeText)} should not be null.",
				nameof(address));
		}

		return new PostalAddress(address.StreetAddress, address.CityName, address.StateName,
			address.PostalCodeText, address.AlternateLocationText, address.AttentionText);
	}
}
