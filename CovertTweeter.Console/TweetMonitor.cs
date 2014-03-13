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
                var result = repo.GetTweetsFromHomeTimeline(lastHomeId);
                if(result==null)
                    ColorConsole.WriteLine(ConsoleColor.DarkRed,"Error updating");
                else foreach (var tweet in result)
                {
                    ShowTweet(tweet);
                    lastHomeId = Math.Max(lastHomeId??0,tweet.Id);
                    if(lastHomeId==0)lastHomeId=null;
                }
                Thread.Sleep(2000);
            }
        }

        private void ShowTweet(dynamic tweet)
        {            
            ColorConsole.Write(ConsoleColor.DarkYellow, "{0} [", tweet.Author.ScreenName);
            ColorConsole.Write(ConsoleColor.Yellow, "{0} {1}", tweet.CreatedDate.ToShortDateString(), tweet.CreatedDate.ToShortTimeString());
            ColorConsole.WriteLine(ConsoleColor.DarkYellow, "]");                        
            ColorConsole.WriteLine(ConsoleColor.DarkGray, ": {0}", tweet.Text);
        }
    }
}