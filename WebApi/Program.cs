using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json.Serialization;
using Application;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Swashbuckle.AspNetCore.Filters;
using WebApi.Controllers;
using WebApi.Infrastructure;

namespace WebApi;

/// <summary>
/// </summary>
/// <remarks>This is not static to support WebApplicationFactory&lt;T&gt; in tests</remarks>
public class Program
{
	/// <summary>
	///     Entry-point
	/// </summary>
	/// <param name="args">command-line args</param>
	[ExcludeFromCodeCoverage(Justification = "ROI low.")]
	public static void Main(string[] args)
	{
		// logging: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-7.0
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddSingleton<IFeatureFlagService, LaunchDarklyClient>();
		builder.Services.ConfigureServices();
		builder.Services.ConfigureInfrastructureServices();
		builder.Services.ConfigureApplicationServices();

		// get a configuration value based upon an injected object example:
		builder.Services.AddOptions<OrdersController.OrdersControllerOptions>()
			.Configure<IFeatureFlagService>(
				(options, service) => options.FeatureFlag1 = service.GetFlag<bool>("FeatureFlag1"));
		builder.Services.Configure<ApiBehaviorOptions>(
			_ =>
			{
				//_.SuppressModelStateInvalidFilter = true;
			});

		builder.Services.AddControllers()
			.AddJsonOptions(
				options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen(
			c =>
			{
				c.CustomSchemaIds(
					x =>
						x.GetCustomAttributes<DisplayNameAttribute>().SingleOrDefault()?.DisplayName ?? x.Name);
				var assemblyName = typeof(Program).Assembly.GetName().Name;
				var filePath = Path.Combine(
					AppContext.BaseDirectory,
					$"{assemblyName}.xml");
				c.IncludeXmlComments(filePath);
			});
		builder.Services.AddSwaggerExamplesFromAssemblies(typeof(OrdersController).Assembly);

		var app = builder.Build();

		var logger = app.Logger;

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			// Configuration Point:
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

		app.Use(Middleware);

		logger.LogInformation("Mapping controllers");
		app.MapControllers();

		logger.LogInformation("Starting the app");
		app.Run();
	}

	[ExcludeFromCodeCoverage]
	private static async Task Middleware(HttpContext context, RequestDelegate next)
	{
		Console.WriteLine($"{context.Request.Method} {context.Request.Path}");
		foreach (var h in context.Request.Headers)
		{
			Console.WriteLine($"{h.Key}: {h.Value}");
		}

		switch (context.Request.Method)
		{
			case "GET":
			case "HEAD":
			default:
			{
				if (MediaTypeHeaderValue.TryParse(
					    context.Request.ContentType,
					    out var type))
				{
					var version = "unknown";

					var parameters = string.Join(
						", ",
						type.Parameters.Select(p => $"{p.Name}: {p.Value}").ToArray());
					if (type.Parameters.Any(e => e.Name == "v"))
					{
						version = type.Parameters.Single(e => e.Name == "v").Value.Value ?? "unknown";
					}
					else if (type.Parameters.Any(e => e.Name == "ver"))
					{
						version = type.Parameters.Single(e => e.Name == "ver").Value.Value ?? "unknown";
					}
					else if (type.Parameters.Any(e => e.Name == "version"))
					{
						version = type.Parameters.Single(e => e.Name == "version").Value.Value ?? "unknown";
					}

					Console.WriteLine(
						$"Type: {type.Type}, SubType: {type.SubTypeWithoutSuffix}, Suffix: {type.Suffix}, Charset: {type.Charset}, Version: {version}");
				}

				Console.WriteLine($"1. Endpoint: {context.GetEndpoint()?.DisplayName ?? "(null)"}");
				await next(context);
				break;
			}
		}
	}
}

#if NOT_WORKING
using System.ComponentModel;
using System.Reflection;
using System.Text.Json.Serialization;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Net.Http.Headers;
using WebApi.Controllers;
using WebApi.Infrastructure;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Builder;

namespace WebApi;

/// <summary>
/// 
/// </summary>
/// <remarks>This is not static to support WebApplicationFactory&lt;T&gt; in tests</remarks>
public class Program
{
    /// <summary>
    /// Entry-point
    /// </summary>
    /// <param name="args">command-line args</param>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "ROI low.")]
    public static void Main(string[] args)
    {
        // logging: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-7.0
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddSingleton<IFeatureFlagService, LaunchDarklyClient>();
        InfrastructureServiceCollectionExtensions.ConfigureServices(builder.Services);

        // get a configuration value based upon an injected object example:
        builder.Services.AddOptions<OrdersController.OrdersControllerOptions>()
            .Configure<IFeatureFlagService>((options, service) =>
            {
                options.FeatureFlag1 = service.GetFlag<bool>("FeatureFlag1");
            });
        builder.Services.Configure<ApiBehaviorOptions>(_ =>
        {
            //_.SuppressModelStateInvalidFilter = true;
        });

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.CustomSchemaIds(x =>
                x.GetCustomAttributes<DisplayNameAttribute>().SingleOrDefault()?.DisplayName ?? x.Name);
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
            // Configuration Point:
        }

