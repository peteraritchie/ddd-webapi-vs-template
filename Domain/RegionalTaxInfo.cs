namespace Domain;

public class RegionalTaxInfo
{
	private readonly float taxRate;

	public RegionalTaxInfo(float taxRate)
	{
		this.taxRate = taxRate;
	}

	public float GetTaxRate(string _)
	{
		return taxRate;
	}
}
