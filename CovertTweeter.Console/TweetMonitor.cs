using System;
using System.Text;
using TweetinviCore.Events.EventArguments;

namespace CovertTweeter
{
    public class TweetMonitor
    {
        private ITweetRepository _repo;

        public TweetMonitor()
        {
            _repo = new TweetRepository();
            Console.OutputEncoding = Encoding.Unicode;
        }

        public void Run()
        {
            _repo.NewTweet += ShowTweet;
            _repo.NewFavourite += ShowFavourite;
            _repo.NewFollower += ShowFollower;
            _repo.NewMessage += ShowMessage;
            //_repo.Heartbeat += () => ColorConsole.WriteLine(ConsoleColor.DarkMagenta,"echo...");
            _repo.Start();
        }

        private void ShowMessage(object sender, MessageEventArgs e)
        {
            PrintUser(e.Message.Sender.ScreenName, e.Message.Sender.Name);
        }

        private void ShowFollower(object sender, UserFollowedEventArgs e)
        {
            PrintUser(e.User.ScreenName, e.User.Name);    
            ColorConsole.WriteLine(ConsoleColor.Yellow, "-> now follows you!");
        }

        private void ShowFavourite(object sender, TweetFavouritedEventArgs e)
        {
            PrintUser(e.FavouritingUser.ScreenName, e.FavouritingUser.Name);            
            ColorConsole.WriteLine(ConsoleColor.Yellow, " -> FAV:");
            PrintTweet(e.Tweet.Text);
        }

        private void ShowTweet(object sender, TweetReceivedEventArgs e)
        {
            ColorConsole.Write(ConsoleColor.DarkYellow, "@{0} \"{1}\" [",
                e.Tweet.Creator.ScreenName,
                e.Tweet.Creator.Name
            );
            ColorConsole.Write(ConsoleColor.Yellow, "{0}", e.Tweet.CreatedAt.ToString());
            ColorConsole.WriteLine(ConsoleColor.DarkYellow, "]");
            PrintTweet(e.Tweet.Text);
        }

        private void PrintUser(string userName, string displayName)
        {
            ColorConsole.Write(ConsoleColor.DarkYellow, "@{0} ", userName);
            if(!string.IsNullOrEmpty(displayName))
            ColorConsole.Write(ConsoleColor.DarkYellow, "\"{0}\"", displayName);
        }

        private void PrintTweet(string body)
        {
            int i = 0;
            ConsoleColor color;
            var sb = new StringBuilder();
            while (i < body.Length)
            {                
                if (body[i] == '#')
                {
                    color = ConsoleColor.DarkCyan;
                }
                else if (body[i] == '@')
                {
                    color = ConsoleColor.DarkMagenta;
                }
                else
                {
                    color = ConsoleColor.DarkGray;
                }
                sb.Append(body[i]);
            }

            ColorConsole.WriteLine(ConsoleColor.DarkGray, "{0}", 
        }
    }
}