using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabelFish.Helpers;
//using Microsoft.VisualBasic.CompilerServices; //COMMENT OUT FOR .NET Standard 2.0

namespace BabelFish.Requests
{
    /// <summary>
    /// Abstract base class for all Request Objects
    /// </summary>
    public abstract class Request
    {


        public Request() { }

        public bool WithAuthentication { get; set; } = false;

        public bool IsShootersTechURI { get; set; } = true;

        public virtual APIStage ApiStage { get; set; } = APIStage.PRODUCTION;

        public virtual SubDomains SubDomain 
        {
            get
            {
                if (this.WithAuthentication)
                {
                    return SubDomains.AUTHAPI_STAGE;
                }
                else
                {
                    return SubDomains.API_STAGE;
                }
            }
        }

        /// <summary>
        /// The relative path for this API Request call. For example, if the complete REST API call is
        /// https://api.orionscoringsystem.com/match/1.1.20022012248563984.0, then this property would return
        /// "/match/1.1.20022012248563984.0". Note, return value includes the front slash.
        /// </summary>
        public virtual string RelativePath
        {
            get
            {
                return "";
            }
        }

        /// <summary>
        /// Returns a dictionary of name value pairs. Where the keys in the dictionary are the names
        /// And the value is a list of parameter values. The values are unescaped.
        /// </summary>
        public virtual Dictionary<string, List<string>> QueryParameters
        {
            get
            {
                return new Dictionary<string, List<string>>();
            }
        }

        /// <summary>
        /// Returns a string representing the query string that may be used in the Rest API Call
        /// </summary>
        public string QueryString
        {
            get
            {
                //    "Convert the return value of QueryParameters into an escaped string that may be used in a Rest API call.");
                List<string> paramList = new List<string>();
                foreach (KeyValuePair<string, List<string>> kvp in QueryParameters) {
                    foreach (string val in kvp.Value)
                        paramList.Add(String.Format("{0}={1}", HttpUtility.UrlEncode(kvp.Key), HttpUtility.UrlEncode(val)));
                }
                var stringtoreturn = String.Join("&", paramList);
                return stringtoreturn;
            }
        }

        /// <summary>
        /// Returns the fragment portion of a Rest API call. Note: Not commonly used.
        /// </summary>
        public virtual string Fragment
        {
            get
            {
                return "";
            }
        }

        //TODO: Figure out how to return values to be posted. Noting that sometimes we post a JSON string, much more than name value pairs. 
        public virtual StringContent PostParameters
        {
            get
            {
                return new StringContent("");
            }
        }
    }
}
