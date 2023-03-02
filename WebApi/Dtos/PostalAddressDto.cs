using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApi.Dtos;

/// <summary>
///     PostalAddress
/// </summary>
[DisplayName("PostalAddress")]
public record PostalAddressDto
{
	/// <summary>
	///     Optional attention line text
	/// </summary>
	/// <example>ATTN: JOHN SMITH</example>
	[JsonPropertyName("attentionText")]
	public string? AttentionText { get; set; }

	/// <summary>
	///     Street number and address
	/// </summary>
	/// <example>14544 ROGUE RIVER DR</example>
	[Required]
	[StringLength(46)]
	[JsonPropertyName("streetAddress")]
	public string? StreetAddress { get; set; }

	/// <summary>
	///     Optional alternate location text
	/// </summary>
	/// <example># 128</example>
	[StringLength(46)]
	[JsonPropertyName("alternateLocationText")]
	public string? AlternateLocationText { get; set; }

	/// <summary>
	///     City Name
	/// </summary>
	/// <example>CHESTERFIELD</example>
	[Required]
	[StringLength(46)]
	[JsonPropertyName("cityName")]
	public string? CityName { get; set; }

	/// <summary>
	///     State Name
	/// </summary>
	/// <example>MO</example>
	[Required]
	[StringLength(46)]
	[JsonPropertyName("stateName")]
	public string? StateName { get; set; }

	/// <summary>
	///     US, Canadian, or Mexican postal code
	/// </summary>
	/// <example>63017</example>
	[Required]
	// ReSharper disable StringLiteralTypo
	[RegularExpression(
		"^((?<USZip>\\d{5})|(?<USZipPlusFour>\\d{5}-\\d{4})|(?<Canadian>[ABCEGHJ-NPRSTVXY]\\d[ABCEGHJ-NPRSTV-Z][ -]?\\d[ABCEGHJ-NPRSTV-Z]\\d)|(?<Mexican>\\d{5}))$")]
	// ReSharper restore StringLiteralTypo
	[JsonPropertyName("postalCode")]
	public string? PostalCodeText { get; set; }
}