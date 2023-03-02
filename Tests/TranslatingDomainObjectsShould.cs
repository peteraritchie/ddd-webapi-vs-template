using WebApi.Dtos.Translators;

namespace Tests;

public class TranslatingDomainObjectsShould
{
	[Fact]
	public void Succeed()
	{
		var dto = TestData.CreateOrder();
		var order = dto.ToDomain();
		var orderDto = order.FromDomain();
		Assert.Equal(
			dto.OrderDate,
			orderDto.OrderDate);
	}

	[Fact]
	public void SucceedWithMinimalOrder()
	{
		var order = TestData.CreateMinimalOrder().ToDomain();
		Assert.NotNull(order.FromDomain());
	}
}