namespace Domain;

public class Account
{
	public Account(decimal balance)
	{
		Balance = balance;
	}

	public decimal Balance { get; private set; } = 0m;
	public AccountHolder AccountHolder { get; set; }

	public void Credit(decimal amount)
	{
		Balance += amount;
	}

	public void Debit(decimal amount)
	{
		Balance -= amount;
	}
}
