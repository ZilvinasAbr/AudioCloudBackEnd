using System.ComponentModel.DataAnnotations;

namespace SaitynoProjektasBackEnd.RequestModels
{
    public class AddPlaylistRequestModel
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(5000)]
        public string Description { get; set; }

        [Required]
        public bool? IsPublic { get; set; }
    }
}
