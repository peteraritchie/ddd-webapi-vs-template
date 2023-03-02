using Domain;
using Domain.Abstractions;

namespace Application.UseCases;

internal class AccountService
{
	private readonly INotificationService notificationService;
	private readonly IAccountRepository repository;

	public AccountService(IAccountRepository repository, INotificationService notificationService)
	{
		this.repository = repository;
		this.notificationService = notificationService;
	}

	public void NotifyAccountHolder(Guid accountId, string message)
	{
		var account = GetAccount(accountId);
		notificationService.SendNotification(
			account.AccountHolder.Email,
			message);
	}

	public Account GetAccount(Guid accountId)
	{
		return repository.GetAsync(accountId).Result;
	}
}