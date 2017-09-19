using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SaitynoProjektasBackEnd.RequestModels;
using SaitynoProjektasBackEnd.ResponseModels;

namespace SaitynoProjektasBackEnd.Services
{
    public interface ISongsService
    {
        SongResponseModel GetSongById(int id);
        IEnumerable<SongResponseModel> GetSongs();
        bool AddSong(AddSongRequestModel song);
    }
}
