using ApiDemo.Library.Twitter;
using ApiDemo.Web.Client.Signal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace ApiDemo.Web.Client.Pages;
public partial class TweetStream
{

    [Inject]
    private SignalClient Signal { get; set; }

    [Inject]
    private AuthenticationStateProvider Auth { get; set; }

    List<TweetModel> tweets = new();

    protected override async Task OnInitializedAsync()
    {
        var user = await Auth.GetAuthenticationStateAsync();
        if (user?.User?.Identity?.Name == null)
        {
            Auth.AuthenticationStateChanged += AuthChanged;
        }
        else
        {
            await Signal.InitializeSignalR();
            Signal.TweetReceived += ThanksForTheTweet;
        }
    }

    private void ThanksForTheTweet(object? sender, TweetEventArgs e)
    {
        if (tweets.Any(t => t.Data.Id == e.Tweet.Data.Id))
            return;

        tweets.Add(e.Tweet);
        StateHasChanged();
    }

    private async void AuthChanged(Task<AuthenticationState> task)
    {
        if (Signal.Hub.State != Microsoft.AspNetCore.SignalR.Client.HubConnectionState.Connected)
        {
            var user = await task;
            if (user.User?.Identity?.Name != null)
            {
                await Signal.InitializeSignalR();
            }
        }
    }

    Color GetConnectionColor()
    {
        return Signal?.Hub?.State == Microsoft.AspNetCore.SignalR.Client.HubConnectionState.Connected ? Color.Success : Color.Warning;
    }

    string GetConnectionString()
    {
        return Signal?.Hub?.State == Microsoft.AspNetCore.SignalR.Client.HubConnectionState.Connected ? "Connected" : "Disconnected";
    }
}
