using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using CovertTweeter.Core;

namespace CovertTweeter
{
    class Program
    {
        static void Main(string[] args)
        {
            var exitEvent = new ManualResetEvent(false);

            Console.CancelKeyPress += (sender, eventArgs) => {
                                          eventArgs.Cancel = true;
                                          exitEvent.Set();
                                      };

            (new TweetMonitor()).Run();

            exitEvent.WaitOne();
        }
    }

    public class TweetMonitor
    {                
        public void Run()
        {
            var lastUpdated = DateTime.Now;
            var repo = new TweetRepository();

            // Get last 10 tweets/mentions/etc

            while (true)
            {
                var timeToCheck = lastUpdated;

                // Get any new tweets since (timestamp)

                // set lastupdated to latest received if any
                
                
                // TEST
                foreach (var tweet in repo.GetTweets())
                {
                    Console.WriteLine("{0}: {1}\n",tweet.Author.ScreenName,tweet.Text);
                }

                return;
            }
        }
    }
}
