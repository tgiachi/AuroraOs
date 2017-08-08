using AuroraOs.Common.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Engine.Core.Interfaces
{
    public interface IMqttQueueClientService : IAuroraService
    {
        void Subscribe(string topic, Action<string, string> action);

        void PublishQueue(string topic, object message);
    }
}
