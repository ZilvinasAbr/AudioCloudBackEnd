using System;
using System.Linq;
using SaitynoProjektasBackEnd.Models;

namespace SaitynoProjektasBackEnd.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Songs.Any())
            {
                return;
            }

            var users = AddUsers();
            var genres = AddGenres(context);
            var playlists = AddPlaylists(context, users);
            var songs = AddSongs(context, users, genres);
            var comments = AddComments(context, users, songs);
            var playlistSongs = AddPlaylistSongs(context, songs, playlists);
            var likes = AddLikes(context, users, songs, playlists);
        }

        private static Comment[] AddComments(ApplicationDbContext context, User[] users, Song[] songs)
        {
            var comments = new[]
            {
                new Comment{Song = songs[0], User = users[3], Message = "Message 1", CreatedOn = DateTime.Now},
                new Comment{Song = songs[0], User = users[4], Message = "Message 2", CreatedOn = DateTime.Now},
                new Comment{Song = songs[0], User = users[3], Message = "Message 3", CreatedOn = DateTime.Now},
                new Comment{Song = songs[0], User = users[4], Message = "Message 4", CreatedOn = DateTime.Now},
                new Comment{Song = songs[0], User = users[3], Message = "Message 5", CreatedOn = DateTime.Now},
                new Comment{Song = songs[0], User = users[4], Message = "Message 6", CreatedOn = DateTime.Now}
            };

            context.Comments.AddRange(comments);
            context.SaveChanges();

            return comments;
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

        public static User[] AddUsers()
        {
            var users = new[]
            {
                new User {UserName = "Antanas", Description = "Description", Location = "Location"},
                new User {UserName = "Petras", Description = "Description", Location = "Location"},
                new User {UserName = "Petras2", Description = "Description", Location = "Location"},
                new User {UserName = "Jonas", Description = "Description", Location = "Location"},
                new User {UserName = "Antanas2", Description = "Description", Location = "Location"},
                new User {UserName = "Petras3", Description = "Description", Location = "Location"},
                new User {UserName = "Vardenis", Description = "Description", Location = "Location"},
                new User {UserName = "Antanas3", Description = "Description", Location = "Location"},
                new User {UserName = "tadas1", Description = "Description", Location = "Location"},
                new User {UserName = "tadas2", Description = "Description", Location = "Location"},
                new User {UserName = "tadas3", Description = "Description", Location = "Location"},
                new User {UserName = "tadas4", Description = "Description", Location = "Location"},
                new User {UserName = "user1", Description = "Description", Location = "Location"},
                new User {UserName = "user2", Description = "Description", Location = "Location"},
                new User {UserName = "user3", Description = "Description", Location = "Location"},
                new User {UserName = "user4", Description = "Description", Location = "Location"},
                new User {UserName = "user5", Description = "Description", Location = "Location"},
                new User {UserName = "user6", Description = "Description", Location = "Location"},
                new User {UserName = "user7", Description = "Description", Location = "Location"}
            };

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
