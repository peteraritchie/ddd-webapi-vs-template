using WebApi.Dtos.Translators;

namespace Tests;

public class TranslatingDomainObjectsShould
{
	[Fact]
	public void Succeed()
	{
		var order = TestData.Order.ToDomain();
		var orderDto = order.FromDomain();
		Assert.Equal(TestData.Order.OrderDate, orderDto.OrderDate);
	}
	[Fact]
	public void SucceedWithMinimalOrder()
	{
		var order = TestData.MinimalOrder.ToDomain();
		Assert.NotNull(order.FromDomain());
	}
}
