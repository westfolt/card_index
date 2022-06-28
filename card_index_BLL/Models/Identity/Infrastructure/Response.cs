using System.Collections.Generic;

namespace card_index_BLL.Models.Identity.Infrastructure
{
    /// <summary>
    /// Object for sending to client app
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Parameterless constructor, when error
        /// </summary>
        public Response()
        {
            Succeeded = false;
        }
        /// <summary>
        /// Constructor with operation result and info message
        /// </summary>
        /// <param name="succeeded">Is operation succeeded</param>
        /// <param name="message">Message about operation</param>
        public Response(bool succeeded, string message)
        {
            Succeeded = succeeded;
            Message = message;
        }
        /// <summary>
        /// Operation result
        /// </summary>
        public bool Succeeded { get; set; }
        /// <summary>
        /// Information message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Errors list
        /// </summary>
        public List<string> Errors { get; set; }
    }
}
