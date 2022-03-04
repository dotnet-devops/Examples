using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Client;
using Tweetinvi.Parameters.V2;

namespace ApiDemo.Library.Twitter
{
    public class TwitterApi
    {
        public async Task GetTwitterFeed()
        {
            #region Access
            string _ConsumerKey = Environment.GetEnvironmentVariable("ConsumerKey");
            string _ConsumerSecret = Environment.GetEnvironmentVariable("ConsumerSecret");
            string _AccessToken = Environment.GetEnvironmentVariable("AccessToken");
            string _AccessTokenSecret = Environment.GetEnvironmentVariable("AccessTokenSecret");

            var client = new TwitterClient(_ConsumerKey, _ConsumerSecret, Environment.GetEnvironmentVariable("BearerToken"));
            #endregion

            var rules = await client.StreamsV2.GetRulesForFilteredStreamV2Async();
            if (rules.Rules.Length > 0)
            {
                await client.StreamsV2.DeleteRulesFromFilteredStreamAsync(rules.Rules);
            }
            await Follow(client, "DotNetDev4Hire");
            var stream = client.StreamsV2.CreateFilteredStream();
            stream.TweetReceived += Tweeted;

            await stream.StartAsync();
        }

        private void Tweeted(object? sender, Tweetinvi.Events.V2.FilteredStreamTweetV2EventArgs e)
        {
            TweetModel tweet = JsonSerializer.Deserialize<TweetModel>(e.Json);
            if (tweet != null)
            {
                if (tweet.Data != null)
                {
                    TweetReceived?.Invoke(this, new(tweet));
                }
            }
        }

        private async Task Follow(TwitterClient client, string username)
        {
            string id = (await client.UsersV2.GetUserByNameAsync(username)).User.Id;
            await client.StreamsV2.AddRulesToFilteredStreamAsync(new FilteredStreamRuleConfig("@" + username));
            await client.StreamsV2.AddRulesToFilteredStreamAsync(new FilteredStreamRuleConfig("from:" + username));
        }

        public event EventHandler<TweetEventArgs> TweetReceived;
    }
}
