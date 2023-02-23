using System.Net;
using System.Text.Json;
using Application.Exceptions;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApi.Common;

namespace WebApi.Infrastructure;

/// <summary>
///     ASP.NET Core middleware implementation to deal with exceptions.
/// </summary>
public class CustomExceptionMiddleware
{
	private readonly ILogger<CustomExceptionMiddleware> logger;
	private readonly RequestDelegate next;
	private readonly ProblemDetailsFactory problemDetailsFactory;

	/// <summary>
	///     CustomExceptionMiddleware constructor
	/// </summary>
	/// <param name="next">The next delegate, provided by the framework</param>
	/// <param name="logger">For logging</param>
	/// <param name="problemDetailsFactory">To create <seealso cref="ProblemDetails" /> instances</param>
	public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger,
		ProblemDetailsFactory problemDetailsFactory)
	{
		this.next = next;
		this.logger = logger;
		this.problemDetailsFactory = problemDetailsFactory;
	}

	/// <summary>
	///     To support invocation by the framework
	/// </summary>
	/// <param name="httpContext">The HTTP request context</param>
	public async Task InvokeAsync(HttpContext httpContext)
	{
		try
		{
			await next(httpContext);
		}
		catch (Exception exception)
		{
			var message = exception.Message;

			logger.LogError("Error: {message}", message);
			await HandleExceptionAsync(httpContext, exception);
		}
	}

	private async Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		HttpStatusCode statusCode;
		string detail;
		ModelStateDictionary modelState = new();
		switch (exception)
		{
			case EntityNotFoundException ex:
				statusCode = HttpStatusCode.NotFound;
				detail = $"Entity with ID {ex.Id} does not exist.";
				break;
			case ValidationsException ex:
				foreach (var e in ex.ToDictionary())
				{
					modelState.AddModelError(e.Key, e.Value);
				}
				await WriteProblemDetails(
					context,
					problemDetailsFactory.CreateValidationProblemDetails(context, modelState));
				return;
			default:
				statusCode = HttpStatusCode.InternalServerError;
				detail = exception.Message;
				break;
		}


		var problemDetails = CreateProblemDetails(context, statusCode, detail);

		await WriteProblemDetails(context, problemDetails);
	}

	private static async Task WriteProblemDetails(HttpContext context, ProblemDetails problemDetails)
	{
		context.Response.StatusCode = problemDetails.Status!.Value;
		context.Response.ContentType = problemDetails.GetContentType();
		if(problemDetails is ValidationProblemDetails validationProblemDetails)
		{
			await JsonSerializer.SerializeAsync(
				context.Response.Body,
				validationProblemDetails
			);
		}
		else
		{
			await JsonSerializer.SerializeAsync(
				context.Response.Body,
				problemDetails
			);
		}
	}

	private ProblemDetails CreateProblemDetails(HttpContext context, HttpStatusCode statusCode, string detail)
	{
		using var m = new HttpResponseMessage(statusCode);
		var problemDetails = problemDetailsFactory.CreateProblemDetails(context,
			title: m.ReasonPhrase,
			detail: detail,
			statusCode: (int)statusCode);
		return problemDetails;
	}
}
