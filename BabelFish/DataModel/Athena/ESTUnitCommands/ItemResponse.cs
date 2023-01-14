using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Scopos.BabelFish.DataModel.Athena.ESTUnitCommands {
    public  class ItemResponse {

        /// <summary>
        /// Status code, who's values are takend from the HTTP Status Code, for the 
        /// command request. 
        /// </summary>
        [JsonConverter( typeof( StringEnumConverter ) )]
        public HttpStatusCode Status { get; set; }

        /// <summary>
        /// Human readable response message for the command request.
        /// </summary>
        public string Message { get; set; }
    }
}
