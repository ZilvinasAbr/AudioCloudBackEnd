namespace SaitynoProjektasBackEnd.Services
{
    public interface ILikesService
    {
        string[] LikeASong(int songId, string userName);
        string[] LikeAPlaylist(int playlistId, string userName);
        string[] DislikeASong(int songId, string userName);
        string[] DislikeAPlaylist(int playlistId, string userName);
    }
}
