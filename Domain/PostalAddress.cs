using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Domain;

public partial record PostalAddress(string StreetAddress, string CityName, string StateName, string PostalCodeText,
	string? AlternateLocationText = default, string? AttentionText = default)
{
	public static bool TryValidate(PostalAddress address)
	{
		Validate(address);

		return true;
	}

	public static void Validate(PostalAddress address)
	{
		if (address.AttentionText is { Length: > 46 })
		{
			throw new ValidationException("Address attn text, when provided should be 46 characters or less.");
		}

		if (address.AlternateLocationText is { Length: > 46 })
		{
			throw new ValidationException(
				"Address alternative location text, when provided should be 46 characters or less.");
		}

		if (address.CityName.Length > 46)
		{
			throw new ValidationException("Address city name text should be 46 characters or less.");
		}

		if (address.StateName.Length > 46)
		{
			throw new ValidationException("Address state name text should be 46 characters or less.");
		}

		if (!PostalCodeRegex().IsMatch(address.PostalCodeText))
		{
			throw new ValidationException("Address postal code text should be valid.");
		}
	}

	[GeneratedRegex("^((?<USZip>\\d{5})|(?<USZipPlusFour>\\d{5}-\\d{4})|(?<Canadian>[ABCEGHJ-NPRSTVXY]\\d[ABCEGHJ-NPRSTV-Z][ -]?\\d[ABCEGHJ-NPRSTV-Z]\\d)|(?<Mexican>\\d{5}))$")]
	private static partial Regex PostalCodeRegex();
}
