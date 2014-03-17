using System;
using System.Configuration;
using System.Timers;
using Microsoft.Win32;
using Tweetinvi;
using TweetinviCore.Events.EventArguments;
using TweetinviCore.Interfaces.Credentials;
using TweetinviCore.Interfaces.Streaminvi;

namespace CovertTweeter
{
    public class TweetRepository : ITweetRepository
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

        public event EventHandler<TweetReceivedEventArgs> NewTweet;
        public event EventHandler<MessageEventArgs> NewMessage;
        public event EventHandler<UserFollowedEventArgs> NewFollower;
        public event EventHandler<TweetFavouritedEventArgs> NewFavourite;
        
        public event EventHandler StreamStopped;
        public event EventHandler StreamStarted;
        
        public event EventHandler<PulseEventArgs> Heartbeat;

        private void CreateEventBindings()
        {            
            _userStream.TweetCreatedByMe += (sender, args) => { if (NewTweet != null) NewTweet(this,args); };
            _userStream.TweetCreatedByFriend += (sender, args) => { if (NewTweet != null) NewTweet(this,args); };

            _userStream.MessageReceived += (sender, args) => { if (NewMessage!= null) NewMessage(this,args); };

            _userStream.TweetFavouritedByAnyoneButMe += (sender, args) =>  { if (NewFavourite!= null) NewFavourite(this,args); };
            _userStream.TweetFavouritedByMe += (sender, args) =>  { if (NewFavourite!= null) NewFavourite(this,args); };

            _userStream.FollowedByUser += (sender, args) => { if (NewFollower != null) NewFollower(this,args); };

            _userStream.StreamStopped += OnStreamStopped;
            _userStream.StreamStarted += OnStreamStarted;
         
            _pulse = new Timer {Interval = 5000};               
            _pulse.Elapsed += (sender, args) => { 
                if (Heartbeat != null) Heartbeat(this, new PulseEventArgs("."));
                _pulse.Start();
            };
        }

        private void OnStreamStarted(object sender, EventArgs args)
        {
            if (StreamStarted != null) StreamStarted(this, args);
            _pulse.Stop();
        }

        private void OnStreamStopped(object sender, StreamExceptionEventArgs args)
        {
            if (StreamStarted != null) StreamStopped(this, args);
            _pulse.Start();
        }

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

        public ILoggedUser GetCurrentUser()
        {
            return User.GetLoggedUser();
        }

        //var user = User.GetLoggedUser();
        
    }
}
