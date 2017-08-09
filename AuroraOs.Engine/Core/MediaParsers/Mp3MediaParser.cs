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
using TagLib;

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

            //TagLib.File.Create(new StreamFileAbstraction())



            try
            {
                using (var fs = new FileStream(filename, FileMode.Open))
                {
                    var tagFile = TagLib.File.Create(new StreamFileAbstraction(filename, fs, fs));

                    var tags = tagFile.GetTag(TagTypes.Id3v2);

                    _audioRepo.AddAudioEntity(filename, tags.FirstAlbumArtist, tags.Title, tags.Album, tags.FirstGenre, (int)tags.Year);

                    _logger.Info($"{tags.FirstAlbumArtist} - {tags.Title}");
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
