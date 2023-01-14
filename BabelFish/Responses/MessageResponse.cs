using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Responses
{
    public class MessageResponse : IResponse
    {

        public List<String> Message { get; set; } = new List<string>();

        /// <summary>
        /// If  NextToken is not an empty string, it means there is more data to be returned, and may be requested on the next call using this value.
        /// </summary>
        public string NextToken { get; set; } = string.Empty;

        [Obsolete( "ResponseCodes is no longer being included in new API Requests, and will stop being included in existing ones as they are updated. Use the response' StatusCode instead." )]
        public List<String> ResponseCodes { get; set; } = new List<string>();

        [Obsolete( "Title is no longer being included in new API Requests, and will stop being included in existing ones as they are updated." )]
        public string Title { get; set; } = string.Empty;
    }
}
