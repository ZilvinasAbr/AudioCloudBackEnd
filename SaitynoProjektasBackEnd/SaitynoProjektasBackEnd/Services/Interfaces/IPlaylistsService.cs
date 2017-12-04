using System.Collections.Generic;
using SaitynoProjektasBackEnd.Models;
using SaitynoProjektasBackEnd.RequestModels;
using SaitynoProjektasBackEnd.ResponseModels;

namespace SaitynoProjektasBackEnd.Services.Interfaces
{
    public interface IPlaylistsService
    {
        IEnumerable<PlaylistResponseModel> GetPlaylists(string authId);
        PlaylistResponseModel GetPlaylistById(int id, string authId);
        Playlist AddPlaylist(AddPlaylistRequestModel playlist, string authId);
        void EditPlaylist(int id, EditPlaylistRequestModel playlist, string authId);
        void DeletePlaylist(int id, string authId);
        IEnumerable<PlaylistResponseModel> GetUserPlaylists(string userNameOfPlaylists, string authId, int? amount);
        PlaylistSong AddSong(int playlistId, int songId, string authId);
        void RemoveSong(int playlistId, int songId, string authId);
        PlaylistResponseModel GetUserLikedPlaylist(string authId, int? amount);
    }
}
