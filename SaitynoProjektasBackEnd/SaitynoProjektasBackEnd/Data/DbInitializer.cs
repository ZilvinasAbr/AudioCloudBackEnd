using System;
using System.Linq;
using SaitynoProjektasBackEnd.Models;
using Microsoft.EntityFrameworkCore;

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

            var users = AddUsers(context);
            var followings = AddFollowings(context, users);
            var genres = AddGenres(context);
            var playlists = AddPlaylists(context, users);
            var songs = AddSongs(context, users, genres);
            var playlistSongs = AddPlaylistSongs(context, songs, playlists);
            var likes = AddLikes(context, users, songs, playlists);

            GenerateEvents(context);
        }

        private static Following[] AddFollowings(ApplicationDbContext context, User[] users)
        {
            var followings = new []
            {
                new Following {Follower=users[0], Followed=users[1]},
                new Following {Follower=users[0], Followed=users[2]},
                new Following {Follower=users[0], Followed=users[3]},
                new Following {Follower=users[0], Followed=users[4]},
                new Following {Follower=users[1], Followed=users[0]},
                new Following {Follower=users[1], Followed=users[2]},
                new Following {Follower=users[1], Followed=users[3]},
                new Following {Follower=users[1], Followed=users[4]},
                new Following {Follower=users[11], Followed=users[0]},
                new Following {Follower=users[10], Followed=users[0]},
            };

            context.Followings.AddRange(followings);
            context.SaveChanges();

            return followings;
        }

        private static void GenerateEvents(ApplicationDbContext context)
        {
            var songs = context.Songs
                .Include(s => s.User)
                .ToList();

            var events = songs.Select(s => new Event
            {
                CreatedOn = s.UploadDate,
                EventType = Event.SongAdded,
                Song = s,
                User = s.User
            });

            context.Events.AddRange(events);
            context.SaveChanges();
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
                new Like {User = users[0], Song = songs[2], CreatedOn = DateTime.Now},
                new Like {User = users[0], Song = songs[3], CreatedOn = DateTime.Now},
                new Like {User = users[0], Song = songs[4], CreatedOn = DateTime.Now},
                new Like {User = users[10], Song = songs[0], CreatedOn = DateTime.Now},
                new Like {User = users[1], Song = songs[2], CreatedOn = DateTime.Now},
                new Like {User = users[1], Song = songs[3], CreatedOn = DateTime.Now},
                new Like {User = users[1], Song = songs[4], CreatedOn = DateTime.Now},
                new Like {User = users[1], Song = songs[0], CreatedOn = DateTime.Now}
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

        public static User[] AddUsers(ApplicationDbContext context)
        {
            // On seed data AuthId equals a random string value, it is not possible to use these users.
            var users = new[]
            {
                new User {UserName = "Test Client", ProfilePictureUrl="/image.png", Description = "Description", Location = "Location", AuthId = "lr7qwwgZlzvfQbC89SKPo4YiQ85UcCsk@clients"},
                new User {UserName = "User 1", ProfilePictureUrl="/image.png", Description = "Description", Location = "Location", AuthId = "auth0|59d527e6a83e933c763fb5e4"},
                new User {UserName = "User 2", ProfilePictureUrl="/image.png", Description = "Description", Location = "Location", AuthId = "auth0|59d52843ea7eb262c0566c60"},
                new User {UserName = "User 3", ProfilePictureUrl="/image.png", Description = "Description", Location = "Location", AuthId = "auth0|59d52855a83e933c763fb602"},
                new User {UserName = "User 4", ProfilePictureUrl="/image.png", Description = "Description", Location = "Location", AuthId = "auth0|59d52865ea7eb262c0566c66"},
                new User {UserName = "User 5", ProfilePictureUrl="/image.png", Description = "Description", Location = "Location", AuthId = "auth0|59d5287da83e933c763fb60e"},
                new User {UserName = "User 6", ProfilePictureUrl="/image.png", Description = "Description", Location = "Location", AuthId = "auth0|59d5288eea7eb262c0566c78"},
                new User {UserName = "User 7", ProfilePictureUrl="/image.png", Description = "Description", Location = "Location", AuthId = "auth0|59d5289da83e933c763fb628"},
                new User {UserName = "User 8", ProfilePictureUrl="/image.png", Description = "Description", Location = "Location", AuthId = "auth0|59d528aba83e933c763fb62e"},
                new User {UserName = "User 9", ProfilePictureUrl="/image.png", Description = "Description", Location = "Location", AuthId = "auth0|59d528b5ea7eb262c0566c87"},
                new User {UserName = "User 10", ProfilePictureUrl="/image.png", Description = "Description", Location = "Location", AuthId = "auth0|59d528bfa83e933c763fb638"},
                new User {UserName = "User 11", ProfilePictureUrl="/image.png", Description = "Description", Location = "Location", AuthId = "auth0|59d52986a83e933c763fb65b"},
                new User {UserName = "User 12", ProfilePictureUrl="/image.png", Description = "Description", Location = "Location", AuthId = "auth0|59d52995a83e933c763fb65d"},
                new User {UserName = "User 13", ProfilePictureUrl="/image.png", Description = "Description", Location = "Location", AuthId = "auth0|59d529a2a83e933c763fb65e"},
                new User {UserName = "User 14", ProfilePictureUrl="/image.png", Description = "Description", Location = "Location", AuthId = "auth0|59d529afa83e933c763fb661"},
                new User {UserName = "User 15", ProfilePictureUrl="/image.png", Description = "Description", Location = "Location", AuthId = "auth0|59d529c0591a282330989334"}
            };

            context.Users.AddRange(users);
            context.SaveChanges();

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
                    FilePath = "audio1.mp3",
                    PictureUrl = "/image.png",
                    Title = "Artist - Song Title 1",
                    Description =
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                    UploadDate = DateTime.Now,
                    Duration = 300,
                    Plays = 55717
                },
                new Song
                {
                    Genre = genres[0],
                    User = users[0],
                    FilePath = "audio2.mp3",
                    PictureUrl = "/image.png",
                    Title = "Artist - Song Title 2",
                    Description =
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                    UploadDate = DateTime.Now,
                    Duration = 300,
                    Plays = 142145
                },
                new Song
                {
                    Genre = genres[1],
                    User = users[1],
                    FilePath = "audio3.mp3",
                    PictureUrl = "/image.png",
                    Title = "Artist - Song Title 3",
                    Description =
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                    UploadDate = DateTime.Now,
                    Duration = 300,
                    Plays = 4545
                },
                new Song
                {
                    Genre = genres[2],
                    User = users[2],
                    FilePath = "audio4.mp3",
                    PictureUrl = "/image.png",
                    Title = "Artist - Song Title 4",
                    Description =
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                    UploadDate = DateTime.Now,
                    Duration = 300,
                    Plays = 4545
                },
                new Song
                {
                    Genre = genres[3],
                    User = users[3],
                    FilePath = "audio5.mp3",
                    PictureUrl = "/image.png",
                    Title = "Artist - Song Title 5",
                    Description =
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                    UploadDate = DateTime.Now,
                    Duration = 300,
                    Plays = 68668688
                },
                new Song
                {
                    Genre = genres[0],
                    User = users[0],
                    FilePath = "audio6.mp3",
                    PictureUrl = "/image.png",
                    Title = "Artist - Song Title 6",
                    Description =
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                    UploadDate = DateTime.Now,
                    Duration = 300,
                    Plays = 77788578
                },
                new Song
                {
                    Genre = genres[0],
                    User = users[1],
                    FilePath = "audio7.mp3",
                    PictureUrl = "/image.png",
                    Title = "Artist - Song Title 7",
                    Description =
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                    UploadDate = DateTime.Now,
                    Duration = 300,
                    Plays = 51
                },
                new Song
                {
                    Genre = genres[0],
                    User = users[1],
                    FilePath = "audio8.mp3",
                    PictureUrl = "/image.png",
                    Title = "Artist - Song Title 8",
                    Description =
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                    UploadDate = DateTime.Now - TimeSpan.FromDays(8),
                    Duration = 300,
                    Plays = 1
                }
            };

            context.Songs.AddRange(songs);
            context.SaveChanges();

            return songs;
        }

        public static void CleanDatabase(ApplicationDbContext dbContext)
        {
            dbContext.Database.ExecuteSqlCommand("DROP TABLE dbo.Events DROP TABLE dbo.Followings DROP TABLE dbo.Genres DROP TABLE dbo.Likes DROP TABLE dbo.Playlists DROP TABLE dbo.PlaylistSongs DROP TABLE dbo.Songs DROP TABLE dbo.Users");
        }
    }
}
