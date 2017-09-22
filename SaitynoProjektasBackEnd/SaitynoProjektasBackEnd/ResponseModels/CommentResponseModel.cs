using System;

namespace SaitynoProjektasBackEnd.ResponseModels
{
    public class CommentResponseModel
    {
        public string Message { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UserName { get; set; }
    }
}
