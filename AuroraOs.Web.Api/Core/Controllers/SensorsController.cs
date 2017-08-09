using AuroraOs.Common.Core.Data.Dto;
using AuroraOs.Common.Core.Data.Errors;
using AuroraOs.Common.Core.Data.Events;
using AuroraOs.Common.Core.Data.IoT.Base;
using AuroraOs.Common.Core.Services.Interfaces;
using AuroraOs.Engine.Core.Interfaces;
using AuroraOs.Entities.Core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace AuroraOs.Web.Api.Core.Controllers
{
    [RoutePrefix("api/Sensors")]
    public class SensorsController : ApiController
    {
        private readonly ISensorsService _sensorsService;
        private readonly IEventQueueService _eventQueueService;
        private readonly ISensorValuesRepository _sensorValuesRepository;

        private readonly IEmotionService _emotionService;

        public SensorsController(ISensorsService sensorsService, IEventQueueService eventQueueService, ISensorValuesRepository sensorValuesRepository, IEmotionService emotionService)
        {
            _sensorsService = sensorsService;
            _eventQueueService = eventQueueService;
            _sensorValuesRepository = sensorValuesRepository;
            _emotionService = emotionService;
        }

        [HttpGet]
        public List<BaseIot> All()
        {
            return _sensorsService.GetSensors();
        }

        [HttpGet]

        public List<SensorError> Errors()
        {
            return _sensorsService.GetSensorErrors();
        }

       
        [HttpPost]
        [Route("SetSwitch/{name}/{state}")]
        public string SetSwitch(string name, string state)
        {
            _eventQueueService.Publish(new ToggleSensorEvent()
            {
                Name = name,
                State = state
            });
            return "ok";
        }

        [Route("ToggleSwitch/{name}")]
        [HttpPost]
        public string ToggleSwitch(string name)
        {
            _eventQueueService.Publish(new ToggleSensorEvent()
            {
                Name = name,
            });

            return "ok";
        }

        [Route("SensorHistory")]
        [HttpGet]
        public GraphData SensorHistory(string sensorName)
        {
            return _sensorsService.GetDataSensor(sensorName, DateTime.MinValue, DateTime.MinValue);
        }

        [Route("SensorLastHistory")]
        [HttpGet]
        public GraphData SensorLastHistory(string sensorName, string um = null)
        {
            return _sensorValuesRepository.GetLastSensorData(sensorName, um);
        }


        [Route("SensorValueAvailable")]
        [HttpGet]
        public List<SensorAvailableDto> SensorValueAvailable()
        {
            return _sensorValuesRepository.GetAvaiablesSensorList();
        }

        [Route("AddEmotion")]
        [HttpPost]
        public bool AddEmotion(EmotionData emotionData)
        {
            _emotionService.SetEmotion(emotionData);

            return true;
        }
    }
}

