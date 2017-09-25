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
        string[] GetUserPlaylists(string userNameOfPlaylists, string userName, out IEnumerable<PlaylistResponseModel> playlists);
        string[] AddSong(int playlistId, int songId, string userName);
        string[] RemoveSong(int playlistId, int songId, string userName);
    }
}
