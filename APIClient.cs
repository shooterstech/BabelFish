using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using BabelFish.Requests;
using BabelFish.Helpers;
using Newtonsoft.Json.Linq;
using NLog;

namespace BabelFish {
    public abstract class APIClient {

        protected APIClient(string xapikey)
        {
            this.XApiKey = xapikey;

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
        /// Control uri for Dev / Prod
        /// </summary>
        private bool _DeveloperMode = false;
        protected bool DeveloperMode
        {
            get { return _DeveloperMode; }
            set
            {
                _DeveloperMode = value;
                if (value)
                {
                    SubDomain = "api-stage";
                    ApiStage = APIStage.PRODUCTION;
                }
                else
                {
                    SubDomain = "api";
                    ApiStage = APIStage.BLANK;
                }

            }
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
        /// Subdomain - AWS Api subdomain identifier
        /// </summary>
        protected string SubDomain { get; set; } = "api";
        #endregion properties

        #region Methods
        protected async void CallAPI<T>(Request request, Response<T> response)
        {
            //TODO make the hostname a variable
            string uri =
                $"https://{SubDomain}.orionscoringsystem.com{EnumHelper.GetAttributeOfType<EnumMemberAttribute>(ApiStage).Value}{request.RelativePath}?{request.QueryString}#{request.Fragment}";

            try
            {
                HttpResponseMessage responseMessage = 
                    await httpClient.GetAsyncWithHeaders(uri,
                    new Dictionary<string, string>() { { "x-api-key", XApiKey } });
                //TODO: Future check if other calls require additional headers, threw Dict here to get basics running

                //HttpResponseMessage responseMessage = httpClient.GetAsync(uri).Result; //make it err, no x-api-key header
                response.StatusCode = responseMessage.StatusCode;
                logger.Info("API Fetched status: {statuscode} || uri: {url}", responseMessage.StatusCode, uri);

                //Convert the returned body to an object of type T
                if (responseMessage.IsSuccessStatusCode)
                {
                    using (Stream s = responseMessage.Content.ReadAsStreamAsync().Result)
                    using (StreamReader sr = new StreamReader(s))
                    using (JsonReader reader = new JsonTextReader(sr))
                    {
                        var returnedJson = JObject.ReadFrom(reader);

                        response.Value = returnedJson.ToObject<T>();

                        response.Body = returnedJson.ToString();
                    }
                }
                else
                {
                    response.Body = StringHelper.ErrorTextExpanded(responseMessage.StatusCode); // or just responseMessage.ReasonPhrase;
                    logger.Error(("API error with: {errorphrase}", responseMessage.ReasonPhrase));
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "API Call failed: {failmsg}", ex.Message);
                response.Body = $"API Call failed: {ex.Message}";
            }
            //NLog.LogManager.Shutdown();
        }
        #endregion Methods
    }
}