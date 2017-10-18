using System;
using System.ComponentModel.DataAnnotations;

namespace SaitynoProjektasBackEnd.Models
{
    public class Event
    {
        public static string SongAdded = "Song Added";
        public int Id { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }
        
        [Required]
        public string EventType { get; set; }

        public int SongId { get; set; }
        public Song Song { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
