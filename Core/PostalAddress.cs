using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Core;

public class PostalAddress
{
    public PostalAddress(string streetAddress, string cityName, string stateName, string postalCodeText)
        :this(streetAddress, cityName, stateName, postalCodeText, default, default)
    {
    }

    public PostalAddress(string streetAddress, string cityName, string stateName, string postalCodeText,
        string? alternateLocationText, string? attentionText)
    {
        AttentionText = attentionText;
        StreetAddress = streetAddress;
        AlternateLocationText = alternateLocationText;
        CityName = cityName;
        StateName = stateName;
        PostalCodeText = postalCodeText;
    }

    public string? AttentionText { get; }
    public string StreetAddress { get; }
    public string? AlternateLocationText { get; }
    public string CityName { get; }
    public string StateName { get; }
    public string PostalCodeText { get; }

    public static bool TryValidate(PostalAddress address)
    {
        try
        {
            Validate(address);
        }
        catch (ValidationException)
        {
            throw;
        }

        return true;
    }

    public static void Validate(PostalAddress address)
    {
        if (address.AttentionText != null && address.AttentionText.Length > 46)
            throw new ValidationException("Address attn text, when provided should be 46 characters or less.");
        if (address.AlternateLocationText != null && address.AlternateLocationText.Length > 46)
            throw new ValidationException("Address alternative location text, when provided should be 46 characters or less.");
        if(address.CityName.Length < 46)
            throw new ValidationException("Address city name text should be 46 characters or less.");
        if (address.StateName.Length < 46)
            throw new ValidationException("Address state name text should be 46 characters or less.");

        if(!Regex.IsMatch(address.PostalCodeText, "^((?<USZip>\\d{5})|(?<USZipPlusFour>\\d{5}-\\d{4})|(?<Canadian>[ABCEGHJ-NPRSTVXY]\\d[ABCEGHJ-NPRSTV-Z][ -]?\\d[ABCEGHJ-NPRSTV-Z]\\d)|(?<Mexican>\\d{5}))$"))
            throw new ValidationException("Address postal code text should be valid.");
    }
}
