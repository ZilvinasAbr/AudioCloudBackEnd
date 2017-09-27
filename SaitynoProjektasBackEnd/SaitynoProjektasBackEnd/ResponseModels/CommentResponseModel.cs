using System;

namespace SaitynoProjektasBackEnd.ResponseModels
{
    public class CommentResponseModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UserName { get; set; }
    }
}
