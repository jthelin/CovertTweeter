using System;
using System.Threading;
using CovertTweeter.Core;

namespace CovertTweeter
{
    public class TweetMonitor
    {
        public void Run()
        {
            var repo = new TweetRepository();

            long? lastHomeId = null;
            //long userId = repo.GetUser().Id;

            while (true)
            {
                try
                {
                    var result = repo.GetTweetsFromHomeTimeline(lastHomeId);
                    foreach (var tweet in result)
                    {
                        ShowTweet(tweet);
                        lastHomeId = Math.Max(lastHomeId ?? 0, tweet.Id);
                        if (lastHomeId == 0)
                            lastHomeId = null;
                    }
                }
                catch (Exception ex)
                {
                    ColorConsole.WriteLine(ConsoleColor.DarkRed, "Error: " + ex.Message);
                }
                finally
                {
                    Thread.Sleep(1000);
                }                
            }
        }

        private void ShowTweet(TwitterStatus tweet)
        {
            ColorConsole.Write(ConsoleColor.DarkYellow, "{0} [", tweet.User.Name);
            ColorConsole.Write(ConsoleColor.Yellow, "{0} {1}", tweet.CreatedAt.ToString());
            ColorConsole.WriteLine(ConsoleColor.DarkYellow, "]");
            ColorConsole.WriteLine(ConsoleColor.DarkGray, ": {0}", tweet.Text);
        }
    }
}