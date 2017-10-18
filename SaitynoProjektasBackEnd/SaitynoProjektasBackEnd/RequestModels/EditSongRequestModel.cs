using System.ComponentModel.DataAnnotations;

namespace SaitynoProjektasBackEnd.RequestModels
{
    public class EditSongRequestModel
    {
        [MaxLength(255)]
        public string Title { get; set; }

        [MaxLength(5000)]
        public string Description { get; set; }
    }
}
