using System.Collections.Generic;
using SaitynoProjektasBackEnd.RequestModels;
using SaitynoProjektasBackEnd.ResponseModels;

namespace SaitynoProjektasBackEnd.Services
{
    public interface IPlaylistsService
    {
        IEnumerable<PlaylistResponseModel> GetPlaylists();
        PlaylistResponseModel GetPlaylistById(int id);
        string[] AddPlaylist(AddPlaylistRequestModel playlist);
        string[] EditPlaylist(int id, EditPlaylistRequestModel playlist);
        string[] DeletePlaylist(int id);
    }
}
