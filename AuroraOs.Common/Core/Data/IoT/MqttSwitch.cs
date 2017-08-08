using AuroraOs.Common.Core.Data.IoT.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AuroraOs.Common.Core.Data.IoT
{
    public class MqttSwitch : BaseMqtt
    {
        public string CommandTopic { get; set; }


        public string PayloadOn { get; set; } = "ON";

        public string PayloadOff { get; set; } = "OFF";

    }
}
