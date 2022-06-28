using System.Collections.Generic;

namespace card_index_BLL.Models.Identity.Infrastructure
{
    public class LoginResponse
    {
        public LoginResponse()
        {
            Succeeded = false;
        }

        public LoginResponse(bool succeeded, string message, string token)
        {
            Succeeded = succeeded;
            Message = message;
            Token = token;
        }
        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
        public List<string> Errors { get; set; }
    }
}
