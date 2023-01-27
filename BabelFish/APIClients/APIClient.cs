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

namespace Scopos.BabelFish.APIClients {
    public abstract class APIClient {

        /// <summary>
        /// The name (key value) to use in http requests for the x api -key
        /// </summary>
        public const string X_API_KEY_NAME = "x-api-key";

        private JsonSerializer serializer = new JsonSerializer();
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

        [Obsolete("What is this used for?")]
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
        protected async Task CallAPI<T>(Request request, Response<T> response) where T : BaseClass
        {
            // Get Uri for call
            string uri = $"https://{request.SubDomain.SubDomainNameWithStage()}.orionscoringsystem.com/{ApiStage.Description()}{request.RelativePath}?{request.QueryString}#{request.Fragment}".Replace("?#", "");

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

                    /*
                    if ( request.PostParameters != null ) {
                        requestMessage.Content= request.PostParameters;
                    }
                    */

                    //If we are making an authenticated call, update the temporary IAM credentials.
                    if (request.RequiresCredentials)
                        request.Credentials.GenerateIAMCredentials();

                    //DAMN THE TORPEDOES FULL SPEED AHEAD (aka make the rest api call)
                    logger.Info( $"Calling {request} on {uri}.");
                    DateTime startTime = DateTime.Now;

                    if (request.RequiresCredentials) {
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

                using (Stream s = responseMessage.Content.ReadAsStreamAsync().Result)
                using (StreamReader sr = new StreamReader(s))
                using (JsonReader reader = new JsonTextReader(sr)) {
                    var apiReturnJson = JObject.ReadFrom(reader);
                    try {

                        //TODO: Do something with invalid data format from Forbidden....
                        if (responseMessage.StatusCode != HttpStatusCode.Forbidden)
                            response.MessageResponse = apiReturnJson.ToObject<MessageResponse>();

                        if (responseMessage.IsSuccessStatusCode)
                            response.Body = apiReturnJson;
                        
                        //Log errors set in calls
                        if ( response.MessageResponse.Message.Count > 0)
                            logger.Error("Processing Call Error {processingerror}", string.Join("; ", response.MessageResponse.Message));

                    } catch (Exception ex) {
                        throw new Exception($"Error parsing return json: {ex.ToString()}");
                    }
                }

                if (!responseMessage.IsSuccessStatusCode)
                    logger.Error("API error with: {errorphrase}", responseMessage.ReasonPhrase);

            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.MessageResponse.Message.Add($"API Call failed: {ex.Message}");
                response.MessageResponse.ResponseCodes.Add(HttpStatusCode.InternalServerError.ToString());
                logger.Fatal(ex, "API Call failed: {failmsg}", ex.Message);
            }
        }
        #endregion Methods
    }
}