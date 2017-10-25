using System;
using System.Collections.Generic;
using SaitynoProjektasBackEnd.Models;

namespace SaitynoProjektasBackEnd.ResponseModels
{
    public class SongResponseModel
    {
        public int Id { get; set; }
        public int? TrackNumber { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadDate { get; set; }
        public string PictureUrl { get; set; }
        public int Duration { get; set; }
        public int Plays { get; set; }
        public int Likes { get; set; }
        public string Genre { get; set; }

        public UserResponseModel User { get; set; }
    }
}
