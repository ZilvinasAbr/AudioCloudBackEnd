using System.Collections.Generic;
using SaitynoProjektasBackEnd.RequestModels;
using SaitynoProjektasBackEnd.ResponseModels;

namespace SaitynoProjektasBackEnd.Services
{
    public interface IPlaylistsService
    {
        IEnumerable<PlaylistResponseModel> GetPlaylists();
        PlaylistResponseModel GetPlaylistById(int id);
        bool AddPlaylist(AddPlaylistRequestModel playlist);
    }
}
