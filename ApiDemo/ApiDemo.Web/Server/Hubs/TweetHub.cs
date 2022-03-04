using ApiDemo.Library.Twitter;
using ApiDemo.Web.Server.Services;
using Microsoft.AspNetCore.SignalR;

namespace ApiDemo.Web.Server.Hubs
{
    public class TweetHub : Hub
    {
        public async void TweetReceived(TweetModel e)
        {
            await Clients.All.SendAsync("OnTweet", e);
        }
    }
}
