using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using TweetSharp;

namespace CovertTweeter.Core
{
    public class TweetRepository
    {
        public List<TwitterStatus> GetTweets()
        {
            var ConsumerKey = ConfigurationManager.AppSettings["consumerKey"];
                var ConsumerSecret = ConfigurationManager.AppSettings["consumerSecret"];
                var AccessToken = ConfigurationManager.AppSettings["accessToken"];
                var AccessTokenSecret = ConfigurationManager.AppSettings["accessTokenSecret"];

            try
            {
                var service = new TwitterService(ConsumerKey, ConsumerSecret);
                service.AuthenticateWith(AccessToken, AccessTokenSecret);

                var tweets = service.ListTweetsOnHomeTimeline(new ListTweetsOnHomeTimelineOptions());
                return tweets.ToList();
            } catch (Exception ex) {
                throw;
            }
        }
    }
}
