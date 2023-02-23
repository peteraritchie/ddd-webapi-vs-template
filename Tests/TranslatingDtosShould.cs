using Domain;
using WebApi.Dtos;
using WebApi.Dtos.Translators;

namespace Tests;

public sealed class TranslatingDtosShould
{
	[Fact]
	public void ValidOrderItemDtoShouldTranslateSuccessfully()
	{
		OrderLineItem actual = TestData.OrderItem.ToDomain();
		Assert.Equal(TestData.OrderItem.SkuText, actual.SkuText);
		Assert.Equal(TestData.OrderItem.UnitPrice, actual.UnitPrice);
		Assert.Equal(TestData.OrderItem.UnitQuantity, actual.UnitQuantity);
	}

	[Fact]
	public void OrderItemDtoMissingQuantityShouldThrow()
	{
		var orderItem = TestData.OrderItem with { UnitQuantity = default };
		Assert.Throws<ArgumentException>(() => orderItem.ToDomain());
	}

	[Fact]
	public void OrderItemDtoMissingPriceShouldThrow()
	{
		var orderItem = TestData.OrderItem with { UnitPrice = default };
		Assert.Throws<ArgumentException>(() => orderItem.ToDomain());
	}

	[Fact]
	public void OrderItemDtoMissingSkuShouldThrow()
	{
		var orderItem = TestData.OrderItem with { SkuText = default };
		Assert.Throws<ArgumentException>(() => orderItem.ToDomain());
	}

	[Fact]
	public void OrderItemDtoZeroQuantityShouldThrow()
	{
		var orderItem = TestData.OrderItem with { UnitQuantity = 0 };
		Assert.Throws<ArgumentOutOfRangeException>(() => orderItem.ToDomain());
	}

	[Fact]
	public void OrderItemDtoInvalidQuantityShouldThrow()
	{
		var orderItem = TestData.OrderItem with { UnitQuantity = ushort.MaxValue };
		Assert.Throws<ArgumentOutOfRangeException>(() => orderItem.ToDomain());
	}

	[Fact]
	public void ThrowWithNullAddress()
	{
		_ = Assert.Throws<ArgumentNullException>(()=>default(PostalAddressDto).ToDomain());
	}

	[Fact]
	public void SucceedWithValidAddress()
	{
		var postalAddress = TestData.ShippingAddress.ToDomain();
		Assert.NotNull(postalAddress);
		Assert.Equal(TestData.ShippingAddress.AlternateLocationText, postalAddress.AlternateLocationText);
		Assert.Equal(TestData.ShippingAddress.AttentionText, postalAddress.AttentionText);
		Assert.Equal(TestData.ShippingAddress.PostalCodeText, postalAddress.PostalCodeText);
		Assert.Equal(TestData.ShippingAddress.StateName, postalAddress.StateName);
		Assert.Equal(TestData.ShippingAddress.StreetAddress, postalAddress.StreetAddress);
	}

	[Fact]
	public void ThrowsWithMissingStreetAddress()
	{
		var address = TestData.ShippingAddress with { StreetAddress = default};
		Assert.Throws<ArgumentException>(()=>address.ToDomain());
	}

	[Fact]
	public void ThrowsWithMissingCityName()
	{
		var address = TestData.ShippingAddress with { CityName = default};
		Assert.Throws<ArgumentException>(()=>address.ToDomain());
	}

	[Fact]
	public void ThrowsWithMissingStateName()
	{
		var address = TestData.ShippingAddress with { StateName = default};
		Assert.Throws<ArgumentException>(()=>address.ToDomain());
	}

	[Fact]
	public void ThrowsWithMissingPostalCodeText()
	{
		var address = TestData.ShippingAddress with { PostalCodeText= default};
		Assert.Throws<ArgumentException>(()=>address.ToDomain());
	}

	[Fact]
	public void SucceedWithMinimalOrder()
	{
		Assert.NotNull(TestData.MinimalOrder.ToDomain());
	}

	[Fact]
	public void SucceedWithBillingAddressOrder()
	{
		Assert.NotNull(TestData.Order.ToDomain());
	}

	[Fact]
	public void ThrowWithMissingOrderDate()
	{
		var order = TestData.MinimalOrder with {OrderDate = default};
		Assert.Throws<ArgumentException>(order.ToDomain);
	}

	[Fact]
	public void ThrowWithMissingShippingAddress()
	{
		var order = TestData.MinimalOrder with { ShippingAddress = default};
		Assert.Throws<ArgumentException>(order.ToDomain);
	}

	[Fact]
	public void ThrowWithMissingOrderItems()
	{
		var order = TestData.MinimalOrder with { OrderItems = default};
		Assert.Throws<ArgumentException>(order.ToDomain);
	}
}
