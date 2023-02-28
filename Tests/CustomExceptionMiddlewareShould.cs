using System.ComponentModel.DataAnnotations;
using Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Moq;
using WebApi.Infrastructure;

namespace Tests;

public class CustomExceptionMiddlewareShould
{
	[Fact]
	public async Task DoTheRightThingWithValidationsException()
	{
		using var bodyStream = new MemoryStream();
		var problemDetailsFactoryMock = SetupProblemDetailsFactoryMock();
		var exception = new ValidationsException(new List<ValidationResult>
		{
			new ValidationResult("out of range", new[] { "field" })
		});
		var requestDelegateMock = SetupRequestDelegateMock(bodyStream, exception, out var httpContext);
		var loggerMock = new Mock<ILogger<CustomExceptionMiddleware>>();
		var sut = new CustomExceptionMiddleware(requestDelegateMock.Object, loggerMock.Object, problemDetailsFactoryMock.Object);

		await sut.InvokeAsync(httpContext);

		bodyStream.Seek(0, SeekOrigin.Begin);
		var text = System.Text.Encoding.ASCII.GetString(bodyStream.ToArray());
		Assert.Equal(
			"{\"title\":\"One or more validation errors occurred.\",\"status\":400,\"errors\":{\"field\":[\"out of range\"]}}",
			text);
		Assert.Equal(StatusCodes.Status400BadRequest, httpContext.Response.StatusCode);
	}

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

	private static Mock<RequestDelegate> SetupRequestDelegateMock(Stream bodyStream, out HttpContext httpContext)
	{
		var httpResponse = new Mock<HttpResponse>()
			.SetupProperty(m => m.Body, bodyStream);
		httpResponse.SetupProperty(m => m.StatusCode);

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

	private static Mock<RequestDelegate> SetupRequestDelegateMock<TException>(
		Stream bodyStream,
		TException exception,
		out HttpContext httpContext) where TException : Exception
	{
		var httpResponse = new Mock<HttpResponse>()
			.SetupProperty(m => m.Body, bodyStream);
		httpResponse.SetupProperty(m => m.StatusCode);

		var httpContextMock = new Mock<HttpContext>();
		httpContextMock
			.SetupGet(m => m.Response)
			.Returns(httpResponse.Object);
		httpContext = httpContextMock.Object;

		var requestDelegateMock = new Mock<RequestDelegate>();
		requestDelegateMock
			.Setup(m => m(It.IsAny<HttpContext>()))
			.Throws<TException>(()=>exception);
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
		problemDetailsFactoryMock
			.Setup(m => m.CreateValidationProblemDetails(
				It.IsAny<HttpContext>(),
				It.IsAny<ModelStateDictionary>(),
				It.IsAny<int?>(),
				It.IsAny<string>(),
				It.IsAny<string>(),
				It.IsAny<string>(),
				It.IsAny<string>()))
			.Returns((HttpContext _, ModelStateDictionary modelStateDictionary, int? statusCode,
				string? title, string? type, string? detail, string? instance) =>
			{
				statusCode ??= 400;

				var problemDetails = new ValidationProblemDetails(modelStateDictionary)
				{
					Status = statusCode,
					Type = type,
					Detail = detail,
					Instance = instance,
				};

				if (title != null)
				{
					// For validation problem details, don't overwrite the default title with null.
					problemDetails.Title = title;
				}

				return problemDetails;
			});
		return problemDetailsFactoryMock;
	}

}