        app.UseSwagger();
        if (true)
        {
            app.UseSwaggerUI();
        }
        else
        {
            app.UseStaticFiles();
            app.UseSwaggerUI(options =>
            {
                var hostingEnv =
                    ((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IWebHostEnvironment>();
                options.SwaggerEndpoint("/swagger.json", $"{hostingEnv.ApplicationName} v1");
            });
        }

        app.UseHttpsRedirection();

        app.UseCustomExceptionMiddleware();

        if (true)
        {
            app.UseRouting();
            //app.UseAuthorization();
#pragma warning disable ASP0014 // Suggest using top level route registrations
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDynamicControllerRoute<VersionedContentTypeTransformer>("{controller}/{action}");
            });
#pragma warning restore ASP0014 // Suggest using top level route registrations
#if false
            app.Use(async (httpContext, next) =>
            {
                Console.WriteLine($"{httpContext.Request.Method} {httpContext.Request.Path}");
                foreach (var h in httpContext.Request.Headers)
                {
                    Console.WriteLine($"{h.Key}: {h.Value}");
                }

                switch (httpContext.Request.Method)
                {
                    case "GET":
                    case "HEAD":
                    default:
                    {
                        MediaTypeHeaderValue.TryParse(httpContext.Request.ContentType, out var type);
                        var parameters = string.Join(", ",
                            type.Parameters.Select(p => $"{p.Name}: {p.Value}").ToArray());
                        string version = "unknown";
                        if (type.Parameters.Any(e => e.Name == "v"))
                            version = type.Parameters.Single(e => e.Name == "v").Value.Value;
                        else if (type.Parameters.Any(e => e.Name == "ver"))
                            version = type.Parameters.Single(e => e.Name == "ver").Value.Value;
                        else if (type.Parameters.Any(e => e.Name == "version"))
                            version = type.Parameters.Single(e => e.Name == "version").Value.Value;

                        Console.WriteLine(
                            $"Type: {type.Type}, SubType: {type.SubTypeWithoutSuffix}, Suffix: {type.Suffix}, Charset: {type.Charset}, Version: {version}");
                        Console.WriteLine($"1. Endpoint: {httpContext.GetEndpoint()?.DisplayName ?? "(null)"}");
                        await next(httpContext);
                        break;
                    }
                }
            });
#endif
        }
        else
        {
            app.UseAuthorization();

            logger.LogInformation("Mapping controllers");
            app.MapControllers();
        }

        logger.LogInformation("Starting the app");
        app.Run();
    }
}

public class VersionedContentTypeTransformer : DynamicRouteValueTransformer
{
    public override async ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
    {
        values["controller"] = "orders";
        switch (httpContext.Request.Method)
        {
            case "GET":
                values["action"] = nameof(OrdersController.GetOrder);
                break;
            case "POST":
                var version = "0";
                if (MediaTypeHeaderValue.TryParse(httpContext.Request.ContentType, out var type))
                {
                    if (type.Parameters.Any(e => e.Name == "v"))
                        version = type.Parameters.Single(e => e.Name == "v").Value.Value ?? version;
                    else if (type.Parameters.Any(e => e.Name == "ver"))
                        version = type.Parameters.Single(e => e.Name == "ver").Value.Value ?? version;
                    else if (type.Parameters.Any(e => e.Name == "version"))
                        version = type.Parameters.Single(e => e.Name == "version").Value.Value ?? version;

                    Console.WriteLine(
                        $"Type: {type.Type}, SubType: {type.SubTypeWithoutSuffix}, Suffix: {type.Suffix}, Charset: {type.Charset}, Version: {version}");
                }

                values["action"] = nameof(OrdersController.CreateOrderAsync);
                break;
            default:
                throw new NotImplementedException();
        }

        return values;
    }
}
#endif