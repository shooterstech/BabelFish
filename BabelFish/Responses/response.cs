using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShootersTech.BabelFish.Requests;

namespace ShootersTech.BabelFish.Responses
{
    /// <summary>
    /// Abstract class representing all Rest API Responses.
    /// T is the type of data object expected returned by the REST API call.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Response<T>
        where T : new() {


        protected JToken body = new JObject();

        /// <summary>
        /// Base constructor.
        /// </summary>
        protected Response() {
            /*
             * Although this constructor is not doing anything, it is needed for base class Construcotrs that take in a concrete
             * Request object. This is because, if this constructor didn't exist the compiler would try and call Response(Request)
             * which throws a NotImplementedException.
             * 
             * A Concrete class constructor should have the following constructor definition:
             * public MyGreatResponseClass( MyGreatReqeustClass reqeust) : base() {}
             */
        }

        public Response( Request request ) {
            //Highly recommended that concrete implementations of Response override this constructor.
            //As a matter of fact, I'm going to purposefully throw an error is someone tries to call
            //this Constructor without the concrete class implementing it.

            //Baseline suggestion of implementation
            this.Request = request;

            throw new NotImplementedException( "Base classes should implement their own constructor that accepts a Request object." );
        }

        /// <summary>
        /// The Request object used to generate this Response object.
        /// </summary>
        public Request Request { get; protected set; }


        /// <summary>
        /// Gets or sets the status data object returned by the Rest API Call.
        /// </summary>
        public MessageResponse MessageResponse
        {
            get;
            internal set;
        } = new MessageResponse();

        /// <summary>
        /// Gets or sets the object data object returned by the Rest API Call.
        /// </summary>
        public T Value
        {
            get;
            internal set;
        } = new T();

        /// <summary>
        /// Gets or Sets the Status Code returned by the Rest API call.
        /// </summary>
        public HttpStatusCode StatusCode { get; internal set; }

        /// <summary>
        /// Certain API Calls limit the amount of data that gets returned, on each call. 
        /// When the returned data is limited, use the ContinuationToken to return the next
        /// set of data. If the Continuation Token is null or an empty string, then all
        /// data has been returned. 
        /// </summary>
        public string ContinuationToken { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets or Sets the raw body returned by the Rest API Call.
        /// </summary>
        public JToken Body {
            get { return body; }
            internal set {
                body = value;
                this.ConvertBodyToValue();
            }
        }

        /// <summary>
        /// Gets or Sets TimeSpan of API call
        /// </summary>
        public TimeSpan TimeToRun { get; internal set; } = TimeSpan.Zero;

        /// <summary>
        /// Function responsible for each concrete implementation to convert the Body, which
        /// is a JToken object, into the Value, which is of type T.
        /// </summary>
        protected virtual void ConvertBodyToValue() {
            Value = Body.ToObject<T>();
        }
    }
}
