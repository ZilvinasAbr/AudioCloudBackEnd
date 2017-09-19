using System.Collections.Generic;

namespace SaitynoProjektasBackEnd.ResponseModels
{
    public class PlaylistResponseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public string UserName { get; set; }
        public IEnumerable<SongResponseModel> Songs { get; set; }
        public int Likes { get; set; }
    }
}
