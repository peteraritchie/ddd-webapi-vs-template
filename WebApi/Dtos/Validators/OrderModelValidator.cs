using FluentValidation;

namespace WebApi.Dtos.Validators
{
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
}
