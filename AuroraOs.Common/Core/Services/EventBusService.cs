using AuroraOs.Common.Core.Services.Interfaces;
using Foundatio.Messaging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using AuroraOs.Common.Core.Attributes;

namespace AuroraOs.Common.Core.Services
{
    [AuroraService]
    public class EventBusService : IEventBusService
    {

        private readonly IMessageBus _messageBus;
        private ILogger _logger = LogManager.GetCurrentClassLogger();

       
        public EventBusService()
        {
            _logger.Info("Initializing message bus");
            _messageBus = new InMemoryMessageBus(new InMemoryMessageBusOptions());

           
        }

        

        public Task PublishAsync(Type messageType, object message, TimeSpan? delay = default(TimeSpan?), CancellationToken cancellationToken = default(CancellationToken))
        {
            return _messageBus.PublishAsync(messageType, message, delay, cancellationToken);
        }

        public Task PublishAsync<T>(T message, TimeSpan? delay = default(TimeSpan?)) where T : class
        {
            return _messageBus.PublishAsync(message, delay);
        }

        public Task SubscribeAsync<T>(Action<T> handler) where T : class
        {
            return _messageBus.SubscribeAsync(handler);
        }

        public void Dispose()
        {
            _messageBus.Dispose();
        }
    }
}
