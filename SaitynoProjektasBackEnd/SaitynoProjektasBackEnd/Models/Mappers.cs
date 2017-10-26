using System.Linq;
using SaitynoProjektasBackEnd.ResponseModels;

namespace SaitynoProjektasBackEnd.Models
{
    public class Mappers
    {
        public static SongResponseModel SongToSongResponseModel(Song song)
        {
            return new SongResponseModel
            {
                Id = song.Id,
                Description = song.Description,
                Duration = song.Duration,
                Genre = song.Genre.Name,
                Likes = song.Likes.Count,
                PictureUrl = song.PictureUrl,
                Plays = song.Plays,
                Title = song.Title,
                UploadDate = song.UploadDate,
                User = UserToUserResponseModel(song.User),
                FilePath = song.FilePath
            };
        }

        public static SongResponseModel PlaylistSongToSongResponseModel(PlaylistSong playlistSong) =>
            new SongResponseModel
            {
                Id = playlistSong.Song.Id,
                TrackNumber = playlistSong.Number,
                Title = playlistSong.Song.Title,
                Description = playlistSong.Song.Description,
                UploadDate = playlistSong.Song.UploadDate,
                PictureUrl = playlistSong.Song.PictureUrl,
                Duration = playlistSong.Song.Duration,
                Plays = playlistSong.Song.Plays,
                User = UserToUserResponseModel(playlistSong.Song.User),
                Genre = playlistSong.Song.Genre.Name,
                Likes = playlistSong.Song.Likes.Count,
                FilePath = playlistSong.Song.FilePath
            };

        public static UserResponseModel UserToUserResponseModel(User user) =>
            new UserResponseModel
            {
                Id = user.Id,
                Name = user.UserName,
                Location = user.Location,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Description = user.Description
            };

        public static PlaylistResponseModel PlaylistToPlaylistResponseModel(Playlist playlist) =>
            new PlaylistResponseModel
            {
                Id = playlist.Id,
                Name = playlist.Name,
                Description = playlist.Description,
                IsPublic = playlist.IsPublic,
                UserName = playlist.User.UserName,
                Songs = playlist.PlaylistSongs.Select(PlaylistSongToSongResponseModel).ToList()
            };

        public static EventResponseModel EventToEventResponseModel(Event e) =>
            new EventResponseModel
            {
                Id = e.Id,
                CreatedOn = e.CreatedOn,
                EventType = e.EventType,
                User = UserToUserResponseModel(e.User),
                Song = SongToSongResponseModel(e.Song)
            };
    }
}
