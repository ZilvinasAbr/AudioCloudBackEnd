using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SaitynoProjektasBackEnd.Models
{
    public class Playlist
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(5000)]
        public string Description { get; set; }

        public bool IsPublic { get; set; }

        [Required]
        public User User { get; set; }
        public ICollection<PlaylistSong> PlaylistSongs { get; set; }
        public ICollection<Like> Likes { get; set; }
    }
}
