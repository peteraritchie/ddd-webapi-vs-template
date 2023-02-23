using Application.UseCases;
using Domain;
using Domain.Abstractions;
using Moq;

namespace Tests;

public class AccountServiceShould
{
	private readonly Mock<IAccountRepository> mockRepository;

	public AccountServiceShould()
	{
		mockRepository = new Mock<IAccountRepository>();
		mockRepository
			.Setup(m => m.GetAsync(It.IsAny<Guid>()))
			.Returns(Task.FromResult(new Account(100m) { AccountHolder = new AccountHolder { Email = "" } }));
	}

	[Fact]
	public void NotifyAccountHolder()
	{
		var mockNotificationService = new Mock<INotificationService>();
		var service = new AccountService(mockRepository.Object, mockNotificationService.Object);
		service.NotifyAccountHolder(Guid.NewGuid(), "message");
		mockNotificationService
			.Verify(
				m => m.SendNotification(It.Is<string>(c => c == ""), It.Is<string>(c => c == "message")),
				Times.Once());
	}

	[Fact]
	public void GetAccount()
	{
		var mockNotificationService = new Mock<INotificationService>();
		var service = new AccountService(mockRepository.Object, mockNotificationService.Object);
		var account = service.GetAccount(Guid.NewGuid());
		Assert.NotNull(account);
	}
}
