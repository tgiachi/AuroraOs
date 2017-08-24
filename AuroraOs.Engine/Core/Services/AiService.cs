using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiAiSDK;
using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Manager;
using AuroraOs.Engine.Core.Interfaces;
using NLog;
using Org.BouncyCastle.Asn1.Crmf;

namespace AuroraOs.Engine.Core.Services
{
    [AuroraService("Ai")]
    public class AiService : IAiService
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private ApiAi _apiAiClient;
        private string _accessToken;


        public AiService()
        {
            Init();
        }

        private void Init()
        {
            _accessToken = ConfigManager.Instance.GetConfigValue<AiService>("accessToken", "e143c1b4710847faaf0b08d7cb7374b9");

            var config = new AIConfiguration(_accessToken, SupportedLanguage.Italian);

            _apiAiClient = new ApiAi(config);
        }

        public void Dispose()
        {
        }
    }
}
