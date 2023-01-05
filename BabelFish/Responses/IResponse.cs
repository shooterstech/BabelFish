using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShootersTech.BabelFish.Responses
{
    interface IResponse
    {

        List<String> Message { get; set; }

        string NextToken { get; set; }

        [Obsolete( "ResponseCodes is no longer being included in new API Requests, and will stop being included in existing ones as they are updated. Use the response' StatusCode instead." )]
        List<String> ResponseCodes { get; set; }

        [Obsolete( "Title is no longer being included in new API Requests, and will stop being included in existing ones as they are updated." )]
        string Title { get; set; }
    }
}
