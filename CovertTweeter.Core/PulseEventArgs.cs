using System;

namespace CovertTweeter
{
    public class PulseEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public PulseEventArgs(string message) {
            Message = message;
        }
    }
}