using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
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

        private const string REG_PATH = @"HKEY_CURRENT_USER\SOFTWARE\NathanChere\CovertTweeter";
        private const string KEY_API = @"ApiKey";
        private const string KEY_APIPRIVATE = @"ApiSecret";
        private const string KEY_TOKEN = @"AccessToken";
        private const string KEY_TOKENPRIVATE = @"AccessTokenSecret";

        public TweetRepository(string apiKey, string apiKeySecret, string accessToken, string accessTokenSecret)
        {
            _apiKey = apiKey;
            _apiKeySecret = apiKeySecret;
            _accessToken = accessToken;
            _accessTokenSecret = accessTokenSecret;
            if (_apiKey != null || _apiKeySecret != null || _accessToken != null || _accessTokenSecret != null) return;

            throw new ConfigurationErrorsException("API keys and access tokens not found in registry or app config");
        }

        public TweetRepository()
        {
            // try from registry
            _apiKey = (string)Registry.GetValue(REG_PATH, KEY_API, null);
            _apiKeySecret = (string)Registry.GetValue(REG_PATH, KEY_APIPRIVATE, null);
            _accessToken = (string)Registry.GetValue(REG_PATH, KEY_TOKEN, null);
            _accessTokenSecret = (string)Registry.GetValue(REG_PATH, KEY_TOKENPRIVATE, null);
            if (_apiKey != null || _apiKeySecret != null || _accessToken != null || _accessTokenSecret != null) return;

            // try from app.config
            _apiKey = ConfigurationManager.AppSettings[KEY_API];
            _apiKeySecret = (string)Registry.GetValue(REG_PATH, KEY_APIPRIVATE, null);
            _accessToken = (string)Registry.GetValue(REG_PATH, KEY_TOKEN, null);
            _accessTokenSecret = (string)Registry.GetValue(REG_PATH, KEY_TOKENPRIVATE, null);
            if (_apiKey != null || _apiKeySecret != null || _accessToken != null || _accessTokenSecret != null) return;

            throw new ConfigurationErrorsException("API keys and access tokens not found in registry or app config");
        }

        public List<TwitterStatus> GetTweets()
        {
            try {
                var service = new TwitterService(_apiKey, _apiKeySecret);
                service.AuthenticateWith(_accessToken, _accessTokenSecret);

                var tweets = service.ListTweetsOnHomeTimeline(new ListTweetsOnHomeTimelineOptions());
                return tweets.ToList();
            } catch (Exception ex) {
                throw;
            }
        }

        public List<TwitterStatus> GetTweetsFromHomeTimeline(long? sinceId = null)
        {
            try {
                var service = new TwitterService(_apiKey, _apiKeySecret);
                service.AuthenticateWith(_accessToken, _accessTokenSecret);

                var options = new ListTweetsOnHomeTimelineOptions{
                    Count = 20,                    
                    ExcludeReplies = false,
                    IncludeEntities = true,
                    TrimUser = false,
                    ContributorDetails = true,
                };

                if (sinceId != null && sinceId.Value > 0)
                {
                    options.SinceId = sinceId;
                    options.MaxId = long.MaxValue;
                }

                var tweets = service.ListTweetsOnHomeTimeline(new ListTweetsOnHomeTimelineOptions());
                return tweets.ToList();
            } catch (Exception ex) {
                return null;
            }
        }

        public List<TwitterStatus> GetTweetsFromUserTimeline(long? sinceId = null)
        {
            try {
                var service = new TwitterService(_apiKey, _apiKeySecret);
                service.AuthenticateWith(_accessToken, _accessTokenSecret);

                var options = new ListTweetsOnUserTimelineOptions{
                    //Count = 20,                    
                };                

                if (sinceId != null && sinceId.Value > 0)
                {
                    options.SinceId = sinceId;
                    options.MaxId = long.MaxValue;
                }
                

                var tweets = service.ListTweetsOnUserTimeline(options);
                return tweets.Where(t=>t.Id > (sinceId??0)).OrderBy(t=>t.Id).ToList();
            } catch (Exception ex) {
                return null;
            }
        }

        public TwitterUser GetUser()
        {
            try
            {
                var service = new TwitterService(_apiKey, _apiKeySecret);
                service.AuthenticateWith(_accessToken, _accessTokenSecret);
                return service.GetUserProfile(new GetUserProfileOptions());
            } 
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
