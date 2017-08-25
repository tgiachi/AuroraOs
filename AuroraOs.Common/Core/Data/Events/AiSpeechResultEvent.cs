using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Common.Core.Data.Events
{
    public class AiSpeechResultEvent
    {
        public string ActionName { get; set; }

        public string ResultText { get; set; }
    }
}
