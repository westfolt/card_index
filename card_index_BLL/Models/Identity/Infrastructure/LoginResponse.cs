using System.Collections.Generic;

namespace card_index_BLL.Models.Identity.Infrastructure
{
    /// <summary>
    /// Object to send when user loggs in
    /// </summary>
    public class LoginResponse
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
        /// Login result
        /// </summary>
        public bool Succeeded { get; set; }
        /// <summary>
        /// Info message
        /// </summary>
        public string? Message { get; set; }
        /// <summary>
        /// JSON Web Token
        /// </summary>
        public string? Token { get; set; }
        /// <summary>
        /// Errors list
        /// </summary>
        public List<string> Errors { get; set; }
    }
}
