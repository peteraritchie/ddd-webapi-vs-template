namespace WebApi.Infrastructure;

public static class InfrastructureApplicationBuilderExtensions
{
	public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder app)
	{
		return app.UseMiddleware<CustomExceptionMiddleware>();
	}
}