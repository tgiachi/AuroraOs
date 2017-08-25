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
    [AuroraService("System")]
    public class EventQueueService : IEventQueueService
    {

        private readonly IMessageBus _messageBus;
        private ILogger _logger = LogManager.GetCurrentClassLogger();

       
        public EventQueueService()
        {
            _logger.Info("Initializing message bus");
            _messageBus = new InMemoryMessageBus(new InMemoryMessageBusOptions());

           
        }

        public Task Init()
        {
            return Task.CompletedTask;
        }


        public bool Subscribe<T>(Action<T> action) where T : class
        {
            _messageBus.SubscribeAsync(action);

            return true;
        }

        public async Task Publish<T>(T obj) where T : class
        {
            await _messageBus.PublishAsync(obj);
        }

        public void Dispose()
        {
            _messageBus.Dispose();
        }
    }
}
