using System;
using TweetinviCore.Events.EventArguments;
using TweetinviCore.Interfaces.Credentials;

namespace CovertTweeter
{
    public interface ITweetRepository
    {
        event EventHandler<TweetReceivedEventArgs> NewTweet;
        event EventHandler<MessageEventArgs> NewMessage;
        event EventHandler<UserFollowedEventArgs> NewFollower;
        event EventHandler<TweetFavouritedEventArgs> NewFavourite;
        event EventHandler Heartbeat;

        void Start();
        void Pause();
        void Stop();
        ILoggedUser GetCurrentUser();
    }
}