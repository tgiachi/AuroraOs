using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuroraOs.Common.Core.Data.Events
{
    public class MqttSendMessageEvent
    {
        public string Topic { get; set; }


        public string Message { get; set; }
    }
}
