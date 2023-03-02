namespace Domain;

public record AccountHolder(string LastName, string FirstName, string Email)
{
	public string LastName { get; } = LastName ?? throw new ArgumentNullException(nameof(LastName));
	public string FirstName { get; } = FirstName ?? throw new ArgumentNullException(nameof(FirstName));
	public string Email { get; } = Email ?? throw new ArgumentNullException(nameof(Email));
}