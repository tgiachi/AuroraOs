using AuroraOs.Common.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Common.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SensorHandlerAttribute : Attribute
    {
        public Type SensorType { get; set; }

        public SensorHandlerAttribute(Type sensorType)
        {
            SensorType = sensorType;
        }
    }
}
