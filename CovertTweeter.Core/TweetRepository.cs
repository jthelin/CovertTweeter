using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.Win32;
using TweetSharp;

namespace CovertTweeter.Core
{
    public class TweetRepository
    {
        private readonly string _apiKey;
        private readonly string _apiKeySecret;
        private readonly string _accessToken;
        private readonly string _accessTokenSecret;

        private readonly string _regPath = @"HKEY_CURRENT_USER\SOFTWARE\NathanChere\CovertTweeter";

        public TweetRepository(string apiKey, string apiKeySecret, string accessToken, string accessTokenSecret)
        {
            _apiKey = apiKey;
            _apiKeySecret = apiKeySecret;
            _accessToken = accessToken;
            _accessTokenSecret = accessTokenSecret;
        }

        public TweetRepository()
        {
            
            (string)Registry.GetValue(_regPath, "Installed", null);    
            _apiKey = apiKey;
            _apiKeySecret = apiKeySecret;
            _accessToken = accessToken;
            _accessTokenSecret = accessTokenSecret;
        }

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
