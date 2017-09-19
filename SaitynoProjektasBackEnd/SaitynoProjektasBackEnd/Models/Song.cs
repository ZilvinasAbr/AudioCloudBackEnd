using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SaitynoProjektasBackEnd.Models
{
    public class Song
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Title { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime UploadDate { get; set; }

        public string PictureUrl { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public int Plays { get; set; }

        public User User { get; set; }
        public Genre Genre { get; set; }
  
        public ICollection<PlaylistSong> PlaylistSongs { get; set; }
        public ICollection<Like> Likes { get; set; }
    }
}
