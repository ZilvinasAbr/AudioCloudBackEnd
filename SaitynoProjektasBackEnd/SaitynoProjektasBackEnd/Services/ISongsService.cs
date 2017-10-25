using System.Collections.Generic;
using System.Threading.Tasks;
using SaitynoProjektasBackEnd.Models;
using SaitynoProjektasBackEnd.RequestModels;
using SaitynoProjektasBackEnd.ResponseModels;

namespace SaitynoProjektasBackEnd.Services
{
    public interface ISongsService
    {
        SongResponseModel GetSongById(int id);
        IEnumerable<SongResponseModel> GetSongs();
        Task<Song> AddSong(AddSongRequestModel songRequestModel, string authId);
        void EditSong(int id, EditSongRequestModel song, string authId);
        Task<bool> DeleteSong(int id, string authId);
        IEnumerable<SongResponseModel> SearchSongs(string query);
        IEnumerable<SongResponseModel> GetSongsByGenre(string genreName);
        IEnumerable<SongResponseModel> GetUserSongs(string userName);
        IEnumerable<SongResponseModel> GetPopularSongs();
    }
}
