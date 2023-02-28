using System.ComponentModel.DataAnnotations;
using FluentValidation;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Tests")]

namespace WebApi.Dtos.Validators;


internal class OrderModelValidator : AbstractValidator<OrderDto>
{
	public OrderModelValidator()
	{
		RuleFor(order => order.OrderDate)
			.NotEmpty();
		RuleFor(order => order.ShippingAddress)
			.NotNull();
		RuleFor(order => order.OrderItems)
			.NotEmpty();
	}
}


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
