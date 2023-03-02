using Domain;
using Domain.Builders.Validators;

namespace Tests.Domain;

public class PostalAddressParametersValidatorShould
{
	[Fact]
	public void ValidValuesAreNotFlagged()
	{
		Assert.True(
			PostalAddressParametersValidator.TryValidate(
				"streetAddress",
				"cityName",
				"stateName",
				"H0H 0H0",
				null,
				null,
				out _));
	}

	[Fact]
	public void FlagInvalidPostalCode()
	{
		Assert.False(
			PostalAddressParametersValidator.TryValidate(
				"streetAddress",
				"cityName",
				"stateName",
				"postalCode",
				null,
				null,
				out var result));
		Assert.Contains(
			nameof(PostalAddress.PostalCodeText),
			result!.MemberNames);
	}

	[Fact]
	public void FlagInvalidCityName()
	{
		Assert.False(
			PostalAddressParametersValidator.TryValidate(
				"streetAddress",
				"123456789012345678901234567890123456789012345678",
				"stateName",
				"H0H 0H0",
				null,
				null,
				out var result));
		Assert.Contains(
			nameof(PostalAddress.CityName),
			result!.MemberNames);
	}

	[Fact]
	public void FlagInvalidStateName()
	{
		Assert.False(
			PostalAddressParametersValidator.TryValidate(
				"streetAddress",
				"cityName",
				"123456789012345678901234567890123456789012345678",
				"H0H 0H0",
				null,
				null,
				out var result));
		Assert.Contains(
			nameof(PostalAddress.StateName),
			result!.MemberNames);
	}

	[Fact]
	public void FlagInvalidAttentionText()
	{
		Assert.False(
			PostalAddressParametersValidator.TryValidate(
				"streetAddress",
				"cityName",
				"stateName",
				"H0H 0H0",
				null,
				"123456789012345678901234567890123456789012345678",
				out var result));
		Assert.Contains(
			nameof(PostalAddress.AttentionText),
			result!.MemberNames);
	}

	[Fact]
	public void FlagInvalidAlternateLocationText()
	{
		Assert.False(
			PostalAddressParametersValidator.TryValidate(
				"streetAddress",
				"cityName",
				"stateName",
				"H0H 0H0",
				"123456789012345678901234567890123456789012345678",
				null,
				out var result));
		Assert.Contains(
			nameof(PostalAddress.AlternateLocationText),
			result!.MemberNames);
	}
}