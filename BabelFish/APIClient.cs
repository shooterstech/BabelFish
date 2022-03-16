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
using Newtonsoft.Json.Linq;
using NLog;

namespace BabelFish {
    public abstract class APIClient {

        protected APIClient(string xapikey)
        {
            this.XApiKey = xapikey;
            this.ApiStage = APIStage.PRODUCTION;
            this.SubDomain = SubDomains.APISTAGE;

            logger.Info("BablFish API instantiated");
        }
        protected APIClient(string xapikey, Dictionary<string, string>? CustomUserSettings = null) : this(xapikey)
        {
            if (CustomUserSettings != null)
                SettingsHelper.IncomingUserSettings = CustomUserSettings;
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
            APISTAGE,
            [Description("authapi-stage")]
            [EnumMember(Value = "authapi-stage")]
            AUTHAPISTAGE
        }

        /// <summary>
        /// UserSettings Enums
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum AuthEnums
        {
            [Description("")]
            [EnumMember(Value = "")]
            BLANK,
            [Description("User Name")]
            [EnumMember(Value = "username")]
            UserName,
            [Description("Password")]
            [EnumMember(Value = "password")]
            PassWord
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

        /// <summary>
        /// Users Access Key for valid AWS access
        /// </summary>
        private string AuthAccessKey { get; set; } = string.Empty;

        /// <summary>
        /// Users Secret Key for valid AWS access
        /// </summary>
        private string AuthSecretKey { get; set; } = string.Empty;

        /// <summary>
        /// Users Session Token for valid AWS access
        /// </summary>
        private string AuthSessionToken { get; set; } = string.Empty;

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
                    SubDomain = SubDomains.AUTHAPISTAGE;
                else
                    SubDomain = SubDomains.APISTAGE;

                uri = $"https://{EnumHelper.GetAttributeOfType<EnumMemberAttribute>(SubDomain).Value}.orionscoringsystem.com{EnumHelper.GetAttributeOfType<EnumMemberAttribute>(ApiStage).Value}{request.RelativePath}?{request.QueryString}#{request.Fragment}".Replace("?#", "");
            }

            try {
                DateTime startTime = DateTime.Now;
                HttpResponseMessage responseMessage = new HttpResponseMessage();

                if (!FunctionOptions["UseShootersTechUri"])
                    responseMessage = await httpClient.GetAsync(uri).ConfigureAwait(false);
                else {
                    Dictionary<string, string> AssembledHeaders = new Dictionary<string, string>();
                    if (FunctionOptions["UseAuth"]) {
                        AuthAccessKey = "ASIA2HUPLTYW5XSNZ2NK";
                        AuthSecretKey = "N1HnfSOOG77i7faP48NbpqN2QrFcrZF8V2eh+Sef";
                        AuthSessionToken =
                            "IQoJb3JpZ2luX2VjEAQaCXVzLWVhc3QtMSJHMEUCIFIeO6Jpc7zUaS/76c0bBdq+pV1qDiV2eQMY23n/vO2JAiEAorS9rHWHqii+nVk18qn8Kyxh0v/o2LVopmpaYS2zZO0qxAQIfBAAGgw3MDM2MDE1NDg4NDUiDMnv4ikqoiMc+InYciqhBAHToqtQlDVs/KvR5dnYpQIlP1/xSGNZOK74osDxYKUg6K1KAZyRF3wFiJ14V5LXK5GMx5Yc6lvGbwV+jh+O8QaeCxrWNohJgmz8Zo+ShSPDsxsZ7hMdjORwsTha3KKzUtIixN5k0u1WrWJ0L/3a0SAJk0GyY7ZVoDfTYNvy3fke7dEllWh1PGPk3JG9Li8LZQsGkh1xMMZUxyfFuFVr3SI2bj3Ry2fRpIfYNtgjpYMnkDXcL783n/5fNlUJTo0EY5YdQBMJHLXnXj7rhIb2SoeUklGIiH8pBrZHdDryysYZ4j/Ph/7ZJ+si+i8lfrlc7OgRiuctbNVP20yQOGIw3lta9mZBPV7uWTzc827tZwC+N4/m158c6ZQn1hao8tIUMhPKQyQnX9dFMWGxRGOqHtVNFDONdQ+7kzALpiMD5gwB/wBjvKP4VKaPcda6mrl0KJ6bs7WRK4BC8IzrYLn42UtxCk9UC86hZcpVOZGW6kJUnoVWRC8xp+pXdKUvlGrmvNIJA+zTS4/xLO1KClj4domi2nwI6OVcdtGtfgbDV2V8QYd1LTbySLhiD8Ubyw/Q8UrqVPUWVDYg8c1xb4/OBrI3F7KHRmFkfxRynKPS7NHzoJR+SimpjAWj9CjfntMxKl3Uj4k0RRu3HHtTY8WiXUq0f0jC+U4c1FdgWOOa8nj/UJXocU+42VxqsiQUPOVMjM/d4CWPzh6a6ZPCNUw+x7WaMKHLw5EGOoUCRfhSGfIvybsc7nKbKzdT8/ooc/Ytwn5AtVOPSEIREnC+OZNa0KvmN4qQhqxHrRF5rcPvwPFn0fM0cBDgWeQEfxOjRjGBNRmfj+fM7Stu4wzTulV0QcyzCpVhOhrNK3eYNGRB5dvkBpi1McWd8/1VnGQstV4jYeOXDaR2IaW1ZZzuXhlx1tcIyUsLMld7iK5uhezWKkqh0HrQ2u+DMyMbZB+VrCqbtSBSyZeAhM//FgzCT7tjHzOypOdDeDCJjxkLM8XViActEPPL5meyoEV1//i84X9G9iDbFymhJTN//QoOclUJMNtRkyoxytgdXUOdzg4/+mx6U3hlbYoq9myFdhLKIFPk";

                        AwsSigner AwsSignature = new AwsSigner(AuthAccessKey, AuthSecretKey, uri, AuthSessionToken);
                        responseMessage = await AwsSignature.GetAws4Signature("AwsSignatureV4RichW")
                            .ConfigureAwait(false); // AwsSignatureVersion4  Aws4RequestSigner  AwsSignatureV4RichW
                        //responseMessage = await httpClient.SendAsync(AuthRequest).ConfigureAwait(false);
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
            //TODO: Found this recommended but do not want to shut down ongoing instance...onUnload? NLog.LogManager.Shutdown();
        }
        #endregion Methods
    }
}