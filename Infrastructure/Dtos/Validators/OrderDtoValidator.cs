using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Dtos.Validators
{
	internal static class OrderDtoValidator
	{
		public static bool TryValidate(OrderDto order, out ValidationResult? result)
		{
			if (!order.DateTime.HasValue)
			{
				result = new ValidationResult(
					$"{nameof(order)}.{nameof(order.DateTime)} is required.",
					new[] { nameof(order.DateTime) });
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
}
