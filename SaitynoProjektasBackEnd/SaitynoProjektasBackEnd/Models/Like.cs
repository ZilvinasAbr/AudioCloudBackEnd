﻿namespace SaitynoProjektasBackEnd.Models
{
    public class Like
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int? SongId { get; set; }
        public Song Song { get; set; }

        public int? PlaylistId { get; set; }
        public Playlist Playlist { get; set; }
    }
}
