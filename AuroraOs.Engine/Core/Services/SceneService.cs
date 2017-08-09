using AuroraOs.Common.Core;
using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Data.Dto;
using AuroraOs.Common.Core.Data.Events;
using AuroraOs.Common.Core.Data.IoT.Scene;
using AuroraOs.Common.Core.Manager;
using AuroraOs.Common.Core.Services.Interfaces;
using AuroraOs.Engine.Core.Interfaces;
using AuroraOs.Entities.Core.Repositories.Interfaces;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Engine.Core.Services
{

    [AuroraService]
    public class SceneService : ISceneService
    {
        private readonly IEventQueueService _eventQueueService;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IJsService _jsService;
        private readonly IConfigValuesRepository _configValuesRepository;

        private List<SceneData> _scenes = new List<SceneData>();


        public SceneService(IEventQueueService eventQueueService, IJsService jsService, IConfigValuesRepository configValuesRepository)
        {
            _eventQueueService = eventQueueService;
           
            _jsService = jsService;
            _configValuesRepository = configValuesRepository;


            _eventQueueService.Subscribe(new Action<SensorValueUpdateEvent>(OnSensorValueUpdate));

            _configValuesRepository.AddConfigValue("home", new Coordinates(43.7496897, 11.3232478));

            TestScenes();

            InitJsFunctions();
            LoadScenes();


        }



        private void InitJsFunctions()
        {
            _jsService.RegisterFunction("config_str", new Func<string, string>(s =>
            _configValuesRepository.GetConfigByName(s).Value)
            );

            _jsService.RegisterFunction("sum", new Func<int, int, int>((arg1, arg2) =>
            {
                return arg1 + arg2;
            }));


            _jsService.RegisterFunction("config_obj", new Func<string, object>(s =>
            {
                var obj = _configValuesRepository.GetConfigByName(s);

                return JsonConvert.DeserializeObject(obj.Value, Type.GetType(obj.Type));

            }));

            _jsService.RegisterFunction("distance_to", new Func<double, double, double, double, double>(
                (lat1, lon1, lat2, lon2) =>
                {
                    var coord1 = new Coordinates(lat1, lon1);
                    var coord2 = new Coordinates(lat2, lon2);

                    return coord1.DistanceTo(coord2);

                }));

            _jsService.RegisterFunction("print", new Func<object, string>((arg) =>
            {
                return "s";
            }));

        }

        private void TestScenes()
        {
            var ts = new SceneData()
            {
                Name = "Test scene",
                Entities = new List<SceneSensor>()
                {
                    new SceneSensor()
                    {
                        Name = "Bedroom",
                        State = "OK"
                    }
                },
                Trigger = new SceneTrigger()
                {
                    SensorName = "temperature",
                    Comparator = TriggerComparatorEnum.GreaterThan,
                    Value = "30"
                }
            };


            File.WriteAllText($"{ConfigManager.Instance.GetPath("scenes")}test.scene", JsonConvert.SerializeObject(ts));
        }

        private void LoadScenes()
        {
            var files = Directory.GetFiles(ConfigManager.Instance.GetPath("scenes"));

            foreach (var file in files)
            {
                try
                {
                    var obj = JsonConvert.DeserializeObject<SceneData>(File.ReadAllText(file));

                    _logger.Info($"Adding {obj.Name} scene");
                    _scenes.Add(obj);

                }
                catch (Exception ex)
                {
                    _logger.Error($"Error during loading file {file} => {ex.Message}");
                }
            }

        }

        private void OnSensorValueUpdate(SensorValueUpdateEvent sensorValueUpdateEvent)
        {


        }

        public List<SceneData> Scenes()
        {
            return _scenes;
        }

        public void StartScene(string sceneName)
        {
            var scene = _scenes.FirstOrDefault(s => s.Name == sceneName);

            if (scene != null)
            {
                _logger.Info($"Starting scene {sceneName}");

                foreach (var entity in scene.Entities)
                {
                    _logger.Info($"Toggle {entity.Name} => {entity.State} ");
                    _eventQueueService.Publish(new ToggleSensorEvent()
                    {
                        State = entity.State,
                        Name = entity.Name
                    });
                }
            }
        }

        public void Dispose()
        {
        }


    }
}
