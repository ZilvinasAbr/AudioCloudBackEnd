using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IDropBoxService _dropBoxService;

        public SongsService(ApplicationDbContext context, IDropBoxService dropBoxService)
        {
            _context = context;
            _dropBoxService = dropBoxService;
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

        public string[] GetSongsByGenre(string genreName, out IEnumerable<SongResponseModel> songsResult)
        {
            songsResult = null;
            var genre = _context.Genres
                .SingleOrDefault(g => g.Name == genreName);

            if (genre == null)
                return new[] { "Genre is not found" };

            songsResult = _context.Songs
                .Include(s => s.User)
                .Include(s => s.Genre)
                .Include(s => s.Likes)
                .Include(s => s.Comments)
                    .ThenInclude(c => c.User)
                .Where(s => s.Genre.Name == genreName)
                .Select(Mappers.SongToSongResponseModel);

            return null;
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

        public async Task<string[]> AddSong(AddSongRequestModel songRequestModel, string userName)
        {
            var genre = _context.Genres
                .SingleOrDefault(g => g.Name == songRequestModel.Genre);
            var user = _context.Users
                .SingleOrDefault(u => u.UserName == userName);
            var isFilePathUsed = _context.Songs
                .Where(s => s.FilePath == songRequestModel.FilePath)
                .Any();

            if (genre == null)
                return new[] {"Genre is not found"};
            if (user == null)
                return new[] {"User is not found"};
            if (isFilePathUsed)
                return new[] {"Invalid file path"};

            var fileIsFound = await _dropBoxService.DoesFileExist(songRequestModel.FilePath);

            if (!fileIsFound)
                return new[] {"File specified is not found in the file storage"};

            var song = new Song
            {
                Title = songRequestModel.Title,
                Description = songRequestModel.Description,
                UploadDate = DateTime.Now,
                PictureUrl = songRequestModel.PictureUrl,
                Duration = 0,
                Plays = 0,
                Genre = genre,
                User = user,
                FilePath = songRequestModel.FilePath
            };

            _context.Songs.Add(song);
            var result = _context.SaveChanges();

            return result == 0 ? new[] {"Could not add song"} : null;
        }

        public string[] EditSong(int id, EditSongRequestModel songRequestModel, string userName)
        {
            var song = _context.Songs
                .Include(s => s.User)
                .SingleOrDefault(s => s.Id == id);
            var user = _context.Users
                .SingleOrDefault(u => u.UserName == userName);

            if (song == null)
                return new[] {"Song is not found"};
            if (user == null)
                return new[] {"User is not found"};
            if (song.User.UserName != userName)
                return new[] {"User is not the owner of the song"};

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

        public async Task<string[]> DeleteSong(int id, string userName)
        {
            var song = _context.Songs
                .Include(s => s.User)
                .SingleOrDefault(s => s.Id == id);
            var user = _context.Users
                .SingleOrDefault(u => u.UserName == userName);

            if (song == null)
                return new[] {"Song is not found"};
            if (user == null)
                return new[] {"User is not found"};
            if (song.User.UserName != userName)
                return new[] {"User is not the owner of the song"};

            var playlistSongs = _context.PlaylistSongs.Where(ps => ps.SongId == id);
            _context.PlaylistSongs.RemoveRange(playlistSongs);

            var likes = _context.Likes.Where(l => l.SongId == id);
            _context.Likes.RemoveRange(likes);

            _context.Songs.Remove(song);

            _context.SaveChanges();

            var result = await _dropBoxService.DeleteFile(song.FilePath);

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
