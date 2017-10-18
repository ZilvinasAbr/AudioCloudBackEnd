using System.ComponentModel.DataAnnotations;

namespace SaitynoProjektasBackEnd.RequestModels
{
    public class EditUserRequestModel
    {
        [MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Location { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}
