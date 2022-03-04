using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDemo.Library.Twitter
{
    public class TweetEventArgs : EventArgs
    {
        public TweetEventArgs() { }

        public TweetEventArgs(TweetModel tweet)
        {
            Tweet = tweet;
        }
        public TweetModel Tweet { get; set; }
    }
}
