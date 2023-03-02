using System.ComponentModel.DataAnnotations;

namespace Domain.Builders.Validators;

public static class AccountParametersValidator
{
	public static bool TryValidate(
		decimal? balance,
		string? firstName,
		string? lastName,
		out ValidationResult? result)
	{
		if (balance is null)
		{
			result = new ValidationResult(
				$"{nameof(Account.Balance)} is required",
				new[] { nameof(Account.Balance) });
			return false;
		}

		if (firstName is null || lastName is null)
		{
			result = new ValidationResult(
				"Both first and last name are required",
				new[] { nameof(AccountHolder.FirstName), nameof(AccountHolder.LastName) });
			return false;
		}

		result = null;
		return true;
	}
}