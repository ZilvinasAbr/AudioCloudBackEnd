using System;
using System.Collections.Generic;

namespace SaitynoProjektasBackEnd.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime UploadDate { get; set; }
        public string PictureUrl { get; set; }
        public int Duration { get; set; }
        public int Plays { get; set; }

        public User User { get; set; }
        public Genre Genre { get; set; }
  
        public ICollection<PlaylistSong> PlaylistSongs { get; set; }
        public ICollection<Like> Likes { get; set; }
    }
}
