using ApiDemo.Library.Twitter;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.SignalR.Client;

namespace ApiDemo.Web.Client.Signal
{
    public class SignalClient
    {
        private HubConnection _hub;
        private readonly IAccessTokenProvider _access;
        private readonly string _endpoint = "https://localhost:7066";

        public SignalClient(NavigationManager nav, IAccessTokenProvider access)
        {
            _access = access;
        }
        
        public HubConnection Hub { get => _hub; }

        public event EventHandler<TweetEventArgs> TweetReceived;

        protected virtual void OnTweet(TweetEventArgs args) => TweetReceived?.Invoke(this, args);

        private void AddClientEvents()
        {
            _hub.Remove("OnTweet");
            _hub.On("OnTweet", (TweetModel tweet) =>
            {
                OnTweet(new TweetEventArgs(tweet));
            });
        }

        private async Task<string> GetToken()
        {
            var rq = await _access.RequestAccessToken();
            rq.TryGetToken(out var token);
            return token.Value;
        }

        public async Task InitializeSignalR()
        {
            _hub = new HubConnectionBuilder().WithUrl(
                _endpoint + "/tweet",
                options =>
                {
                    options.AccessTokenProvider = async () => await Task.FromResult(await GetToken());
                }
            )
            .WithAutomaticReconnect()
            .Build();

            await _hub.StartAsync();
            _hub.KeepAliveInterval = TimeSpan.FromSeconds(30);

            await InspectHubConnection();
        }

        private async Task InspectHubConnection()
        {
            if (_hub.State == HubConnectionState.Connecting)
            {
                int attempt = 0;
                while (_hub.State == HubConnectionState.Connecting && attempt < 10)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    attempt += 1;
                }
            }
            do
            {
                await Task.Delay(1000);
            } while (_hub.State is not HubConnectionState.Connected);

            if (_hub.State is HubConnectionState.Connected)
            {
                AddClientEvents();
            }
        }
    }
}
