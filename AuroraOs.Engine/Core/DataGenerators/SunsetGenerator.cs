using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Data;
using AuroraOs.Common.Core.Data.Dto;
using AuroraOs.Common.Core.Services.Interfaces;
using AuroraOs.Entities.Core.Repositories.Interfaces;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AuroraOs.Engine.Core.Interfaces;
using AuroraOs.Engine.Core.Services;

namespace AuroraOs.Engine.Core.DataGenerators
{
    [DataGeneratorInfo(3600)]
    public class SunsetGenerator : IDataGenerator
    {
        private HttpClient _httpClient;

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private readonly ISensorsService _sensorsService;

        private readonly IConfigValuesRepository _configValuesRepository;

        public SunsetGenerator(ISensorsService sensorsService, IConfigValuesRepository configValuesRepository)
        {
            _sensorsService = sensorsService;
            _configValuesRepository = configValuesRepository;
            _httpClient = new HttpClient();

        }

        public async void Execute()
        {
            //http://api.sunrise-sunset.org/json?lat=56.960815&lng=-3.752712&Date=2015-03-05

            try
            {
                var coordTxt = _configValuesRepository.GetConfigByName("home");

                if (coordTxt != null)
                {
                    var coordinates = JsonConvert.DeserializeObject<Coordinates>(coordTxt.Value);

                    var lat = coordinates.Latitude.ToString().Replace(",", ".");
                    var lon = coordinates.Longitude.ToString().Replace(",", ".");

                    var date = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

                    var url = $"http://api.sunrise-sunset.org/json?lat={lat}&lng={lon}&Date={date}";


                    var response = await _httpClient.GetAsync(url);


                    if (response.IsSuccessStatusCode)
                    {
                        var strSunset = await response.Content.ReadAsStringAsync();

                        var obj = JsonConvert.DeserializeObject<SunsetData>(strSunset);

                        var sunrise = DateTime.Parse(obj.Results.Sunrise);
                        var sunset = DateTime.Parse(obj.Results.Sunset);

                        _configValuesRepository.AddConfigValue("sunrise", sunrise);
                        _configValuesRepository.AddConfigValue("sunset", sunset);

                        _sensorsService.AddSensorValue("sunrise",  typeof(DateTime).FullName, sunset);
                        _sensorsService.AddSensorValue("sunset", typeof(DateTime).FullName, sunset);

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error during update sunset => {ex.Message} ");

            }

        }
    }
}
