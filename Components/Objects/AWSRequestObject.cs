using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabelFish.Components.Objects
{
    public class AWSRequestObject
    {
        public AWSRequestObject(){}

        public AWSRequestObject(string requestMethod, Dictionary<string, string> requestHeader)
        {
            RequestMethod = AWSUtility.ValidRequestMethodEnum(requestMethod.ToUpper());
            RequestHeaders = requestHeader;
        }

        public AWSRequestObject(Dictionary<string, string> requestHeader, string requestMethod, string environment, string urlQuery) 
            : this(requestMethod, requestHeader)
        {
            Subdomain = AWSUtility.ValidateSubdomain("api");
            Environment = environment.ToLower();
            UrlQuery = urlQuery;
        }

        #region Properties
        /// <summary>
        /// Special Request Header ParameteR
        /// Contains x-api-key at a minimum
        /// </summary>
        public Dictionary<string, string> RequestHeaders { get; private set; } = new Dictionary<string, string>();

        /// <summary>
        /// Call Method Parameter
        /// </summary>
        public string RequestMethod { get; private set; } = string.Empty;

        /// <summary>
        /// Subdomain Parameter
        /// </summary>
        public string Subdomain { get; set; } = string.Empty;

        /// <summary>
        /// Environment Stage Parameter
        /// </summary>
        public string Environment { get; set; } = string.Empty;

        /// <summary>
        /// Indivdual API call's unique URL+params string after Environment parameter
        /// </summary>
        public string UrlQuery { get; set; } = string.Empty;
        #endregion Properties
    }
}

