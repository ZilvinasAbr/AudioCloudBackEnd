using System.Linq;
using SaitynoProjektasBackEnd.ResponseModels;

namespace SaitynoProjektasBackEnd.Models
{
    public class Mappers
    {
        public static SongResponseModel SongToSongResponseModel(Song song) =>
            new SongResponseModel
            {
                Description = song.Description,
                Duration = song.Duration,
                Genre = song.Genre.Name,
                Likes = song.Likes.Count,
                PictureUrl = song.PictureUrl,
                Plays = song.Plays,
                Title = song.Title,
                UploadDate = song.UploadDate,
                UploaderName = song.User.UserName,
                Comments = song.Comments.Select(CommentToCommentResponseModel)
            };

        public static SongResponseModel PlaylistSongToSongResponseModel(PlaylistSong playlistSong) =>
            new SongResponseModel
            {
                TrackNumber = playlistSong.Number,
                Title = playlistSong.Song.Title,
                Description = playlistSong.Song.Description,
                UploadDate = playlistSong.Song.UploadDate,
                PictureUrl = playlistSong.Song.PictureUrl,
                Duration = playlistSong.Song.Duration,
                Plays = playlistSong.Song.Plays,
                UploaderName = playlistSong.Song.User.UserName,
                Genre = playlistSong.Song.Genre.Name,
                Likes = playlistSong.Song.Likes.Count
            };

        public static UserResponseModel UserToUserResponseModel(User user) =>
            new UserResponseModel
            {
                Name = user.UserName,
                Location = user.Location,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Description = user.Description
            };

        public static CommentResponseModel CommentToCommentResponseModel(Comment comment) =>
            new CommentResponseModel
            {
                Message = comment.Message,
                CreatedOn = comment.CreatedOn,
                UserName = comment.User.UserName
            };

        public static PlaylistResponseModel PlaylistToPlaylistResponseModel(Playlist playlist) =>
            new PlaylistResponseModel
            {
                Name = playlist.Name,
                Description = playlist.Description,
                IsPublic = playlist.IsPublic,
                UserName = playlist.User.UserName,
                Likes = playlist.Likes.Count,
                Songs = playlist.PlaylistSongs.Select(PlaylistSongToSongResponseModel)
            };
    }
}
