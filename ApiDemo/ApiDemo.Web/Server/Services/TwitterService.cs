using ApiDemo.Library.Twitter;
using ApiDemo.Web.Server.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ApiDemo.Web.Server.Services
{
    public class TwitterService : BackgroundService
    {
        private bool isRunning;
        private readonly TwitterApi _twitterApi;
        private readonly IHubContext<TweetHub> _hub;
        private readonly LightService _lights;

        public TwitterService(TwitterApi api, IHubContext<TweetHub> hub, LightService lights)
        {
            _twitterApi = api;
            _twitterApi.TweetReceived += TweetReceived;
            _hub = hub;
            _lights = lights;
            isRunning = false;
        }

        private async void TweetReceived(object? sender, TweetEventArgs e)
        {
            await _hub.Clients.All.SendAsync("OnTweet", e.Tweet);
            await _lights.TweetNotification();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Run();
                await Task.Delay(1000);
            }
           
        }

        private async Task Run()
        {
            if (!isRunning)
            {
                try
                {
                    isRunning = true;
                    await _twitterApi.GetTwitterFeed();
                }
                catch
                {
                    isRunning = false;
                }
            }
        }

    }
}
