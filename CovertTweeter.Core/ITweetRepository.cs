namespace CovertTweeter.Core
{
    public interface ITweetRepository
    {
        event TweetRepository.NewTweetEvent NewTweet;
        event TweetRepository.NewMessageEvent NewMessage;
        event TweetRepository.NewFollowerEvent NewFollower;
        event TweetRepository.NewFavouriteEvent NewFavourite;
        event TweetRepository.HeartbeatEvent Heartbeat;
        void Start();
        void Pause();
        void Stop();
    }
}