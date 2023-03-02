using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests")]

namespace WebApi.Dtos.Validators;

internal static class OrderDtoValidator
{
	public static bool TryValidate(OrderDto order, out ValidationResult? result)
	{
		if (!order.OrderDate.HasValue)
		{
			result = new ValidationResult(
				$"{nameof(order)}.{nameof(order.OrderDate)} is required.",
				new[] { nameof(order.OrderDate) });
			return false;
		}

		if (order.ShippingAddress is null)
		{
			result = new ValidationResult(
				"{nameof(order)}.{nameof(order.ShippingAddress)} is required.",
				new[] { nameof(order.ShippingAddress) });
			return false;
		}

		if (order.OrderItems == null)
		{
			result = new ValidationResult(
				$"{nameof(order)}.{nameof(order.OrderItems)} is required.",
				new[] { nameof(order.OrderItems) });
			return false;
		}

		result = null;
		return true;
	}
}
