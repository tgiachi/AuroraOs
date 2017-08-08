using AuroraOs.Common.Core.Data.IoT.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuroraOs.Common.Core.Data.Events
{
    public class SensorAddedEvent
    {
        public BaseIot Sensor { get; set; }
    }
}
