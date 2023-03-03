namespace Domain.Services;

public class TaxCalculator
{
	private readonly RegionalTaxInfo taxInfo;

	public TaxCalculator(RegionalTaxInfo taxInfo)
	{
		this.taxInfo = taxInfo;
	}

	internal decimal CalculateTax(Order order)
	{
		return order.OrderItems.Sum(e => (decimal)taxInfo.GetTaxRate(e.SkuText) * e.UnitPrice * e.UnitQuantity);
	}
}
