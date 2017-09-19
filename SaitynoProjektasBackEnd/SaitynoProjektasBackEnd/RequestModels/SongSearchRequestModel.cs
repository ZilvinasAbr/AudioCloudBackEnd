using System.ComponentModel.DataAnnotations;

namespace SaitynoProjektasBackEnd.RequestModels
{
    public class SongSearchRequestModel
    {
        [Required]
        public string Query { get; set; }
    }
}
