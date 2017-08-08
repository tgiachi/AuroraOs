using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Common.Core.Data.Events
{
    public class MqttSendEvent
    {
        public string Topic { get; set; }

        public object Message { get; set; }
    }
}
