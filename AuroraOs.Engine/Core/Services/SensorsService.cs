using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Data.Dto;
using AuroraOs.Common.Core.Data.Errors;
using AuroraOs.Common.Core.Data.Events;
using AuroraOs.Common.Core.Data.IoT;
using AuroraOs.Common.Core.Data.IoT.Base;
using AuroraOs.Common.Core.Interfaces;
using AuroraOs.Common.Core.Manager;
using AuroraOs.Common.Core.Services.Interfaces;
using AuroraOs.Common.Core.Utils;
using AuroraOs.Engine.Core.Interfaces;
using AuroraOs.Entities.Core.Repositories.Interfaces;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Engine.Core.Services
{
    [AuroraService("System")]
    public class SensorsService : ISensorsService
    {

        private readonly IMqttQueueClientService _mqttQueueClientService;
        private readonly IEventQueueService _eventQueueService;

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly ISensorValuesRepository _sensorValuesRepository;
        private readonly IUnityContainer _container;

        private readonly List<SensorError> _sensorErrors = new List<SensorError>();

        private readonly List<BaseIot> _sensors = new List<BaseIot>();


        private readonly Dictionary<string, Type> _handlers = new Dictionary<string, Type>();

        private readonly Dictionary<string, Type> _handlersData = new Dictionary<string, Type>();

        public SensorsService(IMqttQueueClientService mqttQueueClientService, IEventQueueService eventQueueService, ISensorValuesRepository sensorValuesRepository, IUnityContainer unityContainer)
        {
            _eventQueueService = eventQueueService;
            _container = unityContainer;
            _mqttQueueClientService = mqttQueueClientService;
            _sensorValuesRepository = sensorValuesRepository;

            _logger.Info("Loading sensors...");

            _eventQueueService.Subscribe(new Action<SensorAddedEvent>(sensor =>
            {
                _sensors.Add(sensor.Sensor);
            }));


            TestSensors();

            InitSersorHandlers();
        }

        public List<BaseIot> GetSensors()
        {
            return _sensors;
        }

        private void TestSensors()
        {
            var testMq = new MqttSwitch()
            {
                CommandTopic = "home/bedroom/switch1/set",
                StateTopic = "home/bedroom/switch1",
                Room = "Bedroom",
                Name = "Switch Bedroom",
                Type = typeof(MqttSwitch).Name
            };

            File.WriteAllText($"{ConfigManager.Instance.GetPath("sensors")}switch1_bedroom_switch.sensor", JsonConvert.SerializeObject(testMq));

            var testSensor = new MqttSensor()
            {
                Name = "Temperature",
                Room = "Bedroom",
                StateTopic = "home/bedroom/switch1",
                UnitOfMeasurement = "C",
                Type = typeof(MqttSensor).Name
            };

            File.WriteAllText($"{ConfigManager.Instance.GetPath("sensors")}switch1_bedroom.sensor", JsonConvert.SerializeObject(testSensor));


            var testMqGpsSensors = new MqttLocationSensor()
            {
                StateTopic = "owntracks",
                Name = "axon7",
                DeviceName = "axon7",
                Username = "owntracks",
                Type = typeof(MqttLocationSensor).Name
            };

            File.WriteAllText($"{ConfigManager.Instance.GetPath("sensors")}axon7.sensor", JsonConvert.SerializeObject(testMqGpsSensors));
        }

        private void InitSensors()
        {
            var files = Directory.GetFiles(ConfigManager.Instance.GetPath("sensors"));

            foreach (var file in files)
            {
                _logger.Info($"Loading sensor {file}");

                try
                {
                    var obj = JsonConvert.DeserializeObject(File.ReadAllText(file)) as JObject;

                    if (obj["Name"] == null)
                        throw new Exception("Name cannot be null");

                    var objType = obj["Type"].Value<string>();

                    if (_handlers.ContainsKey(objType))
                    {
                        var sh = _container.Resolve(_handlers[objType]) as ISensorHandler;
                        _logger.Info($"Adding sensor {obj["Name"].Value<string>()}");

                        sh?.AddSensor(JsonConvert.DeserializeObject(File.ReadAllText(file), _handlersData[objType]));
                    }
                    else
                    {
                        _sensorErrors.Add(new SensorError()
                        {
                            Name = objType,
                            Error = $"Object type {objType} don't have handler!"
                        });

                        _logger.Warn($"Object type {objType} don't have handler!");
                    }
                }
                catch (Exception ex)
                {
                    _sensorErrors.Add(new SensorError()
                    {
                        Name = file,
                        Error = ex.Message
                    });
                }
            }
        }

        public List<SensorError> GetSensorErrors()
        {
            return _sensorErrors;
        }

        public GraphData GetDataSensor(string sensorName, DateTime fromDate, DateTime toDate)
        {

            return _sensorValuesRepository.GetSensorData(sensorName, fromDate, toDate);
        }

        private bool InitSersorHandlers()
        {
            var types = AssemblyUtils.ScanAllAssembliesFromAttribute(typeof(SensorHandlerAttribute));

            _logger.Info($"Found {types.Count} sensors handlers");

            foreach (var type in types)
            {
                var attr = type.GetCustomAttribute<SensorHandlerAttribute>();

                _logger.Info($"Handler {type.Name} for type => {attr.SensorType.Name}");

                _handlers.Add(attr.SensorType.Name, type);

                _handlersData.Add(attr.SensorType.Name, attr.SensorType);

                _container.RegisterType(type, new ContainerControlledLifetimeManager());
            }

            InitSensors();

            return true;
        }

        public bool AddSensorValue(string sensorName, string type, object data)
        {
            var s = data as string;
            if (s != null)
            {
                _sensorValuesRepository.AddData(sensorName, type, s);

            }
            else
            {
                type = data.GetType().FullName;
                _sensorValuesRepository.AddData(sensorName, type, data.ToJson());
            }

          
            return true;
            // _sensorsValuesRepository.AddData("weather_temperature", "°C", Convert.ToString(result.currently.temperature));

        }

        public void Dispose()
        {

        }
    }
}
