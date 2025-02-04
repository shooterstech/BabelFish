using System.ComponentModel;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using Scopos.BabelFish.Requests;
using Scopos.BabelFish.Responses;
using Scopos.BabelFish.Helpers;
using NLog;
using Scopos.BabelFish.Runtime;
using Scopos.BabelFish.Runtime.Authentication;
using Scopos.BabelFish.DataModel;
using Scopos.BabelFish.Converters;
using System.Diagnostics;

namespace Scopos.BabelFish.APIClients {
    public abstract class APIClient<T> {
        //APIClient is being marked as a generic abstract class, so each concrete instance get's their own set of static properties and
        //variables declared within APIClient. Specifically LocalStorageDirectory.
        //https://stackoverflow.com/questions/3542171/c-sharp-abstract-class-static-field-inheritance

        /// <summary>
        /// The name (key value) to use in http requests for the x api -key
        /// </summary>
        public const string X_API_KEY_NAME = "x-api-key";

        /// <summary>
        /// Keep track of how many times this API Client is called. Will also update the sitewite statistics class.
        /// </summary>
        public static APIClientStatistics Statistics { get; private set; } = new APIClientStatistics();

        /// <summary>
        /// Standard json serializer settings intended for use while deserializing json to object model.
        /// Will ignore any json values that are null, and instead use the default value of the property.
        /// </summary>
        /// <remarks>Newtonsoft.json used NullValueHandling = NullValueHandling.Ignore </remarks>
        public static G_STJ.JsonSerializerOptions DeserializerOptions = new();

