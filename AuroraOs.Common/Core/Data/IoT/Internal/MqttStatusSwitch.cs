using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuroraOs.Common.Core.Data.IoT.Internal
{
    public class MqttStatusSwitch 
    {
        public MqttSwitch Switch { get; set; }

        public bool Status { get; set; }
    }
}
