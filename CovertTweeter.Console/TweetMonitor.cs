using System;
using System.Collections.Generic;
using CovertTweeter.Core;
using TweetSharp;

namespace CovertTweeter
{
    public class TweetMonitor
    {            
        public void Run()
        {
            var repo = new TweetRepository();

            long? lastHomeId = null;
            long userId = repo.GetUser().Id;

            while (true)
            {                
                foreach (var tweet in repo.GetTweetsFromHomeTimeline(lastHomeId))
                {
                    ShowTweet(tweet);
                    lastHomeId = Math.Max(lastHomeId??0,tweet.Id);
                    if(lastHomeId==0)lastHomeId=null;
                }
            }
        }

        private void ShowTweet(TwitterStatus tweet)
        {            
            ColorConsole.Write(ConsoleColor.DarkYellow, tweet.Author.ScreenName);
            ColorConsole.WriteLine(ConsoleColor.DarkGray, ": {0}", tweet.Text);
        }
    }
}