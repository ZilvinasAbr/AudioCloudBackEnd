using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SaitynoProjektasBackEnd.Models
{
    public class Song
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        [MaxLength(5000)]
        public string Description { get; set; }

        [Required]
        public DateTime UploadDate { get; set; }

        [Required]
        public string FilePath { get; set; }

        public string PictureUrl { get; set; }

        public int Duration { get; set; }

        [Required]
        public int Plays { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
  
        public ICollection<PlaylistSong> PlaylistSongs { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}
