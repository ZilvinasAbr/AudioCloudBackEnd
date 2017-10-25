using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SaitynoProjektasBackEnd.ResponseModels
{
    public class GenreResponseModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }
    }
}
