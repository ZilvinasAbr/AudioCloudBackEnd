using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SaitynoProjektasBackEnd.Models;

namespace SaitynoProjektasBackEnd.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context, UserManager<User> userManager)
        {
            context.Database.EnsureCreated();

            if (context.Songs.Any())
            {
                return;
            }

            var users = AddUsers(userManager);
            var genres = AddGenres(context);
            var playlists = AddPlaylists(context, users);
            var songs = AddSongs(context, users, genres);
            var playlistSongs = AddPlaylistSongs(context, songs, playlists);
            var likes = AddLikes(context, users, songs, playlists);
        }

        private static PlaylistSong[] AddPlaylistSongs(ApplicationDbContext context, Song[] songs, Playlist[] playlists)
        {
            var playlistSongs = new[]
            {
                new PlaylistSong {Number = 1, Playlist = playlists[0], Song = songs[0]},
                new PlaylistSong {Number = 2, Playlist = playlists[0], Song = songs[1]},
                new PlaylistSong {Number = 3, Playlist = playlists[0], Song = songs[2]},
                new PlaylistSong {Number = 4, Playlist = playlists[0], Song = songs[3]},
                new PlaylistSong {Number = 5, Playlist = playlists[0], Song = songs[4]},
                new PlaylistSong {Number = 1, Playlist = playlists[1], Song = songs[0]},
                new PlaylistSong {Number = 2, Playlist = playlists[1], Song = songs[1]},
                new PlaylistSong {Number = 3, Playlist = playlists[1], Song = songs[2]},
                new PlaylistSong {Number = 4, Playlist = playlists[1], Song = songs[3]},
                new PlaylistSong {Number = 5, Playlist = playlists[1], Song = songs[4]},
                new PlaylistSong {Number = 1, Playlist = playlists[2], Song = songs[0]},
                new PlaylistSong {Number = 2, Playlist = playlists[2], Song = songs[1]},
                new PlaylistSong {Number = 3, Playlist = playlists[2], Song = songs[2]},
                new PlaylistSong {Number = 4, Playlist = playlists[2], Song = songs[3]},
                new PlaylistSong {Number = 5, Playlist = playlists[2], Song = songs[4]},
                new PlaylistSong {Number = 1, Playlist = playlists[3], Song = songs[0]},
                new PlaylistSong {Number = 2, Playlist = playlists[3], Song = songs[1]},
                new PlaylistSong {Number = 3, Playlist = playlists[3], Song = songs[2]},
                new PlaylistSong {Number = 4, Playlist = playlists[3], Song = songs[3]},
                new PlaylistSong {Number = 5, Playlist = playlists[3], Song = songs[4]},
                new PlaylistSong {Number = 1, Playlist = playlists[4], Song = songs[0]},
                new PlaylistSong {Number = 2, Playlist = playlists[4], Song = songs[1]},
                new PlaylistSong {Number = 3, Playlist = playlists[4], Song = songs[2]},
                new PlaylistSong {Number = 4, Playlist = playlists[4], Song = songs[3]},
                new PlaylistSong {Number = 5, Playlist = playlists[4], Song = songs[4]},
                new PlaylistSong {Number = 1, Playlist = playlists[5], Song = songs[0]},
                new PlaylistSong {Number = 2, Playlist = playlists[5], Song = songs[1]},
                new PlaylistSong {Number = 3, Playlist = playlists[5], Song = songs[2]},
                new PlaylistSong {Number = 4, Playlist = playlists[5], Song = songs[3]},
                new PlaylistSong {Number = 5, Playlist = playlists[5], Song = songs[4]},
                new PlaylistSong {Number = 1, Playlist = playlists[6], Song = songs[0]},
                new PlaylistSong {Number = 2, Playlist = playlists[6], Song = songs[1]},
                new PlaylistSong {Number = 3, Playlist = playlists[6], Song = songs[2]},
                new PlaylistSong {Number = 4, Playlist = playlists[6], Song = songs[3]},
                new PlaylistSong {Number = 5, Playlist = playlists[6], Song = songs[4]},
            };

            context.PlaylistSongs.AddRange(playlistSongs);
            context.SaveChanges();

            return playlistSongs;
        }

        private static Like[] AddLikes(ApplicationDbContext context, User[] users, Song[] songs, Playlist[] playlists)
        {
            var likes = new[]
            {
                new Like {User = users[0], Playlist = playlists[0]},
                new Like {User = users[0], Song = songs[2]},
                new Like {User = users[0], Song = songs[3]},
                new Like {User = users[0], Song = songs[4]},
                new Like {User = users[0], Playlist = playlists[2]}
            };

            context.Likes.AddRange(likes);
            context.SaveChanges();

            return likes;
        }

        private static Playlist[] AddPlaylists(ApplicationDbContext context, User[] users)
        {
            var playlists = new[]
            {
                new Playlist {User=users[0], Description = "Lorem ipsum", IsPublic = true, Name = "Playlist 1"},
                new Playlist {User=users[0], Description = "Lorem ipsum", IsPublic = false, Name = "Playlist 2"},
                new Playlist {User=users[1], Description = "Lorem ipsum", IsPublic = true, Name = "Playlist 3"},
                new Playlist {User=users[1], Description = "Lorem ipsum", IsPublic = false, Name = "Playlist 4"},
                new Playlist {User=users[2], Description = "Lorem ipsum", IsPublic = true, Name = "Playlist 5"},
                new Playlist {User=users[3], Description = "Lorem ipsum", IsPublic = false, Name = "Playlist 6"},
                new Playlist {User=users[4], Description = "Lorem ipsum", IsPublic = true, Name = "Playlist 7"}
            };

            context.Playlists.AddRange(playlists);
            context.SaveChanges();

            return playlists;
        }

        public static User[] AddUsers(UserManager<User> userManager)
        {
            var users = new[]
            {
                new User {Email = "antanas@gmail.com", UserName = "Antanas", Description = "Description", Location = "Location"},
                new User {Email = "petras.sakys@gmail.com", UserName = "Petras", Description = "Description", Location = "Location"},
                new User {Email = "petras2@gmail.com", UserName = "Petras2", Description = "Description", Location = "Location"},
                new User {Email = "jonas.jonaitis@gmail.com", UserName = "Jonas", Description = "Description", Location = "Location"},
                new User {Email = "antanas2@gmail.com", UserName = "Antanas2", Description = "Description", Location = "Location"},
                new User {Email = "petras3@gmail.com", UserName = "Petras3", Description = "Description", Location = "Location"},
                new User {Email = "vardenis.pav@gmail.com", UserName = "Vardenis", Description = "Description", Location = "Location"},
                new User {Email = "antanas3@gmail.com", UserName = "Antanas3", Description = "Description", Location = "Location"},
                new User {Email = "tadas1@gmail.com", UserName = "tadas1", Description = "Description", Location = "Location"},
                new User {Email = "tadas2@gmail.com", UserName = "tadas2", Description = "Description", Location = "Location"},
                new User {Email = "tadas3.pav@gmail.com", UserName = "tadas3", Description = "Description", Location = "Location"},
                new User {Email = "tadas4@gmail.com", UserName = "tadas4", Description = "Description", Location = "Location"},
                new User {Email = "user1@gmail.com", UserName = "user1", Description = "Description", Location = "Location"},
                new User {Email = "user2@gmail.com", UserName = "user2", Description = "Description", Location = "Location"},
                new User {Email = "user3@gmail.com", UserName = "user3", Description = "Description", Location = "Location"},
                new User {Email = "user4@gmail.com", UserName = "user4", Description = "Description", Location = "Location"},
                new User {Email = "user5@gmail.com", UserName = "user5", Description = "Description", Location = "Location"},
                new User {Email = "user6@gmail.com", UserName = "user6", Description = "Description", Location = "Location"},
                new User {Email = "user7@gmail.com", UserName = "user7", Description = "Description", Location = "Location"}
            };

            foreach (var user in users)
            {
                var result = userManager.CreateAsync(user, "Testas123?").Result;
            }

            return users;
        }

        public static Genre[] AddGenres(ApplicationDbContext context)
        {
            var genres = new[]
            {
                new Genre {Name = "Genre 1"},
                new Genre {Name = "Genre 2"},
                new Genre {Name = "Genre 3"},
                new Genre {Name = "Genre 4"},
                new Genre {Name = "Genre 5"},
                new Genre {Name = "Genre 6"},
                new Genre {Name = "Genre 7"}
            };

            context.Genres.AddRange(genres);
            context.SaveChanges();

            return genres;
        }

        public static Song[] AddSongs(ApplicationDbContext context, User[] users, Genre[] genres)
        {
            var songs = new[]
            {
                new Song
                {
                    Genre = genres[0],
                    User = users[0],
                    Title = "Song 1",
                    Description =
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                    UploadDate = DateTime.Now,
                    Duration = 300,
                    Plays = 1000
                },
                new Song
                {
                    Genre = genres[0],
                    User = users[0],
                    Title = "Song 2",
                    Description =
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                    UploadDate = DateTime.Now,
                    Duration = 300,
                    Plays = 1000
                },
                new Song
                {
                    Genre = genres[1],
                    User = users[1],
                    Title = "Song 3",
                    Description =
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                    UploadDate = DateTime.Now,
                    Duration = 300,
                    Plays = 1000
                },
                new Song
                {
                    Genre = genres[2],
                    User = users[2],
                    Title = "Song 4",
                    Description =
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                    UploadDate = DateTime.Now,
                    Duration = 300,
                    Plays = 1000
                },
                new Song
                {
                    Genre = genres[3],
                    User = users[3],
                    Title = "Song 5",
                    Description =
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                    UploadDate = DateTime.Now,
                    Duration = 300,
                    Plays = 1000
                }
            };

            context.Songs.AddRange(songs);
            context.SaveChanges();

            return songs;
        }
    }
}
