using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Data;
using AuroraOs.Common.Core.Data.Events;
using AuroraOs.Common.Core.Interfaces;
using AuroraOs.Common.Core.Services.Interfaces;
using AuroraOs.Common.Core.Utils;
using Microsoft.Practices.Unity;
using NLog;

namespace AuroraOs.Engine.Core.Services
{

    [AuroraService("System")]
    public class NotificationService : INotificationService
    {
        private readonly IEventQueueService _eventQueueService;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IUnityContainer _unityContainer;

        private readonly List<INotificatorHandler> _handlers = new List<INotificatorHandler>();

        public NotificationService(IEventQueueService eventQueueService, IUnityContainer unityContainer)
        {
            _eventQueueService = eventQueueService;
            _unityContainer = unityContainer;

            ScanNotificationHandlers();

            _eventQueueService.Subscribe(new Action<SensorValueUpdateEvent>(_event =>
            {
                Notify(new NotificationData()
                {
                    SourceName = _event.SensorName,
                    Data =  _event,
                    Message = $"{_event.SensorName} is {_event.Value} {_event.UnitOfMeasurement}"
                });

            }));
        }

        private async void ScanNotificationHandlers()
        {
            var types = AssemblyUtils.ScanAllAssembliesFromAttribute(typeof(NotificatorAttribute));
            _logger.Debug($"Found {types.Count} notifier");

            foreach (var type in types)
            {
                try
                {
                    _unityContainer.RegisterType(type);

                    var notificationHandler = _unityContainer.Resolve(type) as INotificatorHandler;

                    if (notificationHandler != null)
                    {
                        var initSuccess = await notificationHandler.Init();

                        if (initSuccess)
                            _handlers.Add(notificationHandler);
                    }
                    else
                    {
                        _logger.Warn($"Type {type.Name} don't implements interface INotificatorHandler!");
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error($"Error during initializing type {type.Name} => {ex}");

                }
            }


        }

        private void Notify(NotificationData data)
        {
            _handlers.ForEach(async handler =>
            {
                await handler.Notificate(data);
            });
            
        }

        public void Dispose()
        {
            _handlers.ForEach(s => s.Dispose());

        }
    }
}
