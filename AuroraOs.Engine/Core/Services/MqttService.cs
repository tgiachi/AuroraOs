using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Data.Events;
using AuroraOs.Common.Core.Manager;
using AuroraOs.Common.Core.Services.Interfaces;
using AuroraOs.Common.Core.Utils;
using AuroraOs.Engine.Core.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace AuroraOs.Engine.Core.Services
{
    [AuroraService]
    public class MqttService : IMqttService
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private MqttClient _mqttClient;

        private readonly IEventBusService _eventBusService;

        private static string CommandsTopic = "home.manager/system/commands/";

        private Dictionary<string, List<Action<string, string>>> _subcriptions = new Dictionary<string, List<Action<string, string>>>();

        public MqttService(IEventBusService eventBusService)
        {

            Init();
            // _mqttClient = new MqttClient()
            _eventBusService = eventBusService;


            _eventBusService.SubscribeAsync<MqttSendEvent>(_event =>
            {
                if (_mqttClient.IsConnected)
                {
                    PublishQueue(_event.Topic, _event.Message);
                }
                else
                {
                    _logger.Warn($"Can't send message to topic {_event.Topic}, client not connected!");
                }
            });
        }

        private void Init()
        {
            var host = ConfigManager.Instance.GetConfigValue<MqttService>("mqtt_host", "test.mosquitto.org");
            var port = int.Parse(ConfigManager.Instance.GetConfigValue<MqttService>("mqtt_port", "1883"));

            try
            {
                _logger.Info($"Connecting to MQTT server {host}:{port}");
                _mqttClient = new MqttClient(host, port, false, MqttSslProtocols.None, null, null);
                _mqttClient.Connect("AuroraOS"+ "_" + "1.0");

                _mqttClient.MqttMsgPublishReceived += MqttClientOnMqttMsgPublishReceived;
                _mqttClient.MqttMsgPublished += MqttClientOnMqttMsgPublished;
                _mqttClient.MqttMsgSubscribed += MqttClientOnMqttMsgSubscribed;

                _logger.Info($"Connected to {host}:{port}");

            }
            catch (Exception ex)
            {
                _logger.Error($"Error during connect to {host}:{port}");
                _logger.Error(ex);
            }

        }

        public void PublishQueue(string topic, object message)
        {
            if (!_mqttClient.IsConnected) return;

            var text = "";

            if (message is string strMessage)
            {
                text = strMessage;
            }
            else
            {
                text = message.ToJson();
            }

            _logger.Info($"Publish to topic {topic} message => {message}");
            _mqttClient.Publish(topic, Encoding.UTF8.GetBytes(text));
        }


        public void Subscribe(string topic, Action<string, string> action)
        {
            if (!_subcriptions.ContainsKey(topic))
            {
                _subcriptions.Add(topic, new List<Action<string, string>>());

                _mqttClient.Subscribe(new[] { topic }, new byte[] { 0 });
            }

            _subcriptions[topic].Add(action);
        }

        private void MqttClientOnMqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs mqttMsgSubscribedEventArgs)
        {

        }

        private void MqttClientOnMqttMsgPublished(object sender, MqttMsgPublishedEventArgs mqttMsgPublishedEventArgs)
        {
            _logger.Debug($"Message {mqttMsgPublishedEventArgs.MessageId} is published {mqttMsgPublishedEventArgs.IsPublished}");
        }

        private void MqttClientOnMqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs mqttMsgPublishEventArgs)
        {
            _logger.Debug($"Received message from topic {mqttMsgPublishEventArgs.Topic} ({mqttMsgPublishEventArgs.Message.Length} bytes)");

            if (mqttMsgPublishEventArgs.Topic == CommandsTopic)
            {
                ParseSystemCommand(Encoding.UTF8.GetString(mqttMsgPublishEventArgs.Message));
                return;
            }

            if (!_subcriptions.ContainsKey(mqttMsgPublishEventArgs.Topic)) return;


            foreach (var action in _subcriptions[mqttMsgPublishEventArgs.Topic])
            {
                action.Invoke(mqttMsgPublishEventArgs.Topic,
                    Encoding.UTF8.GetString(mqttMsgPublishEventArgs.Message));
            }
        }

        private void ParseSystemCommand(string msg)
        {

        }


        public void Dispose()
        {
            if (_mqttClient.IsConnected)
                _mqttClient.Disconnect();
        }
    }
}
