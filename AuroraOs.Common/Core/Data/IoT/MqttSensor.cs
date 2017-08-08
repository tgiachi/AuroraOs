using AuroraOs.Common.Core.Data.IoT.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AuroraOs.Common.Core.Data.IoT
{
    public class MqttSensor : BaseSensor
    {
        public string StateTopic { get; set; }

        public string ValueTemplate { get; set; }
    }
}
