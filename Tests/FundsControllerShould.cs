using Application;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi;
using WebApi.Controllers;
using WebApi.Dtos;

namespace Tests;

public class FundsControllerShould
{
	private readonly FundsController fundsController;

	public FundsControllerShould()
	{
		fundsController = CreateFundsController();
	}

	private static FundsController CreateFundsController()
	{
		var stubConfigurationManager = new ConfigurationManager();
		var httpContext = new DefaultHttpContext();
		httpContext.Request.Headers["Correlation-ID"] = Guid.NewGuid().ToString();
		var services = new ServiceCollection()
			.ConfigureServices()
			.ConfigureInfrastructureServices(stubConfigurationManager)
			.ConfigureApplicationServices(stubConfigurationManager)
			.AddSingleton(httpContext)
			.AddSingleton<FundsController>();

		using var serviceProvider = services.BuildServiceProvider();
		using var scope = serviceProvider.CreateScope();

		return scope.ServiceProvider.GetRequiredService<FundsController>();
	}

	[Fact]
	public void Smoke()
	{
		Assert.NotNull(fundsController);
	}

	[Fact]
	public void ReturnNoContentOnCreate()
	{
		var fundsTransferRequestDto = new FundsTransferRequestDto
		{
			Amount = 100m,
			DestinationAccountId = Guid.NewGuid(),
			SourceAccountId = Guid.NewGuid()
		};

		var result = fundsController.CreateFundTransferRequest(fundsTransferRequestDto);
		_ = Assert.IsType<NoContentResult>(result);
	}
}
