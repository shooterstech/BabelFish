using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.Runtime.Authentication;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.Requests {
    /// <summary>
    /// Abstract base class for all Request Objects.
    /// A concret implementation of a Request class should coorespond to exactly one REST API method call.
    /// </summary>
    public abstract class Request {

        private readonly Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public Request(string operationId) {
            if (string.IsNullOrEmpty( operationId )) throw new ArgumentNullException( "OperationId may not be null or an empty string" );
            this.OperationId = operationId;
        }
        public Request( string operationId, UserAuthentication credentials ) {
            if (string.IsNullOrEmpty( operationId )) throw new ArgumentNullException( "OperationId may not be null or an empty string" );
            this.OperationId = operationId;

            this.Credentials = credentials;
        }

        /// <summary>
        /// Indicates if this request requires user credentials.
        /// </summary>
        public bool RequiresCredentials { get; protected set; } = false;

        public UserAuthentication Credentials { get; set; }

        /// <summary>
        /// The REST API subdomain used in this request. 
        /// </summary>
        public APISubDomain SubDomain { get; protected set; } = APISubDomain.API;

        /// <summary>
        /// An unique string used to identify an operation. 
        /// Should be the same as the operation id listed in the Swagger documentation.
        /// </summary>
        public string OperationId { get; protected set; }

        public HttpMethod HttpMethod { get; protected set; } = HttpMethod.Get;

        /// <summary>
        /// Key / Value pairs of data that should be included in the request header.
        /// x-api-key is not generally included in this list, and instead is specified in the APIClient.
        /// </summary>
        public Dictionary<string, string> HeaderKeyValuePairs { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Assigned from Response value, for some APIs where Limit is used
        /// </summary>
        public virtual string ContinuationToken { get; set; } = string.Empty;

        /// <summary>
        /// The relative path for this API Request call. For example, if the complete REST API call is
        /// https://api.orionscoringsystem.com/match/1.1.20022012248563984.0, then this property would return
        /// "/match/1.1.20022012248563984.0". Note, return value includes the front slash.
        /// </summary>
        public virtual string RelativePath {
            get {
                return "";
            }
        }

        /// <summary>
        /// Returns a dictionary of name value pairs. Where the keys in the dictionary are the names
        /// And the value is a list of parameter values. The values are unescaped.
        /// </summary>
        public virtual Dictionary<string, List<string>> QueryParameters {
            get {
                return new Dictionary<string, List<string>>();
            }
        }

        /// <summary>
        /// Returns a string representing the query string that may be used in the Rest API Call
        /// </summary>
        public string QueryString {
            get {
                //    "Convert the return value of QueryParameters into an escaped string that may be used in a Rest API call.");
                List<string> paramList = new List<string>();
                foreach (KeyValuePair<string, List<string>> kvp in QueryParameters) {
                    foreach (string val in kvp.Value)
                        paramList.Add( String.Format( "{0}={1}", HttpUtility.UrlEncode( kvp.Key ), HttpUtility.UrlEncode( val ) ) );
                }
                var stringtoreturn = String.Join( "&", paramList );
                return stringtoreturn;
            }
        }

        /// <summary>
        /// Returns the fragment portion of a Rest API call. Note: Not commonly used.
        /// </summary>
        public virtual string Fragment {
            get {
                return "";
            }
        }

        //TODO: Figure out how to return values to be posted. Noting that sometimes we post a JSON string, much more than name value pairs. 
        public virtual StringContent PostParameters {
            get {
                return new StringContent( "" );
            }
        }

        public override string ToString() {
            return $"{OperationId} request";
        }
    }
}
