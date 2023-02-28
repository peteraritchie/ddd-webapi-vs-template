using WebApi.Dtos;
using WebApi.Dtos.Validators;

namespace Tests;

public class OrderValidatorShould
{
	[Fact]
	public async Task FlagEmptyOrderDate()
	{
		var order = new OrderDto
		{
			ShippingAddress = TestData.CreateShippingAddress(),
			OrderItems = TestData.CreateOrderItems()
		};

		var validator = new OrderModelValidator();
		var result = await validator.ValidateAsync(order)!;
		Assert.False(result.IsValid);
		Assert.Single(result.Errors);
		Assert.Equal(nameof(OrderDto.OrderDate), result.Errors.Single().PropertyName);
	}

	[Fact]
	public async Task FlagMissingShippingAddress()
	{
		var order = new OrderDto
		{
			OrderDate = TestData.OrderDate,
			OrderItems = TestData.CreateOrderItems()
		};

		var validator = new OrderModelValidator();
		var result = await validator.ValidateAsync(order)!;
		Assert.False(result.IsValid);
		Assert.Single(result.Errors);
		Assert.Equal(nameof(OrderDto.ShippingAddress), result.Errors.Single().PropertyName);
	}

	[Fact]
	public async Task FlagMissingOrderItems()
	{
		var order = new OrderDto
		{
			OrderDate = TestData.OrderDate,
			ShippingAddress = TestData.CreateShippingAddress(),
		};

		var validator = new OrderModelValidator();
		var result = await validator.ValidateAsync(order)!;
		Assert.False(result.IsValid);
		Assert.Single(result.Errors);
		Assert.Equal(nameof(OrderDto.OrderItems), result.Errors.Single().PropertyName);
	}
}
