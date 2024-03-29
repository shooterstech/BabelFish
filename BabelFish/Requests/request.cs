﻿using System;
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
using Amazon.CognitoIdentity.Model.Internal.MarshallTransformations;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Requests {
    /// <summary>
    /// Abstract base class for all Request Objects.
    /// A concret implementation of a Request class should coorespond to exactly one REST API method call.
    /// </summary>
    public abstract class Request {

        private readonly Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationId"></param>
        /// <exception cref="ArgumentNullException">Thrown if OperationId is null or an empty string</exception>
        public Request(string operationId) {
            if (string.IsNullOrEmpty( operationId )) throw new ArgumentNullException( "OperationId may not be null or an empty string" );
            this.OperationId = operationId;
			this.SubDomain = APISubDomain.API;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationId"></param>
        /// <param name="credentials"></param>
        /// <exception cref="ArgumentNullException">Thrown if UserAuthentication is null or OperationId is null or an empty string</exception>
        public Request( string operationId, UserAuthentication credentials ) {
            if (string.IsNullOrEmpty( operationId )) 
                throw new ArgumentNullException( "OperationId may not be null or an empty string" );

            if (credentials == null)
                throw new ArgumentNullException( "User Credentials argument may not be null." );

            this.OperationId = operationId;

			this.RequiresCredentials = true;
			this.SubDomain = APISubDomain.AUTHAPI;
			this.Credentials = credentials;
        }

        /// <summary>
        /// Creates a new instance of a Request Object, with all the same parameters
        /// </summary>
        /// <returns></returns>
        public virtual Request Copy() { throw new NotImplementedException("Concrete implementations of Request should implement Copy for their class."); }

		/// <summary>
		/// Concrete implementationst that want to cache their requests, must implement
		/// a unique string to be used as the request key. 
		/// </summary>
		/// <returns></returns>
		protected internal string GetRequestCacheKey() {
            var accessToken = "";
            if (Credentials != null)
                accessToken = Credentials.AccessToken;

            //NOTE: Purposefully not including PostParameters, as PostParameters are only included
            //on non-httpmethod-GET calls. And cache is only for GET calls.
            StringBuilder key = new StringBuilder();
            key.Append( this.SubDomain.SubDomainNameWithStage() );
            key.Append( this.RelativePath );
            if (!string.IsNullOrEmpty( this.QueryString )) {
                key.Append( '/' );
                key.Append( this.QueryString );
            }
            if (!string.IsNullOrEmpty( accessToken )) {
				key.Append( '/' );
                key.Append( accessToken );
			}
            return key.ToString();
		}

		/// <summary>
		/// Indicates if the local response cache should be ignored and always 
		/// make the request to the Rest API.
		/// The default value is false, meaning to use the local cache (if avaliable and permitted by the Rest API client).
		/// The option to ignore local cache can either be set at the API Client level, or on a per request level. Cached responses are only valid for HttpMethod GET calls.
		/// </summary>
		public bool IgnoreInMemoryCache { get; set; } = false;

        public bool IgnoreFileSystemCache { get; set; } = false;

		/// <summary>
		/// Indicates if this request requires user credentials. Automatically set to True when the Request constructor using UserAuthentication is used.
		/// </summary>
		public bool RequiresCredentials { get; protected set; } = false;

        public UserAuthentication Credentials { get; set; }

        /// <summary>
        /// The REST API subdomain used in this request. 
        /// Automatically set to AUTHAPI when using the Request constructor using UserAuthentication. Set to API otherwise.
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

        
        /// <summary>
        /// Only applicable to non httpMethod.GET calls. This is the body of the request.
        /// </summary>
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
