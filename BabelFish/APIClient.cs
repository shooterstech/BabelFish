using System.ComponentModel;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using BabelFish.Requests;
using BabelFish.Responses;
using BabelFish.Helpers;
using BabelFish.External;
using Newtonsoft.Json.Linq;
using NLog;

namespace BabelFish {
    public abstract class APIClient {

        protected APIClient(string xapikey)
        {
            this.XApiKey = xapikey;
            this.ApiStage = APIStage.PRODUCTION;

            logger.Info("BablFish API instantiated with x-api-key: {key}", XApiKey);
        }

        #region properties
        private JsonSerializer serializer = new JsonSerializer();
        private readonly Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Environment Enums
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum APIStage
        {
            [Description("")]
            [EnumMember(Value = "")]
            BLANK,
            [Description("alpha")]
            [EnumMember(Value = "/alpha")]
            ALPHA,
            [Description("beta")]
            [EnumMember(Value = "/beta")]
            BETA,
            [Description("production")]
            [EnumMember(Value = "/production")]
            PRODUCTION
        }

        /// <summary>
        /// Users x-api-key for valid AWS access
        /// </summary>
        public string XApiKey { get; set; }

        /// <summary>
        /// Environment - AWS Api {stage}
        /// </summary>
        public APIStage ApiStage { get; set; }

        /// <summary>
        /// SubDomain - AWS Api subdomain identifier
        /// </summary>
        protected string SubDomain { get; set; } = "api-stage";
        #endregion properties

        #region Methods
        protected async Task CallAPI<T>(Request request, Response<T> response)
        {
            string uri = string.Empty;
            if (response is GetExternalResponse)
            {
                uri = request.RelativePath;
            }
            else
            {
                uri =
                    $"https://{SubDomain}.orionscoringsystem.com{EnumHelper.GetAttributeOfType<EnumMemberAttribute>(ApiStage).Value}{request.RelativePath}?{request.QueryString}#{request.Fragment}";
            }

            try
            {
                DateTime startTime = DateTime.Now;
                HttpResponseMessage responseMessage;
                if (response is GetExternalResponse)
                {
                    responseMessage =await httpClient.GetAsync(uri);
                }
                else
                {
                    responseMessage = 
                    await httpClient.GetAsyncWithHeaders(uri,
                    new Dictionary<string, string>() { { "x-api-key", XApiKey } }).ConfigureAwait(false);
                    //TODO: Future check if other calls require additional headers, threw Dict here to get basics running
                }

                response.StatusCode = responseMessage.StatusCode;
                
                logger.Info("API Fetched status: {statuscode} || uri: {url}", responseMessage.StatusCode, uri);

                //Convert the returned body to an object of type T
                var returnedJson = responseMessage.Content.ReadAsStringAsync().Result;

                response.Value = JsonConvert.DeserializeObject<T>(returnedJson);
                response.Body = returnedJson.ToString();

                // Override message format for successful parse
                if (responseMessage.StatusCode == HttpStatusCode.Forbidden)
                    returnedJson = returnedJson.Replace("\"Forbidden\"", "[\"Forbidden\"]");
                response.MessageResponse = JsonConvert.DeserializeObject<MessageResponse>(returnedJson);

                if (!responseMessage.IsSuccessStatusCode)
                    logger.Error("API error with: {errorphrase}", responseMessage.ReasonPhrase);

                TimeSpan ts = DateTime.Now - startTime;
                response.TimeToRun = $"{ts.Minutes}:{ts.Seconds}:{ts.Milliseconds}";
            }
            catch (Exception ex)
            {
                response.Body = $"API Call failed: {ex.Message}";
                logger.Fatal(ex, "API Call failed: {failmsg}", ex.Message);
            }
            //TODO: Found this recommended but do not want to shut down ongoing instance...onUnload? NLog.LogManager.Shutdown();
        }
        #endregion Methods
    }
}