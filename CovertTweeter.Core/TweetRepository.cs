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
using Tweetinvi;

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

        #region ctor
        public TweetRepository() :this(null,null,null,null) { }

        public TweetRepository(string apiKey, string apiKeySecret, string accessToken, string accessTokenSecret)
        {
            // JsonConvert.DefaultSettings = () => new JsonSerializerSettings(){ContractResolver = new TwitterMappingResolver()};

            TwitterCredentials.SetCredentials(_accessToken,_accessTokenSecret,_apiKey,_apiKeySecret);

            _apiKey = GetConfigValue(apiKey, KEY_API);
            _apiKeySecret = GetConfigValue(apiKeySecret, KEY_APIPRIVATE);
            _accessToken = GetConfigValue(accessToken,KEY_TOKEN);
            _accessTokenSecret = GetConfigValue(accessTokenSecret, KEY_TOKENPRIVATE);
            if (_apiKey != null && _apiKeySecret != null && _accessToken != null && _accessTokenSecret != null) return;

            throw new ConfigurationErrorsException("API keys and access tokens not found in registry or app config");
        }

        private string GetConfigValue(string value, string index)
        {
            var result = value 
                ?? (string)Registry.GetValue(REG_PATH, KEY_API, null)
                ?? ConfigurationManager.AppSettings[KEY_API];            
            if(result == null) throw new ConfigurationErrorsException("No value for " + index);            
            return result;
        }

        #endregion

        //var user = User.GetLoggedUser();

        public List<TwitterStatus> GetTweetsFromHomeTimeline(long? sinceId = null)
        {                        

            var client = new RestClient("https://api.twitter.com") {
                Authenticator = OAuth1Authenticator.ForProtectedResource(
                    _apiKey, _apiKeySecret,
                    _accessToken, _accessTokenSecret
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

            var response = client.Execute(request).Content;
            VerifyResponse(response);

            var tweets = JsonConvert.DeserializeObject<List<TwitterStatus>>(response);
            return tweets.Where(t=>t.Id > (sinceId??0)).OrderBy(t=>t.Id).ToList();            
        }

        private void VerifyResponse(string json)
        {
            TwitterResponse result;
            
            try { // TODO: This is rubbish
                result = JsonConvert.DeserializeObject<TwitterResponse>(json);
            }
            catch { return; }            
                                
            if (result.Errors != null && result.Errors.Any())
            {
                throw new Exception(string.Format("{0}: {1}", result.Errors[0].Code, result.Errors[0].Message));
            }            
        }
    }
}
