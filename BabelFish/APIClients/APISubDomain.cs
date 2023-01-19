using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.APIClients {

    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum APISubDomain {

        /// <summary>
        /// Public avaliable REST API. Authentication is not required.
        /// </summary>
        [Description( "api" )]
        [EnumMember( Value = "api" )]
        API,

        /// <summary>
        /// REST API requiring user authentication using their Scopos accounts.
        /// </summary>
        [Description( "authapi" )]
        [EnumMember( Value = "authapi" )]
        AUTHAPI,

        /// <summary>
        /// Scopos only internal access REST APIs.
        /// </summary>
        [Description( "internalapi" )]
        [EnumMember( Value = "internalapi" )]
        INTERNAL
    }

    public static class APISubDomainExstentions {

        /// <summary>
        /// When using the 'stage' specific REST API calls, this function will return the
        /// sub-domain portion of the URL. 
        /// </summary>
        /// <param name="subDomain"></param>
        /// <returns></returns>
        public static string SubDomainNameWithStage(this APISubDomain subDomain) {

            return $"{subDomain.Description()}-stage";

        }

        /// <summary>
        /// When using the always production REST API calls, this function will return the 
        /// sub-domain portion of the URL.
        /// </summary>
        /// <param name="subDomain"></param>
        /// <returns></returns>
        public static string SubDomainName( this APISubDomain subDomain ) {

            return subDomain.Description();

        }
    }
}
