using System;

namespace SaitynoProjektasBackEnd.ResponseModels
{
    public class EventResponseModel
    {
        public DateTime CreatedOn { get; set; }
        public string EventType { get; set; }
        public string SongTitle { get; set; }
        public string UserName { get; set; }
    }
}
