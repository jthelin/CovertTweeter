using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

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
        private DateTime lastUpdated = DateTime.Now;

        public void Run()
        {
            // Get last 10 tweets/mentions/etc

            while (true)
            {
                var timeToCheck = lastUpdated;

                // Get any new tweets since (timestamp)

                // set lastupdated to latest received if any
            }
        }
    }
}
