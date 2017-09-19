using System.ComponentModel.DataAnnotations;

namespace SaitynoProjektasBackEnd.RequestModels
{
    public class AddSongRequestModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string PictureUrl { get; set; }

        [Required]
        public string Genre { get; set; }
    }
}
