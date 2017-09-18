using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace SaitynoProjektasBackEnd.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string ProfilePictureUrl { get; set; }

        public ICollection<Song> Songs { get; set; }
        public ICollection<Playlist> Playlists { get; set; }
        public ICollection<User> Following { get; set; }
    }
}
