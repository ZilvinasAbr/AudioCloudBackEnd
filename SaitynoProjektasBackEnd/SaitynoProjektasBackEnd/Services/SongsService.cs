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
                .ToList();

            var songResponseModels = songs.Select(song => new SongResponseModel
            {
                Description = song.Description,
                Duration = song.Duration,
                Genre = song.Genre.Name,
                Likes = song.Likes.Count,
                PictureUrl = song.PictureUrl,
                Plays = song.Plays,
                Title = song.Title,
                UploadDate = song.UploadDate,
                UploaderName = song.User.UserName
            });

            return songResponseModels;
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

            var songResponseModel = new SongResponseModel
            {
                Description = song.Description,
                Duration = song.Duration,
                Genre = song.Genre.Name,
                Likes = song.Likes.Count,
                PictureUrl = song.PictureUrl,
                Plays = song.Plays,
                Title = song.Title,
                UploadDate = song.UploadDate,
                UploaderName = song.User.UserName
            };

            return songResponseModel;
        }

        public string[] AddSong(AddSongRequestModel songRequestModel)
        {
            var genre = _context.Genres
                .SingleOrDefault(g => g.Name == songRequestModel.Genre);

            if (genre == null)
            {
                return new[] {"Genre is not found"};
            }

            var song = new Song
            {
                Title = songRequestModel.Title,
                Description = songRequestModel.Description,
                UploadDate = DateTime.Now,
                PictureUrl = songRequestModel.PictureUrl,
                Duration = 0,
                Plays = 0,
                Genre = genre
            };

            _context.Songs.Add(song);
            var result = _context.SaveChanges();

            if (result == 0)
            {
                return new[] {"Could not add song"};
            }

            return null;
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
    }
}
