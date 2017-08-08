using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Data.Events;
using AuroraOs.Common.Core.Data.IoT;
using AuroraOs.Common.Core.Data.IoT.Internal;
using AuroraOs.Common.Core.Interfaces;
using AuroraOs.Common.Core.Services.Interfaces;
using AuroraOs.Engine.Core.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Engine.Core.SensorsHandlers
{
    [SensorHandler(typeof(MqttSwitch))]
    public class MqttSwitchHandler : ISensorHandler
    {
        private Dictionary<string, MqttStatusSwitch> _switches = new Dictionary<string, MqttStatusSwitch>();

        private IEventQueueService _eventQueueService;
        private IMqttQueueClientService _mqttQueueClientService;
        private ILogger _logger = LogManager.GetCurrentClassLogger();

        public MqttSwitchHandler(IEventQueueService eventQueueService, IMqttQueueClientService mqttQueueClientService)
        {
            _eventQueueService = eventQueueService;
            _mqttQueueClientService = mqttQueueClientService;


            _eventQueueService.Subscribe(new Action<ToggleSensorEvent>(_event =>
            {
                var mSwitch = _switches.Values.FirstOrDefault(s => s.Switch.Name == _event.Name);

                if (mSwitch == null) return;

                if (string.IsNullOrEmpty(_event.State))
                {
                    _event.State = mSwitch.Status ? mSwitch.Switch.PayloadOff : mSwitch.Switch.PayloadOn;
                }

                _logger.Info($"Setting {mSwitch.Switch.Name} to status {_event.State}");

                _mqttQueueClientService.PublishQueue(mSwitch.Switch.CommandTopic, _event.State);
            }));
        }

        public Task<bool> AddSensor(object obj)
        {
            var _switch = obj as MqttSwitch;

            _switches.Add(_switch.Name, new MqttStatusSwitch() { Switch = _switch });

            if (_switch.StateTopic != null)
            {
                _logger.Debug($"Subscribing sensor {_switch.Name} to status");

                _mqttQueueClientService.Subscribe(_switch.StateTopic, (topic, str) =>
                {
                    var mSwitch = _switches.Values.FirstOrDefault(s => s.Switch.StateTopic == topic);

                    if (mSwitch != null)
                    {
                        if (str == mSwitch.Switch.PayloadOn)
                            mSwitch.Status = true;

                        if (str == mSwitch.Switch.PayloadOff)
                            mSwitch.Status = false;
                    }

                });
            }

            _eventQueueService.Publish(new SensorAddedEvent() { Sensor = _switch });

            return Task.FromResult(true);
        }
    }

}

