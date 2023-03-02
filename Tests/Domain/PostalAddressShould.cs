using Domain;

namespace Tests.Domain;

public class PostalAddressShould
{
	[Fact]
	public void InitializeCorrectly()
	{
		var postalAddress = new PostalAddress(
			"street",
			"city",
			"state",
			"postal code");
		Assert.Equal(
			"street",
			postalAddress.StreetAddress);
		Assert.Equal(
			"city",
			postalAddress.CityName);
		Assert.Equal(
			"state",
			postalAddress.StateName);
		Assert.Equal(
			"postal code",
			postalAddress.PostalCodeText);
	}
}