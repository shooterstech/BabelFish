using System.ComponentModel;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Scopos.BabelFish.Requests;
using Scopos.BabelFish.Responses;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.Responses.Authentication.Credentials;
using Scopos.BabelFish.Responses.AttributeValueAPI;
using Newtonsoft.Json.Linq;
using NLog;
using Scopos.BabelFish.Runtime.Authentication;
using Scopos.BabelFish.DataModel;
using Amazon.Runtime;
using System.Runtime.CompilerServices;

namespace Scopos.BabelFish.APIClients {
    public abstract class APIClient {

        /// <summary>
        /// The name (key value) to use in http requests for the x api -key
        /// </summary>
        public const string X_API_KEY_NAME = "x-api-key";

        /// <summary>
        /// Standard json serializer settings intended for use while deserializing json to object model.
        /// Will ignore any json values that are null, and instead use the default value of the property.
        /// </summary>
        public static JsonSerializer DeSerializer = new JsonSerializer(  ) { NullValueHandling = NullValueHandling.Ignore };

        private readonly Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public HttpClient httpClient = new HttpClient();

        protected APIClient( string xapikey ) {
            this.XApiKey = xapikey;
            this.ApiStage = APIStage.PRODUCTION;

            logger.Info( $"BablFish API instantiated for {ApiStage}." );
        }

        protected APIClient( string xapikey, APIStage apiStage ) {
            this.XApiKey = xapikey;
            this.ApiStage = apiStage;

            logger.Info( $"BablFish API instantiated for {ApiStage}." );
        }

        #region properties

        [Obsolete( "What is this used for?" )]
        private DateTime? ContinuationToken = null;

        /// <summary>
        /// Users x-api-key for valid AWS access
        /// </summary>
        public string XApiKey { get; set; }

        /// <summary>
        /// ApiStage may be used to test in different development stages, e.g. production, beta, alpha.
        /// </summary>
        public APIStage ApiStage { get; protected set; }

        #endregion properties

