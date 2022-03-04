using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace ApiDemo.Library.Graph;

public class GraphApiAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public GraphApiAuthorizationMessageHandler(IAccessTokenProvider provider,
        NavigationManager navigationManager)
        : base(provider, navigationManager)
    {
        ConfigureHandler(
            authorizedUrls: new[] { "https://graph.microsoft.com", "https://localhost:7066" },
            scopes: new[] { "https://graph.microsoft.com/User.Read", "api://c3af81a8-80ef-4d69-8988-f5cbfc99d227/Api.Access" });
    }
}