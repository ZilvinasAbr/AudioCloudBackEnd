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

        public IEnumerable<PlaylistResponseModel> GetPlaylists()
        {
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

            var playlistResponseModels = playlists.Select(playlist => new PlaylistResponseModel
            {
                Name = playlist.Name,
                Description = playlist.Description,
                IsPublic = playlist.IsPublic,
                UserName = playlist.User.UserName,
                Likes = playlist.Likes.Count,
                Songs = playlist.PlaylistSongs.Select(ps => new SongResponseModel
                {
                    TrackNumber = ps.Number,
                    Title = ps.Song.Title,
                    Description = ps.Song.Description,
                    UploadDate = ps.Song.UploadDate,
                    PictureUrl = ps.Song.PictureUrl,
                    Duration = ps.Song.Duration,
                    Plays = ps.Song.Plays,
                    UploaderName = ps.Song.User.UserName,
                    Genre = ps.Song.Genre.Name,
                    Likes = ps.Song.Likes.Count
                })
            })
            .ToList();

            return playlistResponseModels;
        }

        public PlaylistResponseModel GetPlaylistById(int id)
        {
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
                .SingleOrDefault(p => p.Id == id);

            if (playlist == null)
            {
                return null;
            }

            var playlistResponseModel = new PlaylistResponseModel
            {
                Name = playlist.Name,
                Description = playlist.Description,
                IsPublic = playlist.IsPublic,
                UserName = playlist.User.UserName,
                Likes = playlist.Likes.Count,
                Songs = playlist.PlaylistSongs.Select(ps => new SongResponseModel
                {
                    TrackNumber = ps.Number,
                    Title = ps.Song.Title,
                    Description = ps.Song.Description,
                    UploadDate = ps.Song.UploadDate,
                    PictureUrl = ps.Song.PictureUrl,
                    Duration = ps.Song.Duration,
                    Plays = ps.Song.Plays,
                    UploaderName = ps.Song.User.UserName,
                    Genre = ps.Song.Genre.Name,
                    Likes = ps.Song.Likes.Count
                })
            };

            return playlistResponseModel;
        }

        public string[] AddPlaylist(AddPlaylistRequestModel playlistRequestModel)
        {
            var playlist = new Playlist
            {
                Name = playlistRequestModel.Name,
                Description = playlistRequestModel.Description,
                IsPublic = playlistRequestModel.IsPublic
            };

            _context.Playlists.Add(playlist);
            _context.SaveChanges();

            return null;
        }

        public string[] EditPlaylist(int id, EditPlaylistRequestModel playlistRequestModel)
        {
            var playlist = _context.Playlists
                .SingleOrDefault(p => p.Id == id);

            if (playlist == null)
            {
                return new[] {"Playlist is not found"};
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

        public string[] DeletePlaylist(int id)
        {
            var playlist = _context.Playlists
                .SingleOrDefault(p => p.Id == id);

            if (playlist == null)
            {
                return new[] {"Playlist is not found"};
            }

            var playlistSongs = _context.PlaylistSongs.Where(ps => ps.PlaylistId == id);
            _context.PlaylistSongs.RemoveRange(playlistSongs);

            var likes = _context.Likes.Where(l => l.PlaylistId == id);
            _context.Likes.RemoveRange(likes);

            _context.Playlists.Remove(playlist);

            _context.SaveChanges();

            return null;
        }
    }
}
