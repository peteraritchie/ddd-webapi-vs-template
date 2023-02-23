namespace Infrastructure.Dtos;

public record PostalAddressDto
{
	public string? StreetAddress { get; set; }
	public string? CityName { get; set; }
	public string? StateName { get; set; }
	public string? PostalCodeText { get; set; }
	public string? AttentionText { get; set; }
	public string? AlternateLocationText { get; set; }
}
