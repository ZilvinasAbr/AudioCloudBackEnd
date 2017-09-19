using System.Collections.Generic;
using SaitynoProjektasBackEnd.RequestModels;
using SaitynoProjektasBackEnd.ResponseModels;

namespace SaitynoProjektasBackEnd.Services
{
    public interface ISongsService
    {
        SongResponseModel GetSongById(int id);
        IEnumerable<SongResponseModel> GetSongs();
        string[] AddSong(AddSongRequestModel song);
        string[] EditSong(int id, EditSongRequestModel song);
        string[] DeleteSong(int id);
        IEnumerable<SongResponseModel> SearchSongs(string query);
    }
}
