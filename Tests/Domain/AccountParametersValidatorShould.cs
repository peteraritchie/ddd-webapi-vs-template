using Domain;
using Domain.Builders.Validators;

namespace Tests.Domain;

public class AccountParametersValidatorShould
{
	[Fact]
	public void ValidValuesAreNotFlagged()
	{
		Assert.True(
			AccountParametersValidator.TryValidate(
				33m,
				"First",
				"Last",
				out _));
	}

	[Fact]
	public void FlagMissingBalance()
	{
		Assert.False(
			AccountParametersValidator.TryValidate(
				null,
				"First",
				"Last",
				out var results));
		Assert.Contains(
			nameof(Account.Balance),
			results!.MemberNames);
	}

	[Fact]
	public void FlagMissingFirstName()
	{
		Assert.False(
			AccountParametersValidator.TryValidate(
				1m,
				null,
				"Last",
				out var results));
		Assert.Contains(
			nameof(AccountHolder.FirstName),
			results!.MemberNames);
	}

	[Fact]
	public void FlagMissingLastName()
	{
		Assert.False(
			AccountParametersValidator.TryValidate(
				1m,
				null,
				"Last",
				out var results));
		Assert.Contains(
			nameof(AccountHolder.LastName),
			results!.MemberNames);
	}
}