using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuroraOs.Entities.Core.Repositories.Interfaces
{
    public interface IAudioEntityRepository
    {
        void AddAudioEntity(string filename, string artist, string title, string albumName,string genre = "", int? year = null);
    }
}
