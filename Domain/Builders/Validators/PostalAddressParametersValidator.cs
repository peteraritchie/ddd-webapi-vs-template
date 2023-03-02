using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Domain.Builders.Validators;

public static partial class PostalAddressParametersValidator
{
	public static bool TryValidate(string streetAddress, string cityName, string stateName, string postalCodeText,
		string? alternateLocationText, string? attentionText, out ValidationResult? result)
	{
		if (attentionText is { Length: > 46 })
		{
			result = new ValidationResult(
				$"{nameof(PostalAddress.AttentionText)}, when provided, must be 46 characters or less.",
				new[] { nameof(PostalAddress.AttentionText) });
			return false;
		}

		if (alternateLocationText is { Length: > 46 })
		{
			result = new ValidationResult(
				$"{nameof(PostalAddress.AlternateLocationText)} must be less than 46 characters.",
				new[] { nameof(PostalAddress.AlternateLocationText) });
			return false;
		}

		if (cityName.Length > 46)
		{
			result = new ValidationResult(
				$"{nameof(PostalAddress.CityName)} must be less than 46 characters.",
				new[] { nameof(PostalAddress.CityName) });
			return false;
		}

		if (stateName.Length > 46)
		{
			result = new ValidationResult(
				$"{nameof(PostalAddress.StateName)} must be less than 46 characters.",
				new[] { nameof(PostalAddress.StateName) });
			return false;
		}

		if (!PostalCodeRegex().IsMatch(postalCodeText))
		{
			result = new ValidationResult(
				$"{nameof(PostalAddress.PostalCodeText)} must be a valid postal code.",
				new[] { nameof(PostalAddress.PostalCodeText) });
			return false;
		}

		result = default;
		return true;
	}

	[GeneratedRegex(
		"^((?<USZip>\\d{5})|(?<USZipPlusFour>\\d{5}-\\d{4})|(?<Canadian>[ABCEGHJ-NPRSTVXY]\\d[ABCEGHJ-NPRSTV-Z][ -]?\\d[ABCEGHJ-NPRSTV-Z]\\d)|(?<Mexican>\\d{5}))$")]
	private static partial Regex PostalCodeRegex();
}