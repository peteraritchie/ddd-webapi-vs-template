using Domain;

namespace Tests.Domain
{
	public class OrderLineItemShould
	{
		[Fact]
		public void CalculateTotalCorrectly()
		{
			Assert.Equal(
				10m,
				new OrderLineItem(
					string.Empty,
					2,
					5m).TotalPrice);
		}
	}
}
