using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Common.Core.Data.Config
{
    public class AuroraConfig
    {

        public Dictionary<string, Dictionary<string, string>> Configs { get; set; }

        public AuroraConfig()
        {
            Configs = new Dictionary<string, Dictionary<string, string>>();
        }
    }
}
