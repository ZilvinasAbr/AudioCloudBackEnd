using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SaitynoProjektasBackEnd.Data;
using SaitynoProjektasBackEnd.Models;
using SaitynoProjektasBackEnd.RequestModels;
using SaitynoProjektasBackEnd.ResponseModels;

namespace SaitynoProjektasBackEnd.Services
{
    public class PlaylistsService : IPlaylistsService
    {
        private readonly ApplicationDbContext _context;

        public PlaylistsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public string[] GetPlaylists(string authId, out IEnumerable<PlaylistResponseModel> playlistsResult)
        {
            playlistsResult = null;
            var user = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);

            if (user == null)
                return new[] { "User is not found" };

            var playlists = _context.Playlists
                .Include(p => p.User)
                .Include(p => p.PlaylistSongs)
                    .ThenInclude(ps => ps.Song)
                        .ThenInclude(s => s.User)
                .Include(p => p.PlaylistSongs)
                    .ThenInclude(ps => ps.Song)
                        .ThenInclude(s => s.Genre)
                .Include(p => p.PlaylistSongs)
                    .ThenInclude(ps => ps.Song)
                        .ThenInclude(s => s.Likes)
                .Include(p => p.Likes)
                .Where(p => p.IsPublic || p.User.AuthId == authId)
                .ToList();

            playlistsResult = playlists.Select(Mappers.PlaylistToPlaylistResponseModel)
                .ToList();

            return null;
        }

        public string[] GetPlaylistById(int id, string authId, out PlaylistResponseModel playlistResult)
        {
            playlistResult = null;
            var user = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);

            if (user == null)
                return new[] { "User is not found" };

            var playlist = _context.Playlists
                .Include(p => p.User)
                .Include(p => p.PlaylistSongs)
                    .ThenInclude(ps => ps.Song)
                        .ThenInclude(s => s.User)
                .Include(p => p.PlaylistSongs)
                    .ThenInclude(ps => ps.Song)
                        .ThenInclude(s => s.Genre)
                .Include(p => p.PlaylistSongs)
                    .ThenInclude(ps => ps.Song)
                        .ThenInclude(s => s.Likes)
                .Include(p => p.Likes)
                .SingleOrDefault(p => p.Id == id && (p.IsPublic || p.User.AuthId == authId));

            if (playlist == null)
                return new[] {"Playlist is not found"};

            playlistResult = Mappers.PlaylistToPlaylistResponseModel(playlist);

