using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ApiDemo.Library.Graph;

public class GraphClientFactory
{
    private readonly IAccessTokenProviderAccessor accessor;
    private readonly HttpClient httpClient;
    private readonly ILogger<GraphClientFactory> logger;
    private GraphServiceClient graphClient;

    public GraphClientFactory(IAccessTokenProviderAccessor accessor,
        HttpClient httpClient,
        ILogger<GraphClientFactory> logger)
    {
        this.accessor = accessor;
        this.httpClient = httpClient;
        this.logger = logger;
    }

    public HttpClient Http
    {
        get => httpClient;
    }

    public GraphServiceClient GetAuthenticatedClient()
    {
        // Use the existing one if it's there
        if (graphClient == null)
        {
            graphClient = new(httpClient);
            graphClient.AuthenticationProvider =
                new BlazorAuthProvider(accessor);
        }

        return graphClient;
    }

    public HttpClient GetAuthenticatedHttpClient()
    {
        // Use the existing one if it's there
        if (graphClient == null)
        {
            graphClient = new(httpClient);
            graphClient.AuthenticationProvider =
                new BlazorAuthProvider(accessor);
        }

        return httpClient;
    }

    public async Task LoadHttpClient()
    {
        if (httpClient.DefaultRequestHeaders.Authorization == null)
        {
            var result = await accessor.TokenProvider.RequestAccessToken();
            result.TryGetToken(out var token);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
        }
    }
}