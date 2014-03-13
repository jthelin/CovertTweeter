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
            Console.OutputEncoding = System.Text.Encoding.Unicode;
        }

        public void Run()
        {
            _repo.NewTweet += ShowTweet;
            _repo.NewFavourite += ShowFavourite;
            _repo.NewFollower += ShowFollower;
            //_repo.Heartbeat += () => ColorConsole.WriteLine(ConsoleColor.DarkMagenta,"echo...");
            _repo.Start();
        }

        private void ShowFollower(object sender, UserFollowedEventArgs e)
        {
            ColorConsole.Write(ConsoleColor.DarkYellow, "@{0} \"{1}\" ->",
                e.User.ScreenName,
                e.User.Name                
                );                        
            ColorConsole.WriteLine(ConsoleColor.Yellow, " now follows you!");
        }

        private void ShowFavourite(object sender, TweetFavouritedEventArgs e)
        {
            ColorConsole.Write(ConsoleColor.DarkYellow, "@{0} \"{1}\" [",
                e.FavouritingUser.ScreenName,
                e.FavouritingUser.Name                
                );            
            ColorConsole.Write(ConsoleColor.Yellow, "{0}", e.Tweet.CreatedAt.ToString());
            ColorConsole.Write(ConsoleColor.DarkYellow, "]");
            ColorConsole.WriteLine(ConsoleColor.Yellow, " -> FAV:");
            ColorConsole.WriteLine(ConsoleColor.DarkGray, ": {0}", e.Tweet.Text);
        }

        private void ShowTweet(object sender, TweetReceivedEventArgs e)
        {
            ColorConsole.Write(ConsoleColor.DarkYellow, "@{0} \"{1}\" [",
                e.Tweet.Creator.ScreenName,
                e.Tweet.Creator.Name
                );
            ColorConsole.Write(ConsoleColor.Yellow, "{0}", e.Tweet.CreatedAt.ToString());
            ColorConsole.WriteLine(ConsoleColor.DarkYellow, "]");
            ColorConsole.WriteLine(ConsoleColor.DarkGray, ": {0}", e.Tweet.Text);
        }
    }
}