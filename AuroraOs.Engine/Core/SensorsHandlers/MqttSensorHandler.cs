using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Data.Events;
using AuroraOs.Common.Core.Data.IoT;
using AuroraOs.Common.Core.Interfaces;
using AuroraOs.Common.Core.Services.Interfaces;
using AuroraOs.Engine.Core.Interfaces;
using AuroraOs.Entities.Core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Engine.Core.SensorsHandlers
{
    [SensorHandler(typeof(MqttSensor))]
    public class MqttSensorHandler : ISensorHandler
    {
        private readonly IMqttQueueClientService _mqttQueueClientService;


        private readonly ISensorValuesRepository _sensorValuesRepository;
        private readonly IEventQueueService _eventQueueService;

        private readonly IJsService _jsService;

        private Dictionary<string, MqttSensor> _sensors = new Dictionary<string, MqttSensor>();

        public MqttSensorHandler(IMqttQueueClientService mqttQueueClientService, ISensorValuesRepository sensorValuesRepository, IEventQueueService eventQueueService, IJsService jsService)
        {
            _mqttQueueClientService = mqttQueueClientService;
            _sensorValuesRepository = sensorValuesRepository;
            _eventQueueService = eventQueueService;
            _jsService = jsService;
        }

        public Task<bool> AddSensor(object obj)
        {
            var sensor = obj as MqttSensor;
            _sensors.Add(sensor.StateTopic, sensor);
            InitSensor(sensor);

            return Task.FromResult(true);
        }

        private void InitSensor(MqttSensor sensor)
        {
            _mqttQueueClientService.Subscribe(sensor.StateTopic, (topic, message) =>
            {
                var s = _sensors[topic];

                if (string.IsNullOrEmpty(s.ValueTemplate))
                {
                    _sensorValuesRepository.AddData(s.Name, s.UnitOfMeasurement, message);
                }
                else
                {

                }




            });

        }
    }
}
