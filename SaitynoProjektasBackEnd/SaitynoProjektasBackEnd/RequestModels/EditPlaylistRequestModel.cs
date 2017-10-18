using System.ComponentModel.DataAnnotations;

namespace SaitynoProjektasBackEnd.RequestModels
{
    public class EditPlaylistRequestModel
    {
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(5000)]
        public string Description { get; set; }
        public bool? IsPublic { get; set; }
    }
}
