using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SaitynoProjektasBackEnd.Data;
using SaitynoProjektasBackEnd.Models;
using SaitynoProjektasBackEnd.RequestModels;
using SaitynoProjektasBackEnd.ResponseModels;
using SaitynoProjektasBackEnd.Services.Interfaces;

namespace SaitynoProjektasBackEnd.Services.Classes
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
                .Select(Mappers.SongToSongResponseModel);

            return songs;
        }

        public IEnumerable<SongResponseModel> GetSongsByGenre(string genreName)
        {
            var genre = _context.Genres
                .SingleOrDefault(g => g.Name == genreName);

            if (genre == null)
                throw new Exception("Genre is not found");

            var songs = _context.Songs
                .Include(s => s.User)
                .Include(s => s.Genre)
                .Include(s => s.Likes)
                .Where(s => s.Genre.Name == genreName)
                .ToList()
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

        public async Task<Song> AddSong(AddSongRequestModel songRequestModel, string authId)
        {
            var genre = _context.Genres
                .SingleOrDefault(g => g.Name == songRequestModel.Genre);
            var user = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);
            var isFilePathUsed = _context.Songs
                .Any(s => s.FilePath == songRequestModel.FilePath);

            if (genre == null)
                throw new Exception("Genre is not found");
            if (user == null)
                throw new Exception("User is not found");
            if (isFilePathUsed)
                throw new Exception("Invalid file path");

            var fileIsFound = await _dropBoxService.DoesFileExistAsync(songRequestModel.FilePath);

            if (!fileIsFound)
                throw new Exception("File specified is not found in the file storage");

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
            _context.SaveChanges();
            
            return song;
        }

        public void EditSong(int id, EditSongRequestModel songRequestModel, string authId)
        {
            var song = _context.Songs
                .Include(s => s.User)
                .SingleOrDefault(s => s.Id == id);
            var user = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);

            if (song == null)
                throw new Exception("Song is not found");
            if (user == null)
                throw new Exception("User is not found");
            if (song.User.AuthId != authId)
                throw new Exception("User is not the owner of the song");

            if (!string.IsNullOrEmpty(songRequestModel.Title))
            {
                song.Title = songRequestModel.Title;
            }

            if (!string.IsNullOrEmpty(songRequestModel.Description))
            {
                song.Description = songRequestModel.Description;
            }

            _context.SaveChanges();
        }

        public async Task<bool> DeleteSong(int id, string authId)
        {
            var song = _context.Songs
                .Include(s => s.User)
                .SingleOrDefault(s => s.Id == id);
            var user = _context.Users
                .SingleOrDefault(u => u.AuthId == authId);

            if (song == null)
                throw new Exception("Song is not found");
            if (user == null)
                throw new Exception("User is not found");
            if (song.User.AuthId != authId)
                throw new Exception("User is not the owner of the song");

            var playlistSongs = _context.PlaylistSongs.Where(ps => ps.SongId == id);
            _context.PlaylistSongs.RemoveRange(playlistSongs);

            var likes = _context.Likes.Where(l => l.SongId == id);
            _context.Likes.RemoveRange(likes);

            _context.Songs.Remove(song);

            _context.SaveChanges();

            // TODO: Commented out for development purposes
            // var result = await _dropBoxService.DeleteFileAsync(song.FilePath);
            // 
            // return result;
            await new Task(() => {
                
            });
            return true;
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

        public IEnumerable<SongResponseModel> GetUserSongs(string userName)
        {
            var user = _context.Users
                .SingleOrDefault(u => u.UserName == userName);

            if (user == null)
                throw new Exception("User is not found");

            var userSongs = _context.Songs
                .Include(s => s.User)
                .Include(s => s.Genre)
                .Include(s => s.Likes)
                .Where(s => s.User.AuthId == user.AuthId)
                .ToList()
                .Select(Mappers.SongToSongResponseModel);

            return userSongs;
        }

        public IEnumerable<SongResponseModel> GetPopularSongs()
        {
            var songs = _context.Songs
                .Include(s => s.User)
                .Include(s => s.Genre)
                .Include(s => s.Likes)
                .OrderByDescending(s => s.Plays)
                .Select(Mappers.SongToSongResponseModel)
                .Take(10);

            return songs;
        }
    }
}
