using AuroraOs.Common.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Entities.Core.Entities
{

    public class AudioEntity : BaseNoSqlEntity
    {
        public string Artist { get; set; }

        public string AlbumName { get; set; }

        public string Title { get; set; }

      

        public int Year { get; set; }

        public string Genre { get; set; }

        public string Filename { get; set; }
    }
}
