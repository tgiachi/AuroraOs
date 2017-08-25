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
using System.Reactive.Linq;
using AuroraOs.Entities.Core.Repositories.Interfaces;
using Birdhouse;
using Microsoft.Practices.Unity;
using NLog;


namespace AuroraOs.Engine.Core.Services
{
    [AuroraService("Climate")]
    public class NestService : INestService
    {

        [Dependency]
        public ISensorsService SensorService { get; set; }

        [Dependency]
        public IMqttQueueClientService MqttQueueClientService { get; set; }

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private string _clientId;
        private string _secretId;
        private string _accessToken;
        private string _code;
        private NestClient _nestClient;

     
        

        public Task  Init()
        {

            _secretId = ConfigManager.Instance.GetConfigValue<NestService>("secret_id", "0430P42EXLbusyMyC1uGCyhMg");
            _clientId = ConfigManager.Instance.GetConfigValue<NestService>("client_id", "f3a5825e-1000-435c-bbf4-dcf81eecc082");
            _accessToken = ConfigManager.Instance.GetConfigValue<NestService>("access_token", "");
            _code = ConfigManager.Instance.GetConfigValue<NestService>("code", "");


            if (string.IsNullOrEmpty(_code))
            {
                var authorizationUrl = $"https://home.nest.com/login/oauth2?client_id={_clientId}&state=STATE";

                using (var process = Process.Start(authorizationUrl))
                {
                    _logger.Info("Awaiting response, please accept on the Works with Nest page to continue");

                }
            }

            if (!string.IsNullOrEmpty(_accessToken))
            {
                InitializeNestClient();
            }

            return Task.CompletedTask;
        }

        private async void InitializeNestClient()
        {
            try
            {
                _nestClient = new NestClient(_accessToken);
                _logger.Info("Loading Thermostats");

                var thermostats = await _nestClient.GetThermostatsAsync();

                foreach (var thermostat in thermostats)
                {
                    _logger.Info($"Temperature of '{thermostat.Value.Name}' {thermostat.Value.AmbientTemperatureCelsius} C");
                    UpdateValues(thermostat.Value.AmbientTemperatureCelsius, thermostat.Value.Humidity);

                    Observable.Interval(TimeSpan.FromMinutes(5)).Subscribe(async l =>
                    {
                        var thermo = await _nestClient.GetThermostatAsync(thermostat.Key);
                        _logger.Debug($"Updating Thermostat '{thermo.Name}' temperature {thermo.AmbientTemperatureCelsius} - Humidity {thermo.Humidity}");

                        UpdateValues(thermo.AmbientTemperatureCelsius, thermo.Humidity);
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error during initializing NEST => {ex}");
            }
        }
        public void Dispose()
        {

        }

        private void UpdateValues(float temperature, float humidity)
        {
            SensorService.AddSensorValue("home_temperature", "C", temperature);
            SensorService.AddSensorValue("home_humidity", "%", humidity);

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
