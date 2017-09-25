using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaitynoProjektasBackEnd.Models
{
    public class Following
    {
        public int FollowerId { get; set; }

        public User Follower { get; set; }

        public int FollowedId { get; set; }

        public User Followed { get; set; }
    }
}
