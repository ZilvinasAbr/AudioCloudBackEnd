using System.Collections.Generic;
using SaitynoProjektasBackEnd.RequestModels;
using SaitynoProjektasBackEnd.ResponseModels;

namespace SaitynoProjektasBackEnd.Services
{
    public interface IPlaylistsService
    {
        string[] GetPlaylists(string userName, out IEnumerable<PlaylistResponseModel> playlistsResult);
        string[] GetPlaylistById(int id, string userName, out PlaylistResponseModel playlist);
        string[] AddPlaylist(AddPlaylistRequestModel playlist);
        string[] EditPlaylist(int id, EditPlaylistRequestModel playlist);
        string[] DeletePlaylist(int id);
        string[] GetUserPlaylists(string userNameOfPlaylists, string userName, out IEnumerable<PlaylistResponseModel> playlists);
        string[] AddSong(int playlistId, int songId, string userName);
        string[] RemoveSong(int playlistId, int songId, string userName);
    }
}
