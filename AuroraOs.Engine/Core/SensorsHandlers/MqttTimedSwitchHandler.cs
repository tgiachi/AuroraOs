using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Data.Events;
using AuroraOs.Common.Core.Data.IoT;
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
    [SensorHandler(typeof(MqttTimedSwitch))]
    public class MqttTimedSwitchHandler : ISensorHandler
    {
        private Dictionary<string, MqttTimedSwitch> _switches = new Dictionary<string, MqttTimedSwitch>();

        private readonly IEventQueueService _eventQueueService;
        private readonly IMqttQueueClientService _mqttQueueClientService;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public MqttTimedSwitchHandler(IEventQueueService eventQueueService, IMqttQueueClientService mqttQueueClientService)
        {
            _eventQueueService = eventQueueService;
            _mqttQueueClientService = mqttQueueClientService;
         

            _eventQueueService.Subscribe(new Action<ToggleSensorEvent>(_event =>
            {

                //_event.Name = "";
                if (_switches.ContainsKey(_event.Name))
                {
                    var sw = _switches[_event.Name];

                    Task.Run(new Action(async () =>
                    {

                        var status1 = _event.State;
                        var status2 = "";
                        if (_event.State == sw.PayloadOn)
                            status2 = sw.PayloadOff;
                        else
                            status2 = sw.PayloadOn;

                        _logger.Info($"Setting switch {sw.Name} to state {status1} for {sw.Seconds / 1000}");


                        _mqttQueueClientService.PublishQueue(sw.CommandTopic, status1);
                        await Task.Delay((int)sw.Seconds);
                        _mqttQueueClientService.PublishQueue(sw.CommandTopic, status2);

                    }));
                }

            }));


        }

        public Task<bool> AddSensor(object obj)
        {

            var ses = obj as MqttTimedSwitch;

            _switches.Add(ses.Name, ses);
            return Task.FromResult(true);
        }
    }
}

