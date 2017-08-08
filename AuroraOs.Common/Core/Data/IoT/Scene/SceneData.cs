using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuroraOs.Common.Core.Data.IoT.Scene
{
    public class SceneData
    {
        public string Name { get; set; }

        public List<SceneSensor> Entities { get; set; }

        public SceneTrigger Trigger { get; set; }

        public SceneData()
        {
            Trigger = new SceneTrigger();
            Entities = new List<SceneSensor>();
        }
    }
}

