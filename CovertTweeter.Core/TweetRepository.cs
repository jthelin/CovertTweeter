using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.Win32;
using Newtonsoft.Json;
using Tweetinvi;
using TweetinviCore.Interfaces.Streaminvi;

namespace CovertTweeter.Core
{
    public class TweetRepository
    {
        private const string REG_PATH = @"HKEY_CURRENT_USER\SOFTWARE\NathanChere\CovertTweeter";
        private const string KEY_API = @"ApiKey";
        private const string KEY_APIPRIVATE = @"ApiSecret";
        private const string KEY_TOKEN = @"AccessToken";
        private const string KEY_TOKENPRIVATE = @"AccessTokenSecret";

        private readonly IUserStream _userStream;

        public TweetRepository() :this(null,null,null,null) { }

        public TweetRepository(string apiKey, string apiKeySecret, string accessToken, string accessTokenSecret)
        {                        
            TwitterCredentials.SetCredentials(
                GetConfigValue(accessToken,KEY_TOKEN),
                GetConfigValue(accessTokenSecret, KEY_TOKENPRIVATE),
                GetConfigValue(apiKey, KEY_API),
                GetConfigValue(apiKeySecret, KEY_APIPRIVATE));

            _userStream = Stream.CreateUserStream();
        }

        public void Start()
        {
            _userStream.TweetCreatedByMe += (s, a) => { Console.WriteLine("I posted {0}", a.Tweet.Text); };
            _userStream.TweetCreatedByFriend += (s, a) => { Console.WriteLine("{0} posted {1}", a.Tweet.Creator.Name, a.Tweet.Text); };
            _userStream.MessageReceived += (s, a) => { Console.WriteLine("You received the message : {0}", a.Message.Text); };
            _userStream.StartStream();
        }

        /// <summary>
        /// Return the first non-null result out of the default, registry
        ///  and app.config values for the given index
        /// </summary>        
        private string GetConfigValue(string defaultValue, string index)
        {
            var result = defaultValue 
                ?? (string)Registry.GetValue(REG_PATH, KEY_API, null)
                ?? ConfigurationManager.AppSettings[KEY_API];                        
            if(result == null) throw new ConfigurationErrorsException("No defaultValue for " + index + " found in registry or app.config");
            return result;
        }

        EnvironmentVariableTarget 
        //var user = User.GetLoggedUser();
        
    }
}
