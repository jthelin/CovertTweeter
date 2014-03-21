using System;
using System.Linq;
using System.Text;
using TweetinviCore.Events.EventArguments;
using TweetinviCore.Interfaces.DTO;

namespace CovertTweeter
{
    public class TweetMonitor
    {
        private ITweetRepository _repo;
        private readonly string _currentUser;

        public TweetMonitor()
        {
            _repo = new TweetRepository();
            _currentUser = _repo.GetCurrentUser().ScreenName;

            Console.OutputEncoding = Encoding.Unicode;
        }

        public void Run()
        {
            _repo.NewTweet += ShowTweet;
            _repo.NewFavourite += ShowFavourite;
            _repo.NewFollower += ShowFollower;
            _repo.NewMessage += ShowMessage;
            _repo.Heartbeat += (s,e) => ColorConsole.Write(ConsoleColor.DarkMagenta,Clean(e.Message));
            _repo.StreamStarted += (s,e) => ColorConsole.WriteLine(ConsoleColor.DarkMagenta,"Twitter feed GO!");
            _repo.StreamStopped += (s,e) => ColorConsole.WriteLine(ConsoleColor.DarkMagenta,"Twitter feed halted:" + Clean(e.DisconnectMessage.ToString()));
            _repo.Start();
        }

        // Prevent formatting bug
        private string Clean(string message)
        {
            return message.Replace("{","{{").Replace("}","}}");
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
            PrintUser(e.Tweet.Creator.ScreenName, e.Tweet.Creator.Name);
            ColorConsole.Write(ConsoleColor.Yellow, " -> +fav by ");
            PrintUser(e.FavouritingUser.ScreenName, e.FavouritingUser.Name);
            CR();
            PrintTweet(e.Tweet.Text);
        }        

        private void ShowTweet(object sender, TweetReceivedEventArgs e)
        {
            ColorConsole.Write(ConsoleColor.DarkYellow, "@{0} \"{1}\" [",
                Clean(e.Tweet.Creator.ScreenName),
                Clean(e.Tweet.Creator.Name)
            );
            ColorConsole.Write(ConsoleColor.Yellow, "{0}", e.Tweet.CreatedAt.ToString());
            ColorConsole.WriteLine(ConsoleColor.DarkYellow, "]");
            PrintTweet(e.Tweet.Text);
        }

        private void PrintUser(string userName, string displayName)
        {
            var color = ConsoleColor.DarkYellow;
            if(userName == _currentUser) color = ConsoleColor.Yellow;
            ColorConsole.Write(ConsoleColor.DarkYellow, "@{0} ", Clean(userName));
            if(!string.IsNullOrEmpty(displayName))
            ColorConsole.Write(ConsoleColor.DarkYellow, "\"{0}\"", Clean(displayName));
        }

        private void PrintTweet(string body)
        {
            int i = 0;
            const string invalidChars = " '\";,.;'[]()+=-/\\!@#$%^&*|~`\n\t{}"; // TODO find out the official parsing strategy
            
            while (i < body.Length)
            {                
                var sb = new StringBuilder();
                ConsoleColor color;

                // TODO: bother detecting hyperlinks?
                
                while(i < body.Length && body[i]==' ') Console.Write(body[i++]);
                
                if (body[i] == '#')
                {
                    color = ConsoleColor.DarkMagenta;
                    do
                        { sb.Append(body[i++]); }
                    while
                        (i < body.Length && !invalidChars.Contains(body[i].ToString()));
                }
                else if (body[i] == '@')
                {
                    color = ConsoleColor.DarkCyan;
                    do
                        { sb.Append(body[i++]); }
                    while
                        (i < body.Length && !invalidChars.Contains(body[i].ToString()));

                    if(sb.ToString() == _currentUser) color = ConsoleColor.Cyan;
                }
                else
                {
                    color = ConsoleColor.Gray;
                    do
                        { sb.Append(body[i++]); }
                    while                        
                        (i < body.Length && body[i]!=' ');                                        
                }                

                ColorConsole.Write(color,Clean(sb.ToString()));
            }
            CR();
        }

        private void CR()
        {
            Console.WriteLine("");
        }
    }
}