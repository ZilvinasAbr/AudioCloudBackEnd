using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SaitynoProjektasBackEnd.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string Location { get; set; }

        [Required]
        public string Description { get; set; }

        public string ProfilePictureUrl { get; set; }

        public ICollection<Song> Songs { get; set; }
        public ICollection<Playlist> Playlists { get; set; }
        public ICollection<User> Following { get; set; }
    }
}
