using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuroraOs.Common.Core.Data.Dto
{
    public class GraphData
    {
        public string SensorName { get; set; }

      

        public List<GraphDataDetail> Values { get; set; }


        public GraphData()
        {
            Values = new List<GraphDataDetail>();
        }
    }
}
