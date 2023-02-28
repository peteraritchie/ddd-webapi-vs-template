using Application.Services;
using Domain;
using Domain.Abstractions;
using Moq;

namespace Tests;

public class FundsTransferServiceShould
{
	private readonly FundsTransferService fundsTransferService;
	private readonly Mock<IAccountRepository> mockRepository;

	public FundsTransferServiceShould()
	{
		mockRepository = new Mock<IAccountRepository>();
		mockRepository
			.Setup(m => m.GetAsync(It.IsAny<Guid>()))
			.Returns(Task.FromResult(new Account(new AccountHolder(string.Empty, string.Empty, string.Empty), 100m)));
		fundsTransferService = new FundsTransferService(mockRepository.Object);
	}

	[Fact]
	public async Task BeSuccessful()
	{
		var sourceAccountId = Guid.NewGuid();
		var destinationAccountId = Guid.NewGuid();
		await fundsTransferService.TransferAsync(sourceAccountId, destinationAccountId, 103m);
		mockRepository.Verify(m => m.UpdateAsync(It.IsAny<Guid>(), It.IsAny<Account>()), Times.Exactly(2));
	}

	[Fact]
	public async Task RollsBackOnRepositoryException()
	{
		var sourceAccountId = Guid.NewGuid();
		var destinationAccountId = Guid.NewGuid();
		var mockThrowingRepository = new Mock<IAccountRepository>();
		mockThrowingRepository
			.Setup(m => m.UpdateAsync(It.Is<Guid>(c => c == destinationAccountId), It.IsAny<Account>()))
			.Throws<Exception>();
		mockThrowingRepository
			.Setup(m => m.GetAsync(It.IsAny<Guid>()))
			.Returns(Task.FromResult(new Account(new AccountHolder(string.Empty, string.Empty, string.Empty), 100m)));
		var throwingFundsTransferService = new FundsTransferService(mockThrowingRepository.Object);

		await Assert.ThrowsAsync<Exception>(async ()=>await throwingFundsTransferService.TransferAsync(sourceAccountId, destinationAccountId, 103m));
		mockThrowingRepository.Verify(m => m.UpdateAsync(It.IsAny<Guid>(), It.IsAny<Account>()), Times.Exactly(3));
	}
}
