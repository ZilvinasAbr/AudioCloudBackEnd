using System.Collections.Generic;

namespace SaitynoProjektasBackEnd.ResponseModels
{
    public class UserResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}
