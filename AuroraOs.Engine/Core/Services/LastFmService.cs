using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Manager;
using AuroraOs.Engine.Core.Interfaces;
using IF.Lastfm.Core.Api;
using NLog;

namespace AuroraOs.Engine.Core.Services
{
    [AuroraService("Audio")]
    public class LastFmService : ILastFmService
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private LastfmClient _lastFmClient;

        private string _apiKey;
        private string _apiSecret;

        public LastFmService()
        {
          
        }

       public Task Init()
        {
            _apiKey = ConfigManager.Instance.GetConfigValue<LastFmService>("apiKey", "");
            _apiSecret = ConfigManager.Instance.GetConfigValue<LastFmService>("_apiSecret", "");

            _lastFmClient = new LastfmClient(_apiKey, _apiSecret);

            _logger.Info("Last FM client is ready");    

            return Task.CompletedTask;
        }


        public void Dispose()
        {

        }
    }
}
