namespace SaitynoProjektasBackEnd.Services
{
    public interface ILikesService
    {
        string[] LikeASong(int songId, string authId);
        string[] LikeAPlaylist(int playlistId, string authId);
        string[] DislikeASong(int songId, string authId);
        string[] DislikeAPlaylist(int playlistId, string authId);
    }
}
