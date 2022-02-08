using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BabelFish.Responses
{
    /// <summary>
    /// Abstract class representing all Rest API Responses.
    /// T is the type of data object expected returned by the REST API call.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Response<T>
    {
        //TODO Require T have a public constructor

        /// <summary>
        /// Gets or sets the data object returned by the Rest API Call.
        /// </summary>
        public T Value
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or Sets the Status Code returned by the Rest API call.
        /// </summary>
        public HttpStatusCode StatusCode { get; internal set; }

        /// <summary>
        /// Gets or Sets the raw body returned by the Rest API Call.
        /// </summary>
        public string Body { get; internal set; }

        /// <summary>
        /// Gets or Sets TimeSpan of API call
        /// </summary>
        public string TimeToRun { get; internal set; } = string.Empty;
    }
}
