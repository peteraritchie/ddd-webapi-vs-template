using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using WebApi.Infrastructure;

namespace Tests;

public class CustomExceptionMiddlewareShould
{
	[Fact]
	public async Task CreateProblemDetailsSuccessfully()
	{
		using var bodyStream = new MemoryStream();
		var problemDetailsFactoryMock = SetupProblemDetailsFactoryMock();
		var requestDelegateMock = SetupRequestDelegateMock(bodyStream, out var httpContext);
		var loggerMock = new Mock<ILogger<CustomExceptionMiddleware>>();
		var sut = new CustomExceptionMiddleware(requestDelegateMock.Object, loggerMock.Object, problemDetailsFactoryMock.Object);

		await sut.InvokeAsync(httpContext);

		bodyStream.Seek(0, SeekOrigin.Begin);
		var text = System.Text.Encoding.ASCII.GetString(bodyStream.ToArray());

		Assert.Equal(
			"{\"type\":\"Internal Server Error\",\"status\":500,\"detail\":\"Exception of type \\u0027System.Exception\\u0027 was thrown.\"}",
			text);
	}

	private static Mock<RequestDelegate> SetupRequestDelegateMock(MemoryStream bodyStream, out HttpContext httpContext)
	{
		var httpResponse = new Mock<HttpResponse>()
			.SetupProperty(m => m.Body, bodyStream);
		var httpContextMock = new Mock<HttpContext>();
		httpContextMock
			.SetupGet(m => m.Response)
			.Returns(httpResponse.Object);
		httpContext = httpContextMock.Object;

		var requestDelegateMock = new Mock<RequestDelegate>();
		requestDelegateMock
			.Setup(m => m(It.IsAny<HttpContext>()))
			.Throws<Exception>();
		return requestDelegateMock;
	}

	private static Mock<ProblemDetailsFactory> SetupProblemDetailsFactoryMock()
	{
		var problemDetailsFactoryMock = new Mock<ProblemDetailsFactory>();
		problemDetailsFactoryMock
			.Setup(m => m.CreateProblemDetails(It.IsAny<HttpContext>(), It.IsAny<int>(), It.IsAny<string?>(),
				It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>()))
			.Returns((HttpContext _, int status, string? type, string? title, string? detail, string? instance) =>
				new ProblemDetails
				{
					Type = type,
					Title = title,
					Status = status,
					Detail = detail,
					Instance = instance
				});
		return problemDetailsFactoryMock;
	}
}
