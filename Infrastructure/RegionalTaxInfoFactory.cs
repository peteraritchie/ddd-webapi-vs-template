using Domain;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Infrastructure;

[UsedImplicitly]
public class RegionalTaxInfoFactory
{
	public class RegionalTaxInfoOptions
	{
		public float TaxRate { get; set; }
	}

	private readonly RegionalTaxInfoOptions options;

	public RegionalTaxInfoFactory(IOptions<RegionalTaxInfoOptions> options)
	{
		this.options = options.Value;
	}

	internal RegionalTaxInfo Create()
	{
		return new RegionalTaxInfo(options.TaxRate);
	}
}
