using System.Net;
using Scopos.BabelFish.DataModel;
using Scopos.BabelFish.Requests;

namespace Scopos.BabelFish.Responses
{
    /// <summary>
    /// Abstract class representing all Rest API Responses.
    /// A concret implementation of a Response class should coorespond to exactly one REST API method call.
    /// T is the type of data object expected returned by the REST API call.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Response<T>
        where T : BaseClass, new() {

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
		/// Returns the time that the response object is considered out of date and
        /// should not longer be used in a cached response. 
        /// 
        /// To enable cache for a API call two things needs to happen. First the concrete
        /// APIClient needs to enabled caching response by setting .IgnoreLocalCache to false.
        /// Second, each request object must enable it by overridding GetCacheValueExpiryTime
        /// to a value in the future.
		/// </summary>
		/// <returns></returns>
		protected internal virtual DateTime GetCacheValueExpiryTime() {
            //Return a default value indicating the cahce value has already expired.
            return DateTime.MinValue;
		}

        /// <summary>
        /// If true, indicates this response was from in memory cache, and not from an API call.
        /// </summary>
        public bool InMemoryCachedResponse { get; protected internal set; } = false;

        /// <summary>
        /// If true, indicates this response was from file system local storage, and not from an API call.
        /// </summary>
        public bool FileSystemCachedResponse { get; protected internal set; } = false;

		/// <summary>
		/// Gets or sets the MesageResponse *status* data object returned by the Rest API Call. The Message Response contains all of the standard 
        /// fields returned in a Scopos Rest API call, including Message and NextToken (if used). What it doesn't contain is the requested data model object.
		/// </summary>
		public MessageResponse MessageResponse
        {
            get;
            internal set;
        } = new MessageResponse();        

        /// <summary>
        /// Gets or sets the data object returned by the Rest API Call.
        /// </summary>
        public T Value
        {
            get;
            internal set;
        } = default( T );

        /// <summary>
        /// Gets or Sets the Status Code returned by the Rest API call.
        /// </summary>
        public HttpStatusCode StatusCode { get; internal set; }

        /// <summary>
        /// Raw body returned by the Rest API Call.
        /// </summary>
        private G_STJ.JsonDocument body = null;

        /// <summary>
        /// Gets or Sets the raw body returned by the Rest API Call.
        /// If the StatusCode is something other than OK (200), the value of Body will be invalid.
        /// </summary>
        public G_STJ.JsonDocument Body {
            get { return body; }
            internal set {
                body = value;
                this.ConvertBodyToValue();
            }
        }

        /// <summary>
        /// Total time of the request. Includes the time it takes to make the API call plus time to parse the returned JSON into object form.
        /// </summary>
        //  EKA NOTE Jan 2025: There is an argument that the TimeToRun shold only be the API call and not the time to parse the JSON into object form.
        //  As parsing json into object form can take a while, and maybe shouldn 't be included. I'm not sure which is better? 
        public TimeSpan TimeToRun { get; internal set; } = TimeSpan.Zero;

        /// <summary>
        /// Function responsible for each concrete implementation to convert the Body, which
        /// is a JToken object, into the Value, which is of type T.
        /// </summary>
        protected virtual void ConvertBodyToValue() {
            if (StatusCode == HttpStatusCode.OK)
                Value = G_STJ.JsonSerializer.Deserialize<T>( Body.RootElement, SerializerOptions.SystemTextJsonDeserializer );
            else 
                Value = new T();

            /*
            var nameOfClass = typeof( T ).Name;
            JsonElement body;
            if (Body.RootElement.TryGetProperty( nameOfClass, out body )) {
                Value = JsonSerializer.Deserialize<T>( body, SerializerOptions.APIClientSerializer );
            }
            */
        }

        /// <summary>
        /// The serialized text value returned by the Rest API. This value is only set if there was an error.
        /// </summary>
        public string Json { get; set; }

        public string ExceptionMessage { get; set; }
    }
}
