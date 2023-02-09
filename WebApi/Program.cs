using System.ComponentModel;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers;
using WebApi.Infrastructure;
using Swashbuckle.AspNetCore.Filters;

namespace WebApi;

public class Program
{
	public static void Main(string[] args)
	{
		// logging: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-7.0
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
        builder.Services.AddSingleton<IFeatureFlagService, LaunchDarklyClient>();

        // get a configuration value based upon an injected object example:
        builder.Services.AddOptions<WeatherForecastController.WeatherForecastFeatureFlags>()
            .Configure<IFeatureFlagService>((options, service) =>
            {
                options.ShouldUseCelsius = service.GetFlag<bool>("ShouldUseCelsius");
            });
        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            //options.SuppressModelStateInvalidFilter = true;
        });
        builder.Services.AddControllers();

		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen(c =>
        {
            c.CustomSchemaIds(x => x.GetCustomAttributes<DisplayNameAttribute>().SingleOrDefault()?.DisplayName ?? x.Name);
            var assemblyName = typeof(Program).Assembly.GetName().Name;
            var filePath = Path.Combine(AppContext.BaseDirectory, $"{assemblyName}.xml");
            c.IncludeXmlComments(filePath);
        });
        builder.Services.AddSwaggerExamplesFromAssemblies(typeof(OrdersController).Assembly);

        var app = builder.Build();

		var logger = app.Logger;

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
		}

		app.UseSwagger();
#if true
		app.UseSwaggerUI();
#else
        app.UseStaticFiles();
		app.UseSwaggerUI(options =>
		{
			var hostingEnv = ((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IWebHostEnvironment>();
			options.SwaggerEndpoint("/swagger.json", $"{hostingEnv.ApplicationName} v1");
		});
#endif

		app.UseHttpsRedirection();

		app.UseCustomExceptionMiddleware();

		app.UseAuthorization();

		logger.LogInformation("Mapping controllers");
		app.MapControllers();

		logger.LogInformation("Starting the app");
		app.Run();
        var c = new LaunchDarklyClient();
        var x = c.GetFlag<bool>("theFlag");
    }
}

public interface IFeatureFlagService
{
    TResult GetFlag<TResult>(string flagName) where TResult: struct;
}

public class LaunchDarklyClient : IFeatureFlagService

{
    public TResult GetFlag<TResult>(string flagName) where TResult: struct
    {
        dynamic? value = default;
        if (flagName == "ShouldUseCelsius") value = true;
        return value;
    }
}
