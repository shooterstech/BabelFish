using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using BabelFish.Requests;

namespace BabelFish {
    public abstract class APIClient {

        [JsonConverter( typeof( StringEnumConverter ) )]
        public enum APIStage {
            [Description( "alpha" )] [EnumMember( Value = "alpha" )] ALPHA,
            [Description( "beta" )] [EnumMember( Value = "beta" )] BETA,
            [Description( "production" )] [EnumMember( Value = "production" )] PRODUCTION
        }

        public APIClient( string xapikey ) {
            this.XApiKey = xapikey;
            ApiStage = APIStage.PRODUCTION;
        }

        public string XApiKey { get; set; }

        public APIStage ApiStage { get; set; }
    }
}