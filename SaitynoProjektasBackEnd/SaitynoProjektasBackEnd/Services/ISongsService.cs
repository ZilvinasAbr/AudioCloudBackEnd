using System.Collections.Generic;
using SaitynoProjektasBackEnd.RequestModels;
using SaitynoProjektasBackEnd.ResponseModels;

namespace SaitynoProjektasBackEnd.Services
{
    public interface ISongsService
    {
        SongResponseModel GetSongById(int id);
        IEnumerable<SongResponseModel> GetSongs();
        string[] AddSong(AddSongRequestModel song, string userName);
        string[] EditSong(int id, EditSongRequestModel song);
        string[] DeleteSong(int id);
        IEnumerable<SongResponseModel> SearchSongs(string query);
        string[] GetSongsByGenre(string genreName, out IEnumerable<SongResponseModel> songs);
    }
}
