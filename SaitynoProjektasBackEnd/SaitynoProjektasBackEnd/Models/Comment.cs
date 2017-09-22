using System;

namespace SaitynoProjektasBackEnd.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Message { get; set; }
        public DateTime CreatedOn { get; set; }

        public User User { get; set; }

        public int SongId { get; set; }
        public Song Song { get; set; }


    }
}
