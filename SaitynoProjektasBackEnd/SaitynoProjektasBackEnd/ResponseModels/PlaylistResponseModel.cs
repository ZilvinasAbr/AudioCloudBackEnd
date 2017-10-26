using System.Collections.Generic;

namespace SaitynoProjektasBackEnd.ResponseModels
{
    public class PlaylistResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public UserResponseModel User { get; set; }
        public IList<SongResponseModel> Songs { get; set; }
    }
}
