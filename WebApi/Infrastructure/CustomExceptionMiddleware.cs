using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace WebApi.Infrastructure;

public class CustomExceptionMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<CustomExceptionMiddleware> logger;
    private readonly ProblemDetailsFactory problemDetailsFactory;

    public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger,
        ProblemDetailsFactory problemDetailsFactory)
    {
        this.next = next;
        this.logger = logger;
        this.problemDetailsFactory = problemDetailsFactory;
    }

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

        switch (exception)
        {
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
        context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        context.Response.ContentType = problemDetails.GetContentType();
        await JsonSerializer.SerializeAsync(
            context.Response.Body,
            problemDetails
        );
    }

    private ProblemDetails CreateProblemDetails(HttpContext context, HttpStatusCode statusCode, string detail)
    {
        using var m = new HttpResponseMessage(statusCode);
        var problemDetails = problemDetailsFactory.CreateProblemDetails(context,
            title: m.ReasonPhrase,
            detail: detail,
            statusCode: context.Response.StatusCode);
        return problemDetails;
    }
}

