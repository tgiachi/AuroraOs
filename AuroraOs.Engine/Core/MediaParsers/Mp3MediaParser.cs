using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Interfaces;
using AuroraOs.Entities.Core.Repositories.Interfaces;
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
        private readonly IAudioEntityRepository _audioRepo;

        private ILogger _logger = LogManager.GetCurrentClassLogger();

        public Mp3MediaParser(IAudioEntityRepository audioRepo)
        {
            _audioRepo = audioRepo;
        }

        

        public Task<bool> Parse(string filename)
        {
            try
            {
                using (var mp3 = new Mp3Stream(File.ReadAllBytes(filename)))
                {
                    var tag = mp3.GetTag(Id3TagFamily.Version2x);

                    _logger.Info($"{tag.Artists.Value.FirstOrDefault()} - {tag.Track.Value}");

                    _audioRepo.AddAudioEntity(filename, tag.Artists.Value.FirstOrDefault(),tag.Title.Value , tag.Album.Value, tag.Length.Value);

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
