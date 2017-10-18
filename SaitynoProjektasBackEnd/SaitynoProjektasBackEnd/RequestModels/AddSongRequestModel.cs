using System.ComponentModel.DataAnnotations;

namespace SaitynoProjektasBackEnd.RequestModels
{
    public class AddSongRequestModel
    {
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        [MaxLength(5000)]
        public string Description { get; set; }

        public string PictureUrl { get; set; }

        [Required]
        public string FilePath { get; set; }

        [Required]
        public string Genre { get; set; }
    }
}
