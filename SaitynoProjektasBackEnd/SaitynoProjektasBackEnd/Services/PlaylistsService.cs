using System;
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

        public IEnumerable<PlaylistResponseModel> GetPlaylists(string authId)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);

            if (user == null)
                throw new Exception("User is not found");

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
                .AsEnumerable()
                .Select(Mappers.PlaylistToPlaylistResponseModel)
                .ToList();

            return playlists;
        }

        public PlaylistResponseModel GetPlaylistById(int id, string authId)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);

            if (user == null)
                throw new Exception("User is not found");

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
                throw new Exception("Playlist is not found");

            var playlistResponseModel = Mappers.PlaylistToPlaylistResponseModel(playlist);

            return playlistResponseModel;
        }

        public Playlist AddPlaylist(AddPlaylistRequestModel playlistRequestModel, string authId)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);

            if (user == null)
                throw new Exception("User is not found");

            var playlist = new Playlist
            {
                Name = playlistRequestModel.Name,
                Description = playlistRequestModel.Description,
                IsPublic = playlistRequestModel.IsPublic,
                User = user
            };

            _context.Playlists.Add(playlist);
            _context.SaveChanges();

            return playlist;
        }

        public void EditPlaylist(int id, EditPlaylistRequestModel playlistRequestModel, string authId)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);

            if (user == null)
                throw new Exception("User is not found");

            var playlist = _context.Playlists
                .Include(p => p.User)
                .SingleOrDefault(p => p.Id == id && p.User.AuthId == authId);

            if (playlist == null)
            {
                throw new Exception("Playlist is not found or you have no permission to edit this playlist");
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
        }

        public void DeletePlaylist(int id, string authId)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);

            if (user == null)
                throw new Exception("User is not found");

            var playlist = _context.Playlists
                .Include(p => p.User)
                .SingleOrDefault(p => p.Id == id && p.User.AuthId == authId);

            if (playlist == null)
                throw new Exception("Playlist is not found or you have no permission to delete this playlist");

            var playlistSongs = _context.PlaylistSongs.Where(ps => ps.PlaylistId == id);
            _context.PlaylistSongs.RemoveRange(playlistSongs);

            _context.Playlists.Remove(playlist);

            _context.SaveChanges();
        }

        public IEnumerable<PlaylistResponseModel> GetUserPlaylists(string userNameOfPlaylists, string authId)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);

            if (user == null)
                throw new Exception("User is not found");

            var returnPrivatePlaylists = userNameOfPlaylists == user.UserName;

            var userOfPlaylists = _context.Users
                .SingleOrDefault(u => u.UserName == userNameOfPlaylists);

            if (userOfPlaylists == null)
                throw new Exception("User of playlists is not found");

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
                .Where(p => p.User.AuthId == userOfPlaylists.AuthId)
                .ToList();

            if (!returnPrivatePlaylists)
            {
                playlists = playlists.Where(p => p.IsPublic).ToList();
            }

            var playlistResponseModels = playlists.Select(p => new PlaylistResponseModel
            {
                Name = p.Name,
                Description = p.Description,
                IsPublic = p.IsPublic,
                UserName = p.User.UserName,
                Songs = p.PlaylistSongs.Select(Mappers.PlaylistSongToSongResponseModel).ToList()
            })
            .ToList();

            return playlistResponseModels;
        }

        public PlaylistSong AddSong(int playlistId, int songId, string authId)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);

            if (user == null)
                throw new Exception("User is not found");

            var playlist = _context.Playlists
                .Include(p => p.User)
                .SingleOrDefault(p => p.Id == playlistId);
            var song = _context.Songs
                .SingleOrDefault(s => s.Id == songId);

            if (playlist == null)
                throw new Exception("Playlist is not found");
            if (playlist.User.AuthId != authId)
                throw new Exception("You are not the owner of this playlist");
            if (song == null)
                throw new Exception("Song is not found");

            var playlistSongs = _context.PlaylistSongs
                .Where(ps => ps.PlaylistId == playlistId);

            var isAlreadyInPlaylist = playlistSongs
                .Any(ps => ps.SongId == songId);

            if (isAlreadyInPlaylist)
                throw new Exception("Song is already in the playlist");

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

            return playlistSong;
        }

        public void RemoveSong(int playlistId, int songId, string authId)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);

            if (user == null)
                throw new Exception("User is not found");

            var playlist = _context.Playlists
                .Include(p => p.User)
                .SingleOrDefault(p => p.Id == playlistId);
            var song = _context.Songs
                .SingleOrDefault(s => s.Id == songId);

            if (playlist == null)
                throw new Exception("Playlist is not found");
            if (playlist.User.AuthId != authId)
                throw new Exception("You are not the owner of this playlist");
            if (song == null)
                throw new Exception("Song is not found");

            var playlistSongToRemove = _context.PlaylistSongs
                .Where(ps => ps.PlaylistId == playlistId)
                .SingleOrDefault(ps => ps.SongId == songId);

            if (playlistSongToRemove == null)
                throw new Exception("Song is not in the playlist");

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
        }

        public PlaylistResponseModel GetUserLikedPlaylist(string authId)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);

            if (user == null)
                throw new Exception("User is not found");

            var likedSongs = _context.Likes
                .Include(l => l.User)
                .Include(l => l.Song)
                    .ThenInclude(s => s.Genre)
                .Include(l => l.Song)
                    .ThenInclude(s => s.User)
                .Where(l => l.User.AuthId == authId && l.Song != null)
                .OrderByDescending(l => l.CreatedOn)
                .ToList()
                .Select(l => l.Song)
                .ToList();

            var playlist = new PlaylistResponseModel
            {
                Name = "Liked songs playlist",
                Description = "",
                IsPublic = false,
                UserName = user.UserName,
                Songs = likedSongs.Select(Mappers.SongToSongResponseModel).ToList()
            };

            var count = playlist.Songs.Count;
            for (var i = 0; i < count; i++)
            {
                playlist.Songs[i].TrackNumber = i + 1;
            }

            return playlist;
        }
    }
}
