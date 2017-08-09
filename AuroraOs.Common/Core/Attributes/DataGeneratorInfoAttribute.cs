using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Common.Core.Attributes
{
    public class DataGeneratorInfoAttribute : Attribute
    {
        public int Seconds { get; set; }

        public DataGeneratorInfoAttribute(int seconds)
        {
            Seconds = seconds;
        }
    }
}
