using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using Tests.Common;
using WebApi;
using WebApi.Common;

namespace Tests;

public sealed class OrdersEndpointShould : WebApplicationTesterBase, IDisposable
{
	private readonly MediaTypeHeaderValue orderJsonMediaType =
		MediaTypeHeaderValue.Parse(ApplicationContentTypes.OrderJson);

	private readonly MediaTypeHeaderValue problemJsonMediaType =
		MediaTypeHeaderValue.Parse(ModernMediaTypeNames.Application.ProblemJson);

	public OrdersEndpointShould(WebApplicationFactory<Program> webApplicationFactory)
		: base(webApplicationFactory)
	{
		Console.WriteLine("tick");
	}

	[Fact]
	public async Task HaveCorrectStatusCodeWithCorrectGet()
	{
		using var response = await client.GetAsync("/Orders/76078b26-6130-4b93-9c26-3a37616e1244");
		Assert.NotNull(response);
		Assert.True(response.IsSuccessStatusCode);
		Assert.Equal(
			HttpStatusCode.OK,
			response.StatusCode);
	}

	[Fact]
	public async Task HaveCorrectSuccessContentTypeGet()
	{
		using var response = await client.GetAsync("/Orders/76078b26-6130-4b93-9c26-3a37616e1244");
		Assert.NotNull(response);
		Assert.True(response.IsSuccessStatusCode);
		Assert.Equal(
			orderJsonMediaType.MediaType,
			response.Content.Headers.ContentType?.MediaType);
	}

	[Fact]
	public async Task HaveBadRequestWithBadIdentifierGet()
	{
		using var response = await client.GetAsync("/Orders/00000000-0000-0000-0000-000000000000");
		Assert.NotNull(response);
		Assert.False(response.IsSuccessStatusCode);
		Assert.Equal(
			HttpStatusCode.BadRequest,
			response.StatusCode);
	}

	[Fact]
	public async Task HaveBadRequestWithEmptyContentPost()
	{
		using var content = new StringContent(
			string.Empty,
			orderJsonMediaType);
		using var response = await client.PostAsync(
			"/Orders",
			content);
		Assert.NotNull(response);
		Assert.False(response.IsSuccessStatusCode);
		Assert.Equal(
			HttpStatusCode.BadRequest,
			response.StatusCode);
	}

	[Fact]
	public async Task HaveUnsupportedMediaTypeTextContentTypePost()
	{
		using var content = new StringContent(string.Empty);
		using var response = await client.PostAsync(
			"/Orders",
			content);
		Assert.NotNull(response);
		Assert.False(response.IsSuccessStatusCode);
		Assert.Equal(
			HttpStatusCode.UnsupportedMediaType,
			response.StatusCode);
	}

	[Fact]
	public async Task HaveCreatedStatusOnPost()
	{
		using var content = new StringContent(
			TestData.OrderJsonText,
			orderJsonMediaType);
		using var response = await client.PostAsync(
			"/Orders",
			content);
		Assert.NotNull(response);
		Assert.True(response.IsSuccessStatusCode);
		Assert.Equal(
			HttpStatusCode.Created,
			response.StatusCode);
	}

	[Fact(Skip = "asp.net does not cooperate")]
	public async Task HaveCorrectBadRequestContentTypeGet()
	{
		using var response = await client.GetAsync("/Orders/00000000-0000-0000-0000-000000000000");
		Assert.NotNull(response);
		Assert.False(response.IsSuccessStatusCode);
		Assert.Equal(
			problemJsonMediaType.MediaType,
			response.Content.Headers.ContentType?.MediaType);
	}
}