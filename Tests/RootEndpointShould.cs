using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Tests.Common;
using WebApi;

namespace Tests;

public sealed class RootEndpointShould : WebApplicationTesterBase, IDisposable
{
	public class C
	{
		public string text;
		public Num num;

		public C(Num num, string text)
		{
			this.num = num;
			this.text = text;
		}
	}

	public enum Num
	{
		[Display(Name="1st")]
		First,
		[Display(Name="1st")]
		Second
	}

	[Fact]
	public void Fuck()
	{
		var sut = new C(Num.First, "text");
		var settings = new JsonSerializerSettings();
		settings.Converters.Add(new StringEnumConverter());

		var text = JsonConvert.SerializeObject(sut, settings);

		var options = new System.Text.Json.JsonSerializerOptions();
		options.Converters.Add(new JsonStringEnumConverter());
		text = System.Text.Json.JsonSerializer.Serialize(sut, options);
	}

	public RootEndpointShould(WebApplicationFactory<Program> webApplicationFactory)
		: base(webApplicationFactory)
	{
	}

	[Fact]
	public async Task HaveNotFoundGet()
	{
		using var response = await client.GetAsync("/");
		Assert.NotNull(response);
		Assert.False(response.IsSuccessStatusCode);
		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}

	[Fact]
	public async Task HaveNotFoundPost()
	{
		using var content = new StringContent(string.Empty);
		using var response = await client.PostAsync("/", content);
		Assert.NotNull(response);
		Assert.False(response.IsSuccessStatusCode);
		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}

	[Fact]
	public async Task HaveNotFoundPut()
	{
		using var content = new StringContent(string.Empty);
		using var response = await client.PutAsync("/", content);
		Assert.NotNull(response);
		Assert.False(response.IsSuccessStatusCode);
		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}

	[Fact]
	public async Task HaveNotFoundPatch()
	{
		using var content = new StringContent(string.Empty);
		using var response = await client.PatchAsync("/", content);
		Assert.NotNull(response);
		Assert.False(response.IsSuccessStatusCode);
		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}

	[Fact]
	public async Task HaveNotFoundDelete()
	{
		using var response = await client.DeleteAsync("/");
		Assert.NotNull(response);
		Assert.False(response.IsSuccessStatusCode);
		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}
}
