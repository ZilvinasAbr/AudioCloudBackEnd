using System.Collections.Generic;
using SaitynoProjektasBackEnd.RequestModels;
using SaitynoProjektasBackEnd.ResponseModels;

namespace SaitynoProjektasBackEnd.Services
{
    public interface IPlaylistsService
    {
        string[] GetPlaylists(string authId, out IEnumerable<PlaylistResponseModel> playlistsResult);
        string[] GetPlaylistById(int id, string authId, out PlaylistResponseModel playlist);
        string[] AddPlaylist(AddPlaylistRequestModel playlist, string authId);
        string[] EditPlaylist(int id, EditPlaylistRequestModel playlist, string authId);
        string[] DeletePlaylist(int id, string authId);
        string[] GetUserPlaylists(string userNameOfPlaylists, string authId, out IEnumerable<PlaylistResponseModel> playlists);
        string[] AddSong(int playlistId, int songId, string authId);
        string[] RemoveSong(int playlistId, int songId, string authId);
    }
}
