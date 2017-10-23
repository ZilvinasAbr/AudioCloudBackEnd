using System;

namespace SaitynoProjektasBackEnd.ResponseModels
{
    public class EventResponseModel
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EventType { get; set; }
        public string SongTitle { get; set; }
        public string UserName { get; set; }
        public SongResponseModel Song { get; set; }
        public UserResponseModel User { get; set; }
    }
}
