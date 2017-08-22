using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Manager;
using AuroraOs.Engine.Core.Interfaces;
using Newtonsoft.Json;
using NLog.Internal;
using System.Net.Http;
using NLog;


namespace AuroraOs.Engine.Core.Services
{
    [AuroraService()]
    public class NestService : INestService
    {
        private readonly  ILogger _logger = LogManager.GetCurrentClassLogger();

        private string _clientId;
        private string _secretId;

        public NestService()
        {
            Init();
        }

        private void Init()
        {

            _secretId = ConfigManager.Instance.GetConfigValue<NestService>("secret_id", "0430P42EXLbusyMyC1uGCyhMg");
            _clientId = ConfigManager.Instance.GetConfigValue<NestService>("client_id", "f3a5825e-1000-435c-bbf4-dcf81eecc082");


            if (string.IsNullOrEmpty(ConfigManager.Instance.GetConfigValue<NestService>("code", "")))
            {
                var authorizationUrl = $"https://home.nest.com/login/oauth2?client_id={_clientId}&state=STATE";

                using (var process = Process.Start(authorizationUrl))
                {
                    _logger.Info("Awaiting response, please accept on the Works with Nest page to continue");

                }
            }

        }
        public void Dispose()
        {

        }

        public async void ReceiveCode(string code)
        {
            ConfigManager.Instance.SetConfig<NestService>("code", code);

            var accessToken = await GetAccessToken(code);

            ConfigManager.Instance.SetConfig<NestService>("access_token", accessToken);

        }

        private async Task<string> GetAccessToken(string authorizationCode)
        {
            var url = $"https://api.home.nest.com/oauth2/access_token?code={authorizationCode}&client_id={_clientId}&client_secret={_secretId}&grant_type=authorization_code";

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync(url, content: null))
                {
                    var accessToken = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);

                    return (accessToken as dynamic).access_token;
                }
            }
        }
    }
}
