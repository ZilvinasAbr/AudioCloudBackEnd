using SaitynoProjektasBackEnd.Models;

namespace SaitynoProjektasBackEnd.Services
{
    public interface ILikesService
    {
        Like LikeASong(int songId, string authId);
        Like LikeAPlaylist(int playlistId, string authId);
        void DislikeASong(int songId, string authId);
        void DislikeAPlaylist(int playlistId, string authId);
    }
}
