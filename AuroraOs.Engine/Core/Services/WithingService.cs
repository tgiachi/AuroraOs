using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Manager;
using AuroraOs.Engine.Core.Interfaces;
using NLog;
using Withings.NET.Client;
using Withings.NET.Models;

namespace AuroraOs.Engine.Core.Services
{
    [AuroraService("Health")]
    public class WithingService : IWithingService
    {

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private WithingsClient _client;

        private string _apiKey;
        private string _apiSecret;

        private string _userId;
        private string _oauthToken;
        private string _oauthSecret;

        private WithingsCredentials _credentials;


        public WithingService()
        {
  
        }

        public async Task Init()
        {
            _apiKey = ConfigManager.Instance.GetConfigValue<WithingService>("apiKey",
                "19f29057d08e44b218d6c082577e5943dc236f6cc955dc8089551020c5bbf4");
            _apiSecret = ConfigManager.Instance.GetConfigValue<WithingService>("apiSecret",
                "0b6bf1d008bfcc0bb410c72672b050001574bc1b01d26c2668b753313659c");

            _credentials = new WithingsCredentials()
            {
                ConsumerSecret = _apiSecret,
                ConsumerKey = _apiKey,
                CallbackUrl = "http://localhost:9001/api/oauth/withings"
            };

            var authenticator = new Authenticator(_credentials);

            var token = await authenticator.GetRequestToken();
            
            ConfigManager.Instance.SetConfig<WithingService>("token_key", token.Key);
            ConfigManager.Instance.SetConfig<WithingService>("token_secret", token.Secret);


            using (var process = Process.Start(authenticator.UserRequestUrl(token)))
            {
                _logger.Info("Awaiting response, please accept on the Works with Nest page to continue");

            }

            _client = new WithingsClient(_credentials);

        }

        public void Dispose()
        {

        }

        public async void InitializeAuth(string userId, string oauthToken, string Oauth_verifier)
        {
            _oauthToken = oauthToken;
            _userId = userId;
            
            ConfigManager.Instance.SetConfig<WithingService>("userId", userId);
            ConfigManager.Instance.SetConfig<WithingService>("oauthToken", oauthToken);
            ConfigManager.Instance.SetConfig<WithingService>("Oauth_verifier", Oauth_verifier);

            var ok = await _client.GetBodyMeasures(_userId, DateTime.MinValue, _credentials.ConsumerKey, _credentials.ConsumerSecret);
        }
    }
}
