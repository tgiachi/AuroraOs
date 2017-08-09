﻿using AuroraOs.Common.Core.Attributes;
using AuroraOs.Common.Core.Enums;
using AuroraOs.Common.Core.Services.Interfaces;
using AuroraOs.Entities.Core.Entities;
using AuroraOs.Entities.Core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Entities.Core.Repositories
{

    [AuroraService(AuroraServiceType.PerRequest)]
    public class AudioEntityRepository : IAudioEntityRepository
    {

        private readonly INoSqlService _dbContext;


        public void AddAudioEntity(string filename, string artist, string title, string albumName, TimeSpan duration, string genre = "", int? year = null)
        {
            var ent = new AudioEntity()
            {
                Artist = artist,
                AlbumName = albumName,
                Title = title,
                Duration = duration,
                Genre = genre,

                Filename = filename

            };

            if (year.HasValue)
                ent.Year = year.Value;

            _dbContext.Insert(ent);
        }
    }
}