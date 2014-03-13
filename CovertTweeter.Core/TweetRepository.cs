using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Timers;
using Microsoft.Win32;
using Newtonsoft.Json;
using Tweetinvi;
using TweetinviCore.Events.EventArguments;
using TweetinviCore.Interfaces.Streaminvi;

namespace CovertTweeter.Core
{
    public class TweetRepository
    {        
        const string REG_PATH = @"HKEY_CURRENT_USER\SOFTWARE\NathanChere\CovertTweeter";

        private readonly IUserStream _userStream;
        private Timer _pulse;
        
        #region .ctor                
        public TweetRepository() :this(null,null,null,null) { }

        public TweetRepository(string apiKey, string apiKeySecret, string accessToken, string accessTokenSecret)
        {      
            const string KEY_API = @"ApiKey";
            const string KEY_APIPRIVATE = @"ApiSecret";
            const string KEY_TOKEN = @"AccessToken";
            const string KEY_TOKENPRIVATE = @"AccessTokenSecret";
  
            TwitterCredentials.SetCredentials(
                GetConfigValue(accessToken,KEY_TOKEN),
                GetConfigValue(accessTokenSecret, KEY_TOKENPRIVATE),
                GetConfigValue(apiKey, KEY_API),
                GetConfigValue(apiKeySecret, KEY_APIPRIVATE));

            _userStream = Stream.CreateUserStream();
            CreateEventBindings();            
        }

        private string GetConfigValue(string defaultValue, string index)
        {
            var result = defaultValue 
                ?? (string)Registry.GetValue(REG_PATH, index, null)
                ?? ConfigurationManager.AppSettings[index];                        
            if(result == null) throw new ConfigurationErrorsException("No defaultValue for " + index + " found in registry or app.config");
            return result;
        }
        #endregion

        #region Events
        public delegate void NewTweetEvent(TweetReceivedEventArgs e);
        public delegate void NewMessageEvent(MessageEventArgs e);
        public delegate void NewFollowerEvent(UserFollowedEventArgs e);
        public delegate void NewFavouriteEvent(TweetFavouritedEventArgs e);
        public delegate void HeartbeatEvent();
       
        public event NewTweetEvent NewTweet;
        public event NewMessageEvent NewMessage;
        public event NewFollowerEvent NewFollower;
        public event NewFavouriteEvent NewFavourite;
        public event HeartbeatEvent Heartbeat;

        private void CreateEventBindings()
        {            
            _userStream.TweetCreatedByMe += (sender, args) => { if (NewTweet != null) NewTweet(args); };
            _userStream.TweetCreatedByFriend += (sender, args) => { if (NewTweet != null) NewTweet(args); };

            _userStream.MessageReceived += (sender, args) => { if (NewMessage!= null) NewMessage(args); };

            _userStream.TweetFavouritedByAnyoneButMe += (sender, args) =>  { if (NewFavourite!= null) NewFavourite(args); };
            _userStream.TweetFavouritedByMe += (sender, args) =>  { if (NewFavourite!= null) NewFavourite(args); };            

            _userStream.FollowedByUser += (sender, args) => { if (NewFollower != null) NewFollower(args); };

            _pulse = new Timer {Interval = 5000};
            _pulse.Elapsed += (sender, args) => {if(Heartbeat!=null)Heartbeat();};
            _pulse.Start();
        }
        #endregion

        public void Start()
        {            
            _userStream.StartStream();
        }
        
        public void Pause()
        {            
            _userStream.PauseStream();
        }

        public void Stop()
        {            
            _userStream.StopStream();
        }

        //var user = User.GetLoggedUser();
        
    }
}
