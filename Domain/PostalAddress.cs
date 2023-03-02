namespace Domain;

public record PostalAddress(string StreetAddress, string CityName, string StateName, string PostalCodeText,
	string? AlternateLocationText = default, string? AttentionText = default)
{
	public string StreetAddress { get; } = StreetAddress;
	public string CityName { get; } = CityName;
	public string StateName { get; } = StateName;
	public string PostalCodeText { get; } = PostalCodeText;
	public string? AlternateLocationText { get; } = AlternateLocationText;
	public string? AttentionText { get; } = AttentionText;
}