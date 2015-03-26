using System;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Interfaces;

namespace CovertTweeter
{
    public interface ITweetRepository
    {
        event EventHandler<TweetReceivedEventArgs> NewTweet;
        event EventHandler<MessageEventArgs> NewMessage;
        event EventHandler<UserFollowedEventArgs> NewFollower;
        event EventHandler<TweetFavouritedEventArgs> NewFavourite;
        event EventHandler<PulseEventArgs> Heartbeat;
        event EventHandler<StreamExceptionEventArgs> StreamStopped;
        event EventHandler StreamStarted;

        void Start();
        void Pause();
        void Stop();
        ILoggedUser GetCurrentUser();        
    }
}