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

            var server = new MyServer();     // example
            server.Run();

            exitEvent.WaitOne();
            server.Stop();
        }
    }
}
