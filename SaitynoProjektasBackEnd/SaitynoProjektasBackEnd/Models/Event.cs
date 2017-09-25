using System;

namespace SaitynoProjektasBackEnd.Models
{

    public class Event
    {
        public static string SongAdded = "Song Added";
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public string EventType { get; set; }

        public int SongId { get; set; }
        public Song Song { get; set; }

        public User User { get; set; }
    }
}
