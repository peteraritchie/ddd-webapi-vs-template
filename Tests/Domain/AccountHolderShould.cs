using Domain;

namespace Tests.Domain;

public class AccountHolderShould
{
	[Fact]
	public void InitializeCorrectly()
	{
		var accountHolder = new AccountHolder("last", "first", "email");
		Assert.Equal("first", accountHolder.FirstName);
		Assert.Equal("last", accountHolder.LastName);
	}

	[Fact]
	public void ThrowWithMissingLastName()
	{
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
		var ex = Assert.Throws<ArgumentNullException>(() => new AccountHolder(default, "first", "email"));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
		Assert.Equal(nameof(AccountHolder.LastName), ex.ParamName);
	}

	[Fact]
	public void ThrowWithMissingFirstName()
	{
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
		var ex = Assert.Throws<ArgumentNullException>(() => new AccountHolder("last", default, "email"));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
		Assert.Equal(nameof(AccountHolder.FirstName), ex.ParamName);
	}

	[Fact]
	public void ThrowWithMissingEmail()
	{
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
		var ex = Assert.Throws<ArgumentNullException>(() => new AccountHolder("last", "email", default));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
		Assert.Equal(nameof(AccountHolder.Email), ex.ParamName);
	}
}
