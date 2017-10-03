using System;
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

        public Like LikeASong(int songId, string authId)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);

            if (user == null)
                throw new Exception("User is not found");

            var song = _context.Songs
                .SingleOrDefault(s => s.Id == songId);

            if (song == null)
                throw new Exception("Song is not found");

            var songLikes = _context.Likes
                .Include(l => l.User)
                .Include(l => l.Song)
                .SingleOrDefault(l => l.SongId == songId && l.User.AuthId == authId);

            if (songLikes != null)
            {
                throw new Exception("Song is already liked");
            }

            var like = new Like
            {
                Song = song,
                User = user
            };

            _context.Likes.Add(like);
            _context.SaveChanges();
            
            return like;
        }

        public void DislikeASong(int songId, string authId)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);

            if (user == null)
                throw new Exception("User is not found");

            var song = _context.Songs
                .SingleOrDefault(s => s.Id == songId);

            if (song == null)
                throw new Exception("Song is not found");

            var songLike = _context.Likes
                .Include(l => l.User)
                .Include(l => l.Song)
                .SingleOrDefault(l => l.SongId == songId && l.User.AuthId == authId);

            if (songLike == null)
                throw new Exception("Song is not liked");

            _context.Likes.Remove(songLike);
            _context.SaveChanges();
        }

        public Like LikeAPlaylist(int playlistId, string authId)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);

            if (user == null)
                throw new Exception("User is not found");

            var playlist = _context.Playlists
                .SingleOrDefault(p => p.Id == playlistId);

            if (playlist == null)
                throw new Exception("Playlist is not found");

            var playlistLikes = _context.Likes
                .Include(l => l.User)
                .Include(l => l.Playlist)
                .SingleOrDefault(l => l.PlaylistId == playlistId && l.User.AuthId == authId);

            if (playlistLikes != null)
                throw new Exception("Playlist is already liked");

            var like = new Like
            {
                Playlist = playlist,
                User = user
            };

            _context.Likes.Add(like);
            _context.SaveChanges();
            
            return like;
        }

        public void DislikeAPlaylist(int playlistId, string authId)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);

            if (user == null)
                throw new Exception("User is not found");

            var playlist = _context.Playlists
                .SingleOrDefault(p => p.Id == playlistId);

            if (playlist == null)
                throw new Exception("Playlist is not found");

            var playlistLike = _context.Likes
                .Include(l => l.User)
                .Include(l => l.Playlist)
                .SingleOrDefault(l => l.PlaylistId == playlistId && l.User.AuthId == authId);

            if (playlistLike == null)
                throw new Exception("Playlist is not liked");

            _context.Likes.Remove(playlistLike);
            _context.SaveChanges();
        }
    }
}
