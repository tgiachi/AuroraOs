using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Data;
using AuroraOs.Common.Core.Interfaces;
using AuroraOs.Common.Core.Manager;
using NLog;
using PushbulletSharp;
using PushbulletSharp.Models.Requests;

namespace AuroraOs.Engine.Core.Notifiers
{
    [Notificator]
    public class PushBulletNotifier : INotificatorHandler
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private PushbulletClient _pushbulletClient;
        private string _apiKey = "";
        

        public Task<bool> Init()
        {

            try
            {
                _apiKey = ConfigManager.Instance.GetConfigValue<PushBulletNotifier>("api", "o.QFNcldj0Ud4g3NNGivUCkXtjmjJB3LVO");
                _pushbulletClient = new PushbulletClient(_apiKey);
                

                return Task.FromResult(true);

            }
            catch (Exception ex)
            {
                _logger.Error($"Error during initializing PushBullet => {ex}");
                return Task.FromResult(false);
            }



        }



        public Task<bool> Notificate(NotificationData data)
        {
            _pushbulletClient.CurrentUsersDevices().Devices.ForEach(s =>
            {
                var request = new PushNoteRequest()
                {
                    ClientIden = _pushbulletClient.CurrentUsersInformation().Iden,
                    DeviceIden = s.Iden,
                    SourceDeviceIden = _pushbulletClient.CurrentUsersInformation().Iden,
                    Title = "AuroraOs Notification",
                    Body = data.Message,
                    ChannelTag = "Notifications",
                    Email = "squid@stormwind.it"
                };

                try
                {
                    _pushbulletClient.PushNote(request);
                }
                catch (Exception ex)
                {
                    _logger.Error($"Can't notifiy => {ex}");
                }
               

            });

            return Task.FromResult(true);
        }

        public void Dispose()
        {

        }
    }
}
