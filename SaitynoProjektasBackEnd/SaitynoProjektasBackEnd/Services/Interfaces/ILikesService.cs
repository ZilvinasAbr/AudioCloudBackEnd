using SaitynoProjektasBackEnd.Models;

namespace SaitynoProjektasBackEnd.Services.Interfaces
{
    public interface ILikesService
    {
        bool GetIsSongLiked(int songId, string authId);
        Like LikeASong(int songId, string authId);
        void DislikeASong(int songId, string authId);
    }
}
