using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Manager;
using AuroraOs.Engine.Core.Interfaces;
using TMDbLib.Client;

namespace AuroraOs.Engine.Core.Services
{
    [AuroraService("Movies")]
    public class TmDbService : ITmDbService
    {
        private TMDbClient _tmdbClient;

        private string _apiKey;


      

        public Task Init()
        {
            _apiKey = ConfigManager.Instance.GetConfigValue<TmDbService>("apiKey", "87bd0db42988cd4de3e514bbe540cbfa");

            _tmdbClient = new TMDbClient(_apiKey);

            return Task.CompletedTask;
        }
        
        public void Dispose()
        {
        }
    }
}