        private readonly Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public HttpClient httpClient = new HttpClient();

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        protected APIClient() {

            Settings.CheckXApiKey();

            this.ApiStage = APIStage.PRODUCTION;

            logger.Info( $"BablFish {GetType()} API instantiated for {ApiStage}." );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        protected APIClient( APIStage apiStage ) {
            Settings.CheckXApiKey();

            this.ApiStage = apiStage;

            logger.Info( $"BablFish {GetType()} API instantiated for {ApiStage}." );
        }

        #region properties

        /// <summary>
        /// ApiStage may be used to test in different development stages, e.g. production, beta, alpha.
        /// </summary>
        public APIStage ApiStage { get; protected set; }

        #endregion properties

        #region Methods
        protected async Task CallAPIAsync<T>( Request request, Response<T> response ) where T : BaseClass, new() {
            // Get Uri for call
            string uri = $"https://{request.SubDomain.SubDomainNameWithStage()}.scopos.tech/{ApiStage.Description()}{request.RelativePath}?{request.QueryString}#{request.Fragment}".Replace( "?#", "" );

            DateTime startTime = DateTime.Now;

            //Check if there is a cached response we can use first. 
            ResponseIntermediateObject cachedResponse = null;
            if (!IgnoreInMemoryCache && !request.IgnoreInMemoryCache && request.HttpMethod == HttpMethod.Get && ResponseCache.CACHE.TryGetResponse( request, out cachedResponse )) {

                response.StatusCode = HttpStatusCode.OK;
                //response.MessageResponse = cachedResponse.MessageResponse.Copy();
                //response.MessageResponse.Message.Add( "In memory cached response" );
                //var stopWatch = Stopwatch.StartNew();
                response.Body = cachedResponse.Body;
                response.TimeToRun = DateTime.Now - startTime;
                //stopWatch.Stop();
                response.InMemoryCachedResponse = true;

                logger.Info( $"Returning a in-memory cached Response for {request}." );
                return;
            }

            //If enabled by the concrete API Client, check if we can read the value from the local file system.
            if (!IgnoreFileSystemCache && !request.IgnoreFileSystemCache && request.HttpMethod == HttpMethod.Get) {

                var fileSystemReadResponse = await TryReadFromFileSystemAsync( request ).ConfigureAwait( false );

                if (fileSystemReadResponse.Item1) {
                    response.StatusCode = HttpStatusCode.OK;
                    //response.MessageResponse.Message.Add( "Read from file system response" );
                    response.Body = fileSystemReadResponse.Item2.Body;
                    response.TimeToRun = DateTime.Now - startTime;
                    response.FileSystemCachedResponse = true;

                    ResponseCache.CACHE.SaveResponse( fileSystemReadResponse.Item2 );
                    logger.Info( $"Returning a file system cached Response for {request}." );
                    return;
                }
            }

            //jsonAsString is practically used only for debugging
            string jsonAsString = "";

            try {

                HttpResponseMessage responseMessage = new HttpResponseMessage();

                HttpRequestMessage requestMessage;
                using (requestMessage = new HttpRequestMessage( request.HttpMethod, uri )) {
                    //Add in the x-api-key. Note that the request object *could* override it's value
                    if (!string.IsNullOrEmpty( Settings.XApiKey ))
                        requestMessage.Headers.Add( "x-api-key", Settings.XApiKey );

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
                    if (request.HttpMethod != HttpMethod.Get)
                        requestMessage.Content = request.PostParameters;

                    //DAMN THE TORPEDOES FULL SPEED AHEAD (aka make the rest api call)
                    logger.Info( $"Calling {request} on {uri}." );
                    Statistics.Increment();

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

                    logger.Info( $"{request} has returned with {responseMessage.StatusCode} in {response.TimeToRun.TotalSeconds:f4} seconds." );
                }

                response.StatusCode = responseMessage.StatusCode;

                using (Stream s = await responseMessage.Content.ReadAsStreamAsync())
                using (StreamReader sr = new StreamReader( s )) {
                    /*
                     * EKA Note Jan 2025: There are faster ways of parsing the stream into an object. However, by capturing the json (which slows things down)
                     * it makes troubleshooting much easier. Any by saving the JsonDocument in .Body, makes reusing response in a cache easier.
                     */
                    jsonAsString = sr.ReadToEnd();
                    //var stopWatch = Stopwatch.StartNew();
                    response.Body = G_STJ.JsonDocument.Parse( jsonAsString );
                    response.TimeToRun = DateTime.Now - startTime;
                    //stopWatch.Stop();

                    G_STJ.JsonElement messageArray;
                    if ( response.Body.RootElement.TryGetProperty( "Message", out messageArray ) && messageArray.ValueKind == G_STJ.JsonValueKind.Array ) {
                        foreach (var message in messageArray.EnumerateArray()) {
                            response.MessageResponse.Message.Add( message.GetString() );
                        }
                    }
                }

                if (responseMessage.IsSuccessStatusCode) {
                    //Caching is only valid for GET calls
                    if (request.HttpMethod == HttpMethod.Get) {
                        cachedResponse = new ResponseIntermediateObject() {
                            StatusCode = response.StatusCode,
                            //MessageResponse = response.MessageResponse.Copy(),
                            Request = request,
                            Body = response.Body,
                            ValidUntil = response.GetCacheValueExpiryTime()
                        };

                        ResponseCache.CACHE.SaveResponse( cachedResponse );
                    }
                } else {
                    logger.Error( $"API error with: {responseMessage.ReasonPhrase}" );
                    logger.Debug( jsonAsString );
                }

            } catch (Exception ex) {

                response.StatusCode = HttpStatusCode.InternalServerError;
                //response.MessageResponse.Message.Add( $"API Call failed: {ex.Message}" );
                logger.Fatal( ex, "API Call failed: {failmsg}", ex.Message );
                logger.Debug( jsonAsString );
            }
        }

        /// <summary>
        /// The directory that BabelFish may use to read and store cached responses. 
        /// Because APIClient is a generic class, each concret instance of APIClient get's gtheir own static
        /// property LocalStoreDirectory. https://stackoverflow.com/questions/3542171/c-sharp-abstract-class-static-field-inheritance
        /// </summary>
        public static DirectoryInfo? LocalStoreDirectory { get; set; }

        protected virtual async Task<Tuple<bool, ResponseIntermediateObject?>> TryReadFromFileSystemAsync( Request request ) {

            //Default behavior is not to try and read from the file system.

            return new Tuple<bool, ResponseIntermediateObject?>( false, null );
        }


        /// <summary>
        /// Indicates if the local response cache should be ignored and always 
        /// make the request to the Local File System and then to the Rest API.
        /// The default value is true. Which means if an API Client wants to use local
        /// cached values, it must be enabled (set to false) within the concrete API Client.
        /// The option to ignore in memory cache can either be set at the API Client level, or on a per request level. Cached responses are only valid for HttpMethod GET calls.
        /// 
        /// To enable cache for a API call two things needs to happen. First the concrete
        /// APIClient needs to enabled caching response by setting .IgnoreInMemoryCache to false.
        /// Second, each response object must enable it by overridding GetCacheValueExpiryTime
        /// to a value in the future.
        /// </summary>
        public bool IgnoreInMemoryCache { get; set; } = true;

        /// <summary>
        /// Indicates if the local file system cache should be ignored and always pass through to making the
        /// call to the Rest API.
        /// The option to ignore file system cache can either be set at the API Client level, or on a per request level. Cached responses are only valid for HttpMethod GET calls.
        /// 
        /// To enable cache for anAPI call two things must happen. First, the concrete APIClient needs
        /// to enable file system cache by setting .IgnoreFileSystemCache to false. Second the client's
        /// LocalStoreDirectory must be set.
        /// </summary>
        public bool IgnoreFileSystemCache { get; set; } = true;
		#endregion Methods
	}
}