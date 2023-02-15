using Microsoft.AspNetCore.Mvc.Testing;
using WebApi;

namespace Tests.Common;

public abstract class WebApplicationTesterBase : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    protected readonly HttpClient client;

    protected WebApplicationTesterBase(WebApplicationFactory<Program> webApplicationFactory)
    {
        client = webApplicationFactory.CreateClient();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            client.Dispose();
        }
    }

#pragma warning disable CA1816 // There are no reason to create finalizers
    public void Dispose()
#pragma warning restore CA1816
    {
        Dispose(true);
    }
}
