using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Responses {

    /// <summary>
    /// The Message Response contains all of the standard fields returned in a Scopos Rest API call, including Message and NextToken (if used). What it doesn't contain is the requested data model object.
    /// </summary>
    public class MessageResponse : IResponse {

        public List<String> Message { get; set; } = new List<string>();

        [Obsolete( "ResponseCodes is no longer being included in new API Requests, and will stop being included in existing ones as they are updated. Use the response' StatusCode instead." )]
        public List<String> ResponseCodes { get; set; } = new List<string>();

        [Obsolete( "Title is no longer being included in new API Requests, and will stop being included in existing ones as they are updated." )]
        public string Title { get; set; } = string.Empty;
    }
}
