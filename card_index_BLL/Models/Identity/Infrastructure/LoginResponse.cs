using System.Collections.Generic;

namespace card_index_BLL.Models.Identity.Infrastructure
{
    /// <summary>
    /// Object to send when user logs in
    /// </summary>
    public class LoginResponse:Response
    {
        /// <summary>
        /// Constructor without parameters, when unsuccessful login
        /// </summary>
        public LoginResponse()
        {
            Succeeded = false;
        }

        /// <summary>
        /// All parameters except errors set up via constructor
        /// </summary>
        /// <param name="succeeded">Login successful</param>
        /// <param name="message">Info message</param>
        /// <param name="token">JSON Web token</param>
        public LoginResponse(bool succeeded, string message, string token)
        {
            Succeeded = succeeded;
            Message = message;
            Token = token;
        }
        /// <summary>
        /// JSON Web Token
        /// </summary>
        public string Token { get; set; }
    }
}
