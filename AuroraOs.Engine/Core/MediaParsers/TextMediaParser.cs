using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Engine.Core.MediaParsers
{

    [MediaServiceType("txt")]
    public class TextMediaParser : IMediaParser
    {

        private ILogger _logger = LogManager.GetCurrentClassLogger();

        public Task<bool> Parse(string filename)
        {
            var f = File.ReadAllText(filename).Length;

            _logger.Info($"File size: {f}");

            return Task.FromResult(true);
        }
    }
}
