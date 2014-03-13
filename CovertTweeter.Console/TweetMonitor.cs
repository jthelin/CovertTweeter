using System;
using System.Threading;
using CovertTweeter.Core;
using TweetinviCore.Events.EventArguments;

namespace CovertTweeter
{
    public class TweetMonitor
    {
        private TweetRepository _repo;

        public TweetMonitor()
        {
            _repo = new TweetRepository();            
        }

        public void Run()
        {
            _repo.NewTweet += ShowTweet;
            _repo.Start();
        }

        private void ShowTweet(TweetReceivedEventArgs e)
        {
            ColorConsole.Write(ConsoleColor.DarkYellow, "@{0} \"{1}\" [",
                e.Tweet.Creator.Name,
                e.Tweet.Creator.ScreenName
                );
            ColorConsole.Write(ConsoleColor.Yellow, "{0} {1}", e.Tweet.CreatedAt.ToString());
            ColorConsole.WriteLine(ConsoleColor.DarkYellow, "]");
            ColorConsole.WriteLine(ConsoleColor.DarkGray, ": {0}", e.Tweet.Text);
        }
    }
}