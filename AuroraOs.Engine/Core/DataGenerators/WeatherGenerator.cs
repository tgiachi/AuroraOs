using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Data.Dto;
using AuroraOs.Common.Core.Manager;
using AuroraOs.Common.Core.Services.Interfaces;
using AuroraOs.Common.Core.Utils;
using AuroraOs.Engine.Core.Interfaces;
using AuroraOs.Entities.Core.Repositories.Interfaces;
using ForecastIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Engine.Core.DataGenerators
{

    /// <summary>
    /// Every 5 Minutes
    /// </summary>
    [DataGeneratorInfo(60 )]
    public class WeatherGenerator : IDataGenerator
    {
        private IConfigValuesRepository _configValuesRepository;
        private ISensorValuesRepository _sensorsValuesRepository;
        private ISensorsService _sensorsService;

        public WeatherGenerator(IConfigValuesRepository configValuesRepository, ISensorValuesRepository sensorsValuesRepository, ISensorsService sensorsService)
        {
            _configValuesRepository = configValuesRepository;
            _sensorsValuesRepository = sensorsValuesRepository;
            _sensorsService = sensorsService;
        }

        public async void Execute()
        {
            try
            {
                var home = _configValuesRepository.GetConfigByName("home").Value.FromJson<Coordinates>();

                var request = new ForecastIORequest("12c1a49d1426f8bd57aa42e88eb37486", (float)home.Latitude, (float)home.Longitude, Unit.auto, Language.it);

                var result = await request.GetAsync();


                _sensorsService.AddSensorValue("weather_temperature", "°C", Convert.ToString(result.currently.temperature));
                _sensorsService.AddSensorValue("weather_icon", "image",result.currently.icon);
                _sensorsService.AddSensorValue("weather_pressure", "millBar", Convert.ToString(result.currently.pressure));
                _sensorsService.AddSensorValue("weather_precipitation_prob", "%", Convert.ToString(result.currently.precipProbability));

            }
            catch (Exception ex)
            {

            }
        }
    }
}
