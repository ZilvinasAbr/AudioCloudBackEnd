using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SaitynoProjektasBackEnd.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string AuthId { get; set; }
        
        public string UserName { get; set; }
        
        public string Location { get; set; }
        
        public string Description { get; set; }

        public string ProfilePictureUrl { get; set; }

        public ICollection<Song> Songs { get; set; }
        public ICollection<Playlist> Playlists { get; set; }
        public ICollection<Following> Followers { get; set; }
        public ICollection<Following> Following { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}
