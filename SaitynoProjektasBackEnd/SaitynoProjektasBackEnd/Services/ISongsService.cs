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
        Task<string[]> AddSong(AddSongRequestModel song, string userName);
        string[] EditSong(int id, EditSongRequestModel song, string userName);
        Task<string[]> DeleteSong(int id, string userName);
        IEnumerable<SongResponseModel> SearchSongs(string query);
        string[] GetSongsByGenre(string genreName, out IEnumerable<SongResponseModel> songs);
    }
}
