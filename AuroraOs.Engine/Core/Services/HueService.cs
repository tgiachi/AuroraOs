using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Manager;
using AuroraOs.Engine.Core.Interfaces;
using NLog;
using Q42.HueApi;
using Q42.HueApi.Interfaces;

namespace AuroraOs.Engine.Core.Services
{
    [AuroraService("Lighting")]
    public class HueService : IHueService
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private ILocalHueClient hueClient;

        public HueService()
        {
           

        }

        public async Task Init()
        {
            var apiKey = ConfigManager.Instance.GetConfigValue<HueService>("apiKey", "");
            var bridgeAddress = ConfigManager.Instance.GetConfigValue<HueService>("bridgeIpAddress", "");

            if (string.IsNullOrEmpty(apiKey))
            {
                var locator = new HttpBridgeLocator();

                //For Windows 8 and .NET45 projects you can use the SSDPBridgeLocator which actually scans your network. 
                //See the included BridgeDiscoveryTests and the specific .NET and .WinRT projects
                var bridgeIPs = await locator.LocateBridgesAsync(TimeSpan.FromSeconds(30));

                if (bridgeIPs.Any())
                {
                    hueClient = new LocalHueClient(bridgeIPs.FirstOrDefault().IpAddress);
                    _logger.Info($"Found bridgeId: {bridgeIPs.FirstOrDefault().BridgeId}");

                    apiKey = await hueClient.RegisterAsync("AuroraOS", "AuroraOS");

                    ConfigManager.Instance.SetConfig<HueService>("apiKey", apiKey);
                    ConfigManager.Instance.SetConfig<HueService>("bridgeIpAddress", bridgeIPs.FirstOrDefault().IpAddress);
                }
            }
            else
            {
                hueClient = new LocalHueClient(bridgeAddress, apiKey);
            }



            //foreach (var light in await hueClient.GetLightsAsync())
            //{
            //    await hueClient.SendCommandAsync(new LightCommand() {BrightnessIncrement = 40},
            //        new List<string>() {light.Id});
            //}



        }

        public void Dispose()
        {
        }
    }
}
