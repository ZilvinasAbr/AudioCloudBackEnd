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

        public bool AddSong(AddSongRequestModel songRequestModel)
        {
            var genre = _context.Genres.SingleOrDefault(g => g.Name == songRequestModel.Genre);

            if (genre == null)
            {
                return false;
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
                return false;
            }

            return true;
        }
    }
}
