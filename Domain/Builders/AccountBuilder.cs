using System.ComponentModel.DataAnnotations;
using Domain.Builders.Validators;
using Domain.Common;

namespace Domain.Builders;

internal class AccountBuilder : IBuilder<Account>
{
	public string? FirstName { get; private set; }
	public string? LastName { get; private set; }
	public decimal? Balance { get; private set; }

	public AccountBuilder For(string firstName, string lastName)
	{
		FirstName = firstName;
		LastName = lastName;
		return this;
	}

	public AccountBuilder WithBalance(decimal balance)
	{
		Balance = balance;
		return this;
	}

	public Account Build()
	{
		Validate();
		return new Account(Balance!.Value);
	}

	public void Validate()
	{
		if (!TryValidate(out var result))
			throw new ValidationException(result!, default, default);
	}

	public bool TryValidate(out ValidationResult? result)
	{
		return AccountParametersValidator.TryValidate(Balance, FirstName, LastName, out result);
	}
}
