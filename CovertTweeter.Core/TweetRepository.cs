using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Security.Policy;
using Microsoft.Win32;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

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
            if (_apiKey != null || _apiKeySecret != null || _accessToken != null || _accessTokenSecret != null)
                return;

            throw new ConfigurationErrorsException("API keys and access tokens not found in registry or app config");
        }

        public TweetRepository()
        {
            // try from registry
            _apiKey = (string)Registry.GetValue(REG_PATH, KEY_API, null);
            _apiKeySecret = (string)Registry.GetValue(REG_PATH, KEY_APIPRIVATE, null);
            _accessToken = (string)Registry.GetValue(REG_PATH, KEY_TOKEN, null);
            _accessTokenSecret = (string)Registry.GetValue(REG_PATH, KEY_TOKENPRIVATE, null);
            if (_apiKey != null || _apiKeySecret != null || _accessToken != null || _accessTokenSecret != null)
                return;

            // try from app.config
            _apiKey = ConfigurationManager.AppSettings[KEY_API];
            _apiKeySecret = (string)Registry.GetValue(REG_PATH, KEY_APIPRIVATE, null);
            _accessToken = (string)Registry.GetValue(REG_PATH, KEY_TOKEN, null);
            _accessTokenSecret = (string)Registry.GetValue(REG_PATH, KEY_TOKENPRIVATE, null);
            if (_apiKey != null || _apiKeySecret != null || _accessToken != null || _accessTokenSecret != null)
                return;

            throw new ConfigurationErrorsException("API keys and access tokens not found in registry or app config");
        }

        public dynamic GetTweetsFromHomeTimeline(long? sinceId = null)
        {            
            //https://api.twitter.com/1.1/statuses/home_timeline.json
            //https://dev.twitter.com/docs/api/1.1/get/statuses/home_timeline

            var client = new RestClient("https://api.twitter.com") {
                Authenticator = OAuth1Authenticator.ForProtectedResource(
                    KEY_API, KEY_APIPRIVATE,
                    KEY_TOKEN, KEY_TOKENPRIVATE
                ),
            };

            var request = new RestRequest(Method.GET) {
                Resource = "1.1/statuses/home_timeline.json",
            };

            request.AddParameter("count", long.MaxValue);
            if (sinceId.HasValue) {
                request.AddParameter("max_id", long.MaxValue);
                request.AddParameter("since_id", sinceId.Value);
            }

            var response = client.Execute(request);

            var result = JsonConvert.DeserializeObject<TwitterResponse>(response.Content);            
            VerifyResponse(result);
            return response;
            //return tweets.Where(t=>t.Id > (sinceId??0)).OrderBy(t=>t.Id).ToList();            
        }

        private void VerifyResponse(TwitterResponse result)
        {                                               
            if (result.Errors != null && result.Errors.Any())
            {
                throw new Exception(string.Format("{0}: {1}", result.Errors[0].Code, result.Errors[0].Message));
            }            
        }
    }


    public class TwitterResponse
    {
        public List<TwitterError> Errors { get; set; }
    }

    public class TwitterError
    {
        public string Message { get; set; }
        public int Code { get; set; }
    }
}
