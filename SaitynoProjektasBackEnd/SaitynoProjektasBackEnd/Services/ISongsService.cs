using System.Collections.Generic;
using System.Threading.Tasks;
using SaitynoProjektasBackEnd.RequestModels;
using SaitynoProjektasBackEnd.ResponseModels;

namespace SaitynoProjektasBackEnd.Services
{
    public interface ISongsService
    {
        SongResponseModel GetSongById(int id);
        IEnumerable<SongResponseModel> GetSongs();
        Task<string[]> AddSong(AddSongRequestModel song, string authId);
        string[] EditSong(int id, EditSongRequestModel song, string authId);
        Task<string[]> DeleteSong(int id, string authId);
        IEnumerable<SongResponseModel> SearchSongs(string query);
        string[] GetSongsByGenre(string genreName, out IEnumerable<SongResponseModel> songs);
    }
}
