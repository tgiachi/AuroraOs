using AuroraOs.Common.Core;
using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Data.Dto;
using AuroraOs.Common.Core.Data.Events;
using AuroraOs.Common.Core.Data.IoT;
using AuroraOs.Common.Core.Interfaces;
using AuroraOs.Common.Core.Services.Interfaces;
using AuroraOs.Engine.Core.Interfaces;
using AuroraOs.Entities.Core.Repositories.Interfaces;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Engine.Core.SensorsHandlers
{
    [SensorHandler(typeof(MqttLocationSensor))]
    public class MqttLocationSensorHandler : ISensorHandler
    {
        private readonly IMqttQueueClientService _mqttQueueClientService;
        private readonly ISensorValuesRepository _sensorValuesRepository;
        private readonly IEventQueueService _eventQueueService;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IConfigValuesRepository _configValuesRepository;
        private Dictionary<string, MqttLocationSensor> _sensors = new Dictionary<string, MqttLocationSensor>();

        public MqttLocationSensorHandler(IMqttQueueClientService mqttQueueClientService, ISensorValuesRepository sensorValuesRepository, IEventQueueService eventQueueService, IConfigValuesRepository configValuesRepository)
        {
            _mqttQueueClientService = mqttQueueClientService;
            _eventQueueService = eventQueueService;
            _sensorValuesRepository = sensorValuesRepository;
            _configValuesRepository = configValuesRepository;

            _configValuesRepository.AddConfigValue("home", new Coordinates(43.7496897, 11.3232478));

        }

        Task<bool> ISensorHandler.AddSensor(object obj)
        {
            var sensor = obj as MqttLocationSensor;

            _sensors.Add($"{sensor.StateTopic}/{sensor.Username}/{sensor.DeviceName}", sensor);

            _mqttQueueClientService.Subscribe($"{sensor.StateTopic}/{sensor.Username}/{sensor.DeviceName}", new Action<string, string>((topic, message) =>
            {

                var sens = _sensors[topic];

                if (sens != null)
                {
                    try
                    {
                        var gpsData = JsonConvert.DeserializeObject<OwnTracksLocation>(message);

                        _logger.Info($"Received GPS Data from {gpsData.TrackerId} => Location: {gpsData.Latitude} - {gpsData.Longitude}");

                        _sensorValuesRepository.AddData(sens.Name, typeof(OwnTracksLocation).FullName, message);

                        _eventQueueService.Publish(new SensorValueUpdateEvent()
                        {
                            Sensor = sens,
                            UnitOfMeasurement = typeof(OwnTracksLocation).FullName,
                            Value = message
                        });

                        if (_configValuesRepository.GetConfigByName("home") != null)
                        {
                            var cfgDto = _configValuesRepository.GetConfigByName("home");
                            var home = JsonConvert.DeserializeObject<Coordinates>(cfgDto.Value);

                            var distance = home.DistanceTo(new Coordinates(gpsData.Latitude, gpsData.Longitude));

                            _eventQueueService.Publish(new SensorValueUpdateEvent()
                            {
                                Sensor = sens,
                                UnitOfMeasurement = "km",
                                Value = distance.ToString()
                            });


                            _sensorValuesRepository.AddData(sens.Name, "km", distance.ToString());

                            _logger.Info($"Device {sens.Name} id distance from home {distance} km");
                        }

                    }
                    catch (Exception ex)
                    {

                    }

                }
            }));

            return Task.FromResult(true);
        }

    }
}
