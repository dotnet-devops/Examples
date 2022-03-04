using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MudBlazor.Services;
using Accounting.Web.Client.Models;
using MudBlazor;
using Blazored.LocalStorage;
using UtilityAccrual.ClientLibrary.DataAccess;
using UtilityAccrual.ClientLibrary.Helpers;
using BlazorDownloadFile;

namespace Accounting.Web.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

                config.SnackbarConfiguration.PreventDuplicates = true;
                config.SnackbarConfiguration.NewestOnTop = false;
                config.SnackbarConfiguration.ShowCloseIcon = true;
                config.SnackbarConfiguration.VisibleStateDuration = 10000;
                config.SnackbarConfiguration.HideTransitionDuration = 500;
                config.SnackbarConfiguration.ShowTransitionDuration = 500;
                config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
            });

            builder.Services.AddHttpClient("Accounting.Web.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Accounting.Web.ServerAPI"));
            builder.Services.AddScoped<ApiHelper>();
            builder.Services.AddScoped<TableHelper>();
            builder.Services.AddBlazorDownloadFile();
            builder.Services.AddMsalAuthentication<RemoteAuthenticationState, CustomUserModel>(options =>
            {
                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
                options.ProviderOptions.DefaultAccessTokenScopes.Add(Environment.GetEnvironmentVariable("apiAddress"));
            }).AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, CustomUserModel, CustomUserModelFactory>();

            builder.Services.AddAuthorizationCore(options =>
            {
                options.AddPolicy("UserAccess", policy =>
                    policy.RequireClaim("role", "UtilityAccrualUser"));

                options.AddPolicy("AdminAccess", policy =>
                    policy.RequireClaim("role", "UtilityAccrualAdmin"));
            });
            
            await builder.Build().RunAsync();
        }
    }
}
