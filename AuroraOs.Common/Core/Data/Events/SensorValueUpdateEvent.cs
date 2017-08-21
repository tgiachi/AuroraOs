using AuroraOs.Common.Core.Data.IoT.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AuroraOs.Common.Core.Data.Events
{
    public class SensorValueUpdateEvent
    {
        public string SensorName { get; set; }

        public string Value { get; set; }

        public string UnitOfMeasurement { get; set; }
    }
}
