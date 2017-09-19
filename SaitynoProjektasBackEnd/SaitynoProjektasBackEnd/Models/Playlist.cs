using System.Collections.Generic;

namespace SaitynoProjektasBackEnd.Models
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }

        public User User { get; set; }
        public ICollection<PlaylistSong> PlaylistSongs { get; set; }
        public ICollection<Like> Likes { get; set; }
    }
}
