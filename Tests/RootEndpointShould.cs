using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using WebApi;

namespace Tests
{
    public class RootEndpointShould
        : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly HttpClient client;

        public RootEndpointShould(WebApplicationFactory<Program> webApplicationFactory)
        {
            client = webApplicationFactory.CreateClient();
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

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
