using System.Linq;
using Microsoft.EntityFrameworkCore;
using SaitynoProjektasBackEnd.Data;
using SaitynoProjektasBackEnd.Models;

namespace SaitynoProjektasBackEnd.Services
{
    public class LikesService : ILikesService
    {
        private readonly ApplicationDbContext _context;

        public LikesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public string[] LikeASong(int songId, string userName)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.UserName == userName);

            if (user == null)
                return new[] {"User is not found"};

            var song = _context.Songs
                .SingleOrDefault(s => s.Id == songId);

            if (song == null)
                return new[] {"Song is not found"};

            var songLikes = _context.Likes
                .Include(l => l.User)
                .Include(l => l.Song)
                .SingleOrDefault(l => l.SongId == songId && l.User.UserName == userName);

            if (songLikes != null)
            {
                return new[] {"Song is already liked"};
            }

            var like = new Like
            {
                Song = song,
                User = user
            };

            _context.Likes.Add(like);
            _context.SaveChanges();

            return null;
        }

        public string[] DislikeASong(int songId, string userName)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.UserName == userName);

            if (user == null)
                return new[] { "User is not found" };

            var song = _context.Songs
                .SingleOrDefault(s => s.Id == songId);

            if (song == null)
                return new[] { "Song is not found" };

            var songLike = _context.Likes
                .Include(l => l.User)
                .Include(l => l.Song)
                .SingleOrDefault(l => l.SongId == songId && l.User.UserName == userName);

            if (songLike == null)
            {
                return new[] { "Song is not liked" };
            }

            _context.Likes.Remove(songLike);
            _context.SaveChanges();

            return null;
        }

        public string[] LikeAPlaylist(int playlistId, string userName)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.UserName == userName);

            if (user == null)
                return new[] { "User is not found" };

            var playlist = _context.Playlists
                .SingleOrDefault(p => p.Id == playlistId);

            if (playlist == null)
                return new[] {"Playlist is not found"};

            var playlistLikes = _context.Likes
                .Include(l => l.User)
                .Include(l => l.Playlist)
                .SingleOrDefault(l => l.PlaylistId == playlistId && l.User.UserName == userName);

            if (playlistLikes != null)
            {
                return new[] { "Playlist is already liked" };
            }

            var like = new Like
            {
                Playlist = playlist,
                User = user
            };

            _context.Likes.Add(like);
            _context.SaveChanges();

            return null;
        }

        public string[] DislikeAPlaylist(int playlistId, string userName)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.UserName == userName);

            if (user == null)
                return new[] { "User is not found" };

            var playlist = _context.Playlists
                .SingleOrDefault(p => p.Id == playlistId);

            if (playlist == null)
                return new[] { "Playlist is not found" };

            var playlistLike = _context.Likes
                .Include(l => l.User)
                .Include(l => l.Playlist)
                .SingleOrDefault(l => l.PlaylistId == playlistId && l.User.UserName == userName);

            if (playlistLike == null)
                return new[] {"Playlist is not liked"};

            _context.Likes.Remove(playlistLike);
            _context.SaveChanges();

            return null;
        }
    }
}