            return null;
        }

        public string[] AddPlaylist(AddPlaylistRequestModel playlistRequestModel, string authId)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);

            if (user == null)
                return new[] {"User is not found"};

            var playlist = new Playlist
            {
                Name = playlistRequestModel.Name,
                Description = playlistRequestModel.Description,
                IsPublic = playlistRequestModel.IsPublic,
                User = user
            };

            _context.Playlists.Add(playlist);
            _context.SaveChanges();

            return null;
        }

        public string[] EditPlaylist(int id, EditPlaylistRequestModel playlistRequestModel, string authId)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);

            if (user == null)
                return new[] {"User is not found"};

            var playlist = _context.Playlists
                .Include(p => p.User)
                .SingleOrDefault(p => p.Id == id && p.User.AuthId == authId);

            if (playlist == null)
            {
                return new[] {"Playlist is not found or you have no permission to edit this playlist"};
            }

            if (!string.IsNullOrEmpty(playlistRequestModel.Name))
            {
                playlist.Name = playlistRequestModel.Name;
            }

            if (!string.IsNullOrEmpty(playlistRequestModel.Description))
            {
                playlist.Description = playlistRequestModel.Description;
            }

            if (playlistRequestModel.IsPublic.HasValue)
            {
                playlist.IsPublic = playlistRequestModel.IsPublic.Value;
            }

            _context.SaveChanges();

            return null;
        }

        public string[] DeletePlaylist(int id, string authId)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);

            if (user == null)
                return new[] {"User is not found"};

            var playlist = _context.Playlists
                .Include(p => p.User)
                .SingleOrDefault(p => p.Id == id && p.User.AuthId == authId);

            if (playlist == null)
            {
                return new[] {"Playlist is not found or you have no permission to delete this playlist"};
            }

            var playlistSongs = _context.PlaylistSongs.Where(ps => ps.PlaylistId == id);
            _context.PlaylistSongs.RemoveRange(playlistSongs);

            var likes = _context.Likes.Where(l => l.PlaylistId == id);
            _context.Likes.RemoveRange(likes);

            _context.Playlists.Remove(playlist);

            _context.SaveChanges();

            return null;
        }

        public string[] GetUserPlaylists(string userNameOfPlaylists, string authId, out IEnumerable<PlaylistResponseModel> playlistsResult)
        {
            playlistsResult = null;

            var user = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);

            if (user == null)
                return new[] {"User is not found"};

            var returnPrivatePlaylists = userNameOfPlaylists == user.UserName;

            var userOfPlaylists = _context.Users
                .SingleOrDefault(u => u.UserName == userNameOfPlaylists);

            if (userOfPlaylists == null)
                return new[] { "User of playlists is not found" };

            var playlists = _context.Playlists
                .Include(p => p.User)
                .Include(p => p.PlaylistSongs)
                    .ThenInclude(ps => ps.Song)
                        .ThenInclude(s => s.User)
                .Include(p => p.PlaylistSongs)
                    .ThenInclude(ps => ps.Song)
                        .ThenInclude(s => s.Genre)
                .Include(p => p.PlaylistSongs)
                    .ThenInclude(ps => ps.Song)
                        .ThenInclude(s => s.Likes)
                .Include(p => p.Likes)
                .ToList();

            if (!returnPrivatePlaylists) {
                playlists = playlists.Where(p => p.IsPublic).ToList();
            }

            var playlistResponseModels = playlists.Select(p => new PlaylistResponseModel
            {
                Name = p.Name,
                Description = p.Description,
                IsPublic = p.IsPublic,
                UserName = p.User.UserName,
                Likes = p.Likes.Count,
                Songs = p.PlaylistSongs.Select(Mappers.PlaylistSongToSongResponseModel)
            })
            .ToList();

            playlistsResult = playlistResponseModels;

            return null;
        }

        public string[] AddSong(int playlistId, int songId, string authId)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);

            if (user == null)
                return new[] {"User is not found"};

            var playlist = _context.Playlists
                .Include(p => p.User)
                .SingleOrDefault(p => p.Id == playlistId);
            var song = _context.Songs
                .SingleOrDefault(s => s.Id == songId);

            if (playlist == null)
                return new[] {"Playlist is not found"};
            if (playlist.User.AuthId != authId)
                return new[] {"You are not the owner of this playlist"};
            if (song == null)
                return new[] {"Song is not found"};

            var playlistSongs = _context.PlaylistSongs
                .Where(ps => ps.PlaylistId == playlistId);

            var isAlreadyInPlaylist = playlistSongs
                .Any(ps => ps.SongId == songId);

            if (isAlreadyInPlaylist)
                return new[] {"Song is already in the playlist"};

            var lastPlaylistSong = playlistSongs
                .OrderByDescending(ps => ps.Number)
                .FirstOrDefault();

            var playlistSong = new PlaylistSong
            {
                Playlist = playlist,
                Song = song,
                Number = lastPlaylistSong?.Number + 1 ?? 1
            };

            _context.PlaylistSongs.Add(playlistSong);
            _context.SaveChanges();

            return null;
        }

        public string[] RemoveSong(int playlistId, int songId, string authId)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);

            if (user == null)
                return new[] { "User is not found" };

            var playlist = _context.Playlists
                .Include(p => p.User)
                .SingleOrDefault(p => p.Id == playlistId);
            var song = _context.Songs
                .SingleOrDefault(s => s.Id == songId);

            if (playlist == null)
                return new[] { "Playlist is not found" };
            if (playlist.User.AuthId != authId)
                return new[] { "You are not the owner of this playlist" };
            if (song == null)
                return new[] { "Song is not found" };

            var playlistSongToRemove = _context.PlaylistSongs
                .Where(ps => ps.PlaylistId == playlistId)
                .SingleOrDefault(ps => ps.SongId == songId);

            if (playlistSongToRemove == null)
                return new[] {"Song is not in the playlist"};

            _context.PlaylistSongs.Remove(playlistSongToRemove);
            _context.SaveChanges();

            var playlistSongs = _context.PlaylistSongs
                .Where(ps => ps.PlaylistId == playlistId)
                .OrderBy(ps => ps.Number)
                .ToList();

            for (var i = 0; i < playlistSongs.Count; i++)
            {
                playlistSongs[i].Number = i + 1;
            }

            _context.SaveChanges();

            return null;
        }
    }
}
