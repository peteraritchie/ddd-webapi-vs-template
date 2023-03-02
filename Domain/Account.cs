namespace Domain;

public record Account(AccountHolder AccountHolder, decimal Balance)
{
	public decimal Balance { get; private set; } = Balance;

	public void Credit(decimal amount)
	{
		Balance += amount;
	}

	public void Debit(decimal amount)
	{
		Balance -= amount;
	}
}