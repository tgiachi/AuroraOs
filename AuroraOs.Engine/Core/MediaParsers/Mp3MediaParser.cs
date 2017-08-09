using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Interfaces;
using Id3;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AuroraOs.Engine.Core.MediaParsers
{
    [MediaServiceType("mp3")]
    public class Mp3MediaParser : IMediaParser
    {
        private ILogger _logger = LogManager.GetCurrentClassLogger();

        public Task<bool> Parse(string filename)
        {
            try
            {
                using (var mp3 = new Mp3Stream(File.ReadAllBytes(filename)))
                {
                    var tag = mp3.GetTag(Id3TagFamily.Version2x);

                    _logger.Info($"{tag.Artists.Value.FirstOrDefault()} - {tag.Title.Value.FirstOrDefault()}"); 

                }
                return Task.FromResult(true);

            }
            catch (Exception ex)
            {
                _logger.Error($"Error during parsing file {filename} => {ex.Message}");
                return Task.FromResult(false);
            }
        }
    }
}
