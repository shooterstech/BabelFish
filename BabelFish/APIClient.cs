using System.ComponentModel;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
//using System.Text.Json.Nodes; //COMMENT OUT FOR .NET Standard 2.0
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using BabelFish.Requests;
using BabelFish.Responses;
using BabelFish.Helpers;
using BabelFish.External;
using BabelFish.Responses.Credentials;
using BabelFish.Responses.GetSetAttributeValueAPI;
using Newtonsoft.Json.Linq;
using NLog;

namespace BabelFish {
    public abstract class APIClient {

        protected APIClient(string xapikey)
        {
            this.XApiKey = xapikey;
            this.ApiStage = APIStage.PRODUCTION;
            this.SubDomain = SubDomains.API_STAGE;

            logger.Info("BablFish API instantiated");
        }

        protected APIClient(string xapikey, Dictionary<string, string>? CustomUserSettings = null) : this(xapikey)
        {
            if (CustomUserSettings != null)
                SettingsHelper.IncomingUserSettings = CustomUserSettings;
            if (SettingsHelper.SettingIsNullOrEmpty("XApiKey"))
                SettingsHelper.UserSettings["XApiKey"] = xapikey;
        }

        #region properties
        private JsonSerializer serializer = new JsonSerializer();
        private readonly Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private DateTime? ContinuationToken = null;

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

        [JsonConverter(typeof(StringEnumConverter))]
        private enum SubDomains
        {
            [Description("")]
            [EnumMember(Value = "")]
            BLANK,

            [Description("api")]
            [EnumMember(Value = "api")]
            API,

            [Description("api-stage")]
            [EnumMember(Value = "api-stage")]
            API_STAGE,

            [Description( "authapi" )]
            [EnumMember( Value = "authapi" )]
            AUTHAPI,

            [Description("authapi-stage")]
            [EnumMember(Value = "authapi-stage")]
            AUTHAPI_STAGE,

            [Description("internalapi")]
            [EnumMember(Value = "internalapi")]
            INTERNAL,

            [Description( "internalapi-stage" )]
            [EnumMember( Value = "internalapi-stage" )]
            INTERNAL_STAGE
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
        private SubDomains SubDomain { get; set; }

        #endregion properties

        #region Methods
        protected async Task CallAPI<T>(Request request, Response<T> response)
        {
            // Setup workflow conditions
            Dictionary<string, bool> FunctionOptions = new Dictionary<string, bool>();
            FunctionOptions.Add("UseAuth", request.WithAuthentication);
            FunctionOptions.Add("UseShootersTechUri", (response is GetExternalResponse) ? false : true);

            // Get Uri for call
            string uri = string.Empty;
            if (!FunctionOptions["UseShootersTechUri"])
                uri = request.RelativePath;
            else
            {
                if (FunctionOptions["UseAuth"])
                {
                    SubDomain = SubDomains.AUTHAPI_STAGE;
                    if (response is GetValidateUserIDResponse)
                        ApiStage = APIStage.BETA;
                }
                else if (response is GetCredentialsResponse)
                {
                    SubDomain = SubDomains.INTERNAL;
                    ApiStage = APIStage.BLANK;
                }
                else
                    SubDomain = SubDomains.API_STAGE;

                uri = $"https://{EnumHelper.GetAttributeOfType<EnumMemberAttribute>(SubDomain).Value}.orionscoringsystem.com{EnumHelper.GetAttributeOfType<EnumMemberAttribute>(ApiStage).Value}{request.RelativePath}?{request.QueryString}#{request.Fragment}".Replace("?#", "");
            }

            try {
                DateTime startTime = DateTime.Now;
                HttpResponseMessage responseMessage = new HttpResponseMessage();

                if (!FunctionOptions["UseShootersTechUri"])
                    responseMessage = await httpClient.GetAsync(uri).ConfigureAwait(false);
                else 
                {
                    Dictionary<string, string> AssembledHeaders = new Dictionary<string, string>();
                    if (FunctionOptions["UseAuth"]) {
                        AwsSigner AwsSignature = new AwsSigner(uri, ContinuationToken);
                        responseMessage = await AwsSignature.GetAws4Signature().ConfigureAwait(false);
                        ContinuationToken = AwsSignature.ContinuationToken;
                    } else {
                        AssembledHeaders.Add("x-api-key", XApiKey);
                        responseMessage = await httpClient.GetAsyncWithHeaders(uri, AssembledHeaders)
                            .ConfigureAwait(false);
                    }
                }

                response.StatusCode = responseMessage.StatusCode;

                logger.Info("API Fetched status: {statuscode} || uri: {url}", responseMessage.StatusCode, uri);

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

                //EKA Note: Let's keep TimetoRun as a TimeSpan object, makes it easier to parse later if needed.
                response.TimeToRun = DateTime.Now - startTime;
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.MessageResponse.Message.Add($"API Call failed: {ex.Message}");
                response.MessageResponse.ResponseCodes.Add(HttpStatusCode.InternalServerError.ToString());
                logger.Fatal(ex, "API Call failed: {failmsg}", ex.Message);
            }
        }

        public bool UpdateAuthTokens(Dictionary<string,string> newTokens)
        {
            try
            {
                SettingsHelper.UpdateApplicationSettings(newTokens);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Dictionary<string, string> GetAuthTokens()
        {
            Dictionary<string, string> returnTokens = new Dictionary<string, string>();
            try
            {
                List<string> AuthTokens = new List<string>() { "UserName", "PassWord", "RefreshToken", "IdToken", "AccessToken", "DeviceToken" };
                foreach (string key in AuthTokens)
                    returnTokens.Add(key, SettingsHelper.UserSettings[key]);
            }finally { }

            return returnTokens;
        }
        #endregion Methods
    }
}