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
    public class SongsService : ISongsService
    {
        private readonly ApplicationDbContext _context;

        public SongsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<SongResponseModel> GetSongs()
        {
            var songs = _context.Songs
                .Include(s => s.User)
                .Include(s => s.Genre)
                .Include(s => s.Likes)
                .Include(s => s.Comments)
                    .ThenInclude(c => c.User)
                .Select(Mappers.SongToSongResponseModel);

            return songs;
        }

        public SongResponseModel GetSongById(int id)
        {
            var song = _context.Songs
                .Include(s => s.User)
                .Include(s => s.Genre)
                .Include(s => s.Likes)
                .SingleOrDefault(s => s.Id == id);

            if (song == null)
            {
                return null;
            }

            var songResponseModel = Mappers.SongToSongResponseModel(song);

            return songResponseModel;
        }

        public string[] AddSong(AddSongRequestModel songRequestModel, string userName)
        {
            var genre = _context.Genres
                .SingleOrDefault(g => g.Name == songRequestModel.Genre);
            var user = _context.Users
                .SingleOrDefault(u => u.UserName == userName);

            if (genre == null)
                return new[] {"Genre is not found"};
            if (user == null)
                return new[] {"User is not found"};

            var song = new Song
            {
                Title = songRequestModel.Title,
                Description = songRequestModel.Description,
                UploadDate = DateTime.Now,
                PictureUrl = songRequestModel.PictureUrl,
                Duration = 0,
                Plays = 0,
                Genre = genre,
                User = user
            };

            _context.Songs.Add(song);
            var result = _context.SaveChanges();

            return result == 0 ? new[] {"Could not add song"} : null;
        }

        public string[] EditSong(int id, EditSongRequestModel songRequestModel)
        {
            var song = _context.Songs
                .SingleOrDefault(s => s.Id == id);

            if (song == null)
            {
                return new[] {"Song is not found"};
            }

            if (!string.IsNullOrEmpty(songRequestModel.Title))
            {
                song.Title = songRequestModel.Title;
            }

            if (!string.IsNullOrEmpty(songRequestModel.Description))
            {
                song.Description = songRequestModel.Description;
            }

            _context.SaveChanges();

            return null;
        }

        public string[] DeleteSong(int id)
        {
            var song = _context.Songs
                .SingleOrDefault(s => s.Id == id);

            if (song == null)
            {
                return new[] {"Song is not found"};
            }

            var playlistSongs = _context.PlaylistSongs.Where(ps => ps.SongId == id);
            _context.PlaylistSongs.RemoveRange(playlistSongs);

            var likes = _context.Likes.Where(l => l.SongId == id);
            _context.Likes.RemoveRange(likes);

            _context.Songs.Remove(song);

            _context.SaveChanges();

            return null;
        }

        public IEnumerable<SongResponseModel> SearchSongs(string query) =>
            _context.Songs
                .Include(s => s.User)
                .Include(s => s.Genre)
                .Include(s => s.Likes)
                .Where(song => IsFoundByQuery(song, query.ToUpperInvariant()))
                .AsEnumerable()
                .Select(Mappers.SongToSongResponseModel);

        private static bool IsFoundByQuery(Song song, string query) =>
            song.Title.ToUpperInvariant().Contains(query) || song.Description.ToUpperInvariant().Contains(query);
    }
}
