using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Common.Core.Attributes
{

    [AttributeUsage(AttributeTargets.Class)]
    public class MediaServiceTypeAttribute : Attribute
    {
        public string FileExtension { get; set; }

        public MediaServiceTypeAttribute(string fileExt)
        {
            FileExtension = fileExt;
        }
    }
}
