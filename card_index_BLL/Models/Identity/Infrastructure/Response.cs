using System;
using System.Collections.Generic;
using System.Text;

namespace card_index_BLL.Models.Identity.Infrastructure
{
    public class Response
    {
        public Response()
        {
            Succeeded = false;
        }
        public Response(bool succeeded, string message)
        {
            Succeeded = succeeded;
            Message = message;
        }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
    }
}