        #region Methods
        protected async Task CallAPIAsync<T>( Request request, Response<T> response ) where T : BaseClass {
			// Get Uri for call
			string uri = $"https://{request.SubDomain.SubDomainNameWithStage()}.scopos.tech/{ApiStage.Description()}{request.RelativePath}?{request.QueryString}#{request.Fragment}".Replace( "?#", "" );

			DateTime startTime = DateTime.Now;

			//Check if there is a cached response we can use first. 
			ResponseIntermediateObject cachedResponse = null;
			if (!IgnoreLocalCache && !request.IgnoreLocalCache && request.HttpMethod == HttpMethod.Get && ResponseCache.CACHE.TryGetResponse( request, out cachedResponse )) {

                response.StatusCode = HttpStatusCode.OK;
                response.MessageResponse = cachedResponse.MessageResponse.Clone();
                response.MessageResponse.Message.Add( "Cached Response" );
                response.Body = cachedResponse.Body;
				response.TimeToRun = DateTime.Now - startTime;
                response.CachedResponse = true;

				logger.Debug( $"Returning a cached Response for {request}." );
                return;
			}


            try {

                HttpResponseMessage responseMessage = new HttpResponseMessage();

                HttpRequestMessage requestMessage;
                using (requestMessage = new HttpRequestMessage( request.HttpMethod, uri )) {
                    //Add in the x-api-key. Note that the request object *could* override it's value
                    if (!string.IsNullOrEmpty( this.XApiKey ))
                        requestMessage.Headers.Add( "x-api-key", XApiKey );

                    //Add in the headers to the request
                    if (request.HeaderKeyValuePairs != null)
                        foreach (var keyValuePair in request.HeaderKeyValuePairs)
                            requestMessage.Headers.Add( keyValuePair.Key, keyValuePair.Value );

                    //If we are making an authenticated call, update the temporary IAM credentials.
                    if (request.RequiresCredentials)
                        await request.Credentials.GenerateIAMCredentialsAsync();

                    /*
                     * Technically, HTTP GET methods can have a body / content. However, some versions of .net don't support it.
                     * So only add .Content on non GET calls
                     * https://stackoverflow.com/questions/3981564/cannot-send-a-content-body-with-this-verb-type
                     */
                    if (request.HttpMethod != HttpMethod.Get )
                        requestMessage.Content = request.PostParameters;

                    //DAMN THE TORPEDOES FULL SPEED AHEAD (aka make the rest api call)
                    logger.Info( $"Calling {request} on {uri}." );
					
                    startTime = DateTime.Now;
					if (request.RequiresCredentials) {
                        if (request.Credentials == null)
                            throw new AuthenticationException( $"Attempting to make an authenticated call with null credentials" );
                        //If we are making an authenticated call, use the exstention method from https://github.com/FantasticFiasco/aws-signature-version-4 to sign the request.
                        responseMessage = await httpClient.SendAsync(
                            requestMessage,
                            AuthenticationConstants.AWSRegion,
                            AuthenticationConstants.AWSServiceName,
                            request.Credentials.ImmutableCredentials );
                    } else {
                        responseMessage = await httpClient.SendAsync( requestMessage );
                    }

                    response.TimeToRun = DateTime.Now - startTime;
                    logger.Info( $"{request} has returned with {responseMessage.StatusCode} in {response.TimeToRun.TotalSeconds:f4} seconds." );
                }

                response.StatusCode = responseMessage.StatusCode;

                using (Stream s = await responseMessage.Content.ReadAsStreamAsync())
                using (StreamReader sr = new StreamReader( s ))
                using (JsonReader reader = new JsonTextReader( sr )) {
                    var apiReturnJson = JObject.ReadFrom( reader );
                    try {

                        //TODO: Do something with invalid data format from Forbidden....
                        if (responseMessage.StatusCode != HttpStatusCode.Forbidden)
                            response.MessageResponse = apiReturnJson.ToObject<MessageResponse>( DeSerializer );

                        if (responseMessage.IsSuccessStatusCode)
                            response.Body = apiReturnJson;

                        //Log errors set in calls
                        if (response.MessageResponse.Message.Count > 0)
                            logger.Error( "Processing Call Error {processingerror}", string.Join( "; ", response.MessageResponse.Message ) );

                    } catch (Exception ex) {
                        throw new Exception( $"Error parsing return json: {ex.ToString()}" );
                    }
                }

                if (responseMessage.IsSuccessStatusCode ) {
                    //Caching is only valid for GET calls
                    if (request.HttpMethod == HttpMethod.Get) {
                        cachedResponse = new ResponseIntermediateObject() {
                            StatusCode = response.StatusCode,
                            MessageResponse = response.MessageResponse.Clone(),
                            Request = request,
                            Body = response.Body,
                            ValidUntil = response.GetCacheValueExpiryTime()
                        };

                        ResponseCache.CACHE.SaveResponse( cachedResponse );
                    }
                } else {
                    logger.Error( $"API error with: {responseMessage.ReasonPhrase}"  );
                }

            } catch (Exception ex) {
                
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.MessageResponse.Message.Add( $"API Call failed: {ex.Message}" );
                response.MessageResponse.ResponseCodes.Add( HttpStatusCode.InternalServerError.ToString() );
                logger.Fatal( ex, "API Call failed: {failmsg}", ex.Message );
            }
		}

		/// <summary>
		/// Indicates if the local response cache should be ignored and always 
		/// make the request to the Rest API.
		/// The default value is true. Which means if an API Client wants to use local
		/// cached values, it must be enabled (set to false) within the concrete API Client.
		/// The option to ignore local cache can either be wet at the API Client level, or on a per request level. Cached responses are only valid for HttpMethod GET calls.
		/// 
		/// To enable cache for a API call two things needs to happen. First the concrete
		/// APIClient needs to enabled caching response by setting .IgnoreLocalCache to false.
		/// Second, each request object must enable it by overridding GetCacheValueExpiryTime
		/// to a value in the future.
		/// </summary>
		public bool IgnoreLocalCache { get; set; } = true;
		#endregion Methods
	}
}