using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuroraOs.Common.Core.Data.IoT.Scene
{
    public class SceneTrigger
    {
        public string SensorName { get; set; }

        public TriggerComparatorEnum Comparator { get; set; }

        public string Value { get; set; }

        public string Expression { get; set; }

        public bool OneSceneTrigger { get; set; }
        
    }
}
