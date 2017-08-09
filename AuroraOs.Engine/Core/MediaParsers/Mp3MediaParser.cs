using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Interfaces;
using AuroraOs.Entities.Core.Repositories.Interfaces;

using NLog;
using ParkSquare.Gracenote;
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
        private GracenoteClient _graceNoteClient;
 

        private ILogger _logger = LogManager.GetCurrentClassLogger();

        public Mp3MediaParser(IAudioEntityRepository audioRepo)
        {
            _audioRepo = audioRepo;
            _graceNoteClient = new GracenoteClient("2053684250-A8BB6A7AB9D57251B49EE0E2A4A4743E");
            
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

                    var result =  _graceNoteClient.Search(new SearchCriteria()
                    {
                        Artist = tags.FirstAlbumArtist,
                        TrackTitle = tags.Title,

                    });

                    var f = result.Albums.FirstOrDefault();

                   _audioRepo.AddAudioEntity(filename, f.Artist, f.Title, tags.Album, f.Genre.FirstOrDefault(), f.Year);

                    //_logger.Info($"{tags.FirstAlbumArtist} - {tags.Title}");
                }

                return Task.FromResult(true);

            }
            catch (Exception ex)
            {
                _logger.Error($"Error during parsing file {filename} => {ex.Message}");
                _logger.Error(ex);
                return Task.FromResult(false);
            }
        }
    }
}
