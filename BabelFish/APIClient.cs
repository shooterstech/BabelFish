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
using BabelFish.Responses.DefinitionAPI;
using Newtonsoft.Json.Linq;
using NLog;

namespace BabelFish {
    public abstract class APIClient {

        protected APIClient(string xapikey)
        {
            this.XApiKey = xapikey;
            this.ApiStage = APIStage.PRODUCTION;
            this.SubDomain = SubDomains.APISTAGE;

            logger.Info("BablFish API instantiated with x-api-key: {key}", XApiKey);
        }
        protected APIClient(string xapikey, string userName, string passWord) : this(xapikey)
        {
            this.AuthUsername = userName;
            this.AuthPassword = passWord;
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
        /// Users Name for valid AWS access
        /// </summary>
        private string AuthUsername { get; set; } = string.Empty;

        /// <summary>
        /// Users Password for valid AWS access
        /// </summary>
        private string AuthPassword { get; set; } = string.Empty;

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
        protected async Task CallAPI<T>(Request request, Response<T> response, bool WithAuthentication = false)
        {
            // Setup workflow conditions
            Dictionary<string, bool> FunctionOptions = new Dictionary<string, bool>();
            FunctionOptions.Add("UseAuth", WithAuthentication);
            FunctionOptions.Add("UseShootersTechUri", (response is GetExternalResponse) ? false : true);

            // Get Uri for call
            string uri = string.Empty;
            if (!FunctionOptions["UseShootersTechUri"])
                uri = request.RelativePath;
            else
            {
                if (WithAuthentication)
                    SubDomain = SubDomains.AUTHAPISTAGE;
                else
                    SubDomain = SubDomains.APISTAGE;

                uri = $"https://{EnumHelper.GetAttributeOfType<EnumMemberAttribute>(SubDomain).Value}.orionscoringsystem.com{EnumHelper.GetAttributeOfType<EnumMemberAttribute>(ApiStage).Value}{request.RelativePath}?{request.QueryString}#{request.Fragment}".Replace("?#", "");
            }

            try
            {
                DateTime startTime = DateTime.Now;
                HttpResponseMessage responseMessage = new HttpResponseMessage();

                if (!FunctionOptions["UseShootersTechUri"])
                    responseMessage = await httpClient.GetAsync(uri).ConfigureAwait(false);
                else
                {
                    Dictionary<string, string> AssembledHeaders = new Dictionary<string, string>();
                    if (FunctionOptions["UseAuth"])
                    {
                        //TESTING VALUES RETRIEVED FROM GetCredentials
                        //{
                        //    "Title": "",
                        //    "Message": [],
                        //    "ResponseCodes": [],
                        //    "AccessKeyId": "ASIA2HUPLTYWVNXWV7MM",
                        //    "SecretKey": "ps/x+8ZryaErLWO69qHio6EY/hhNwZ9ieeaHdyq9",
                        //    "SessionToken": "IQoJb3JpZ2luX2VjEKv//////////wEaCXVzLWVhc3QtMSJHMEUCIGI1S7BGR2WYzH9DzodCDXATytHmdGQjYYYidJhJbSrjAiEAodoaM3qeIHGn9wazwzdhp00qHvbeCzhaLRZFgYYk+0oqzQQI9P//////////ARAAGgw3MDM2MDE1NDg4NDUiDMHw4eDqDjkpdwdRxiqhBBicZtQtKxbDWi2LXLYYU7NBlMhiejC3u7c6OJ9cGUYFQvITvez30j/pxUufT8qIWFMQQB9DMEqKoJrvGTh+BVqutzIR/mcxOGR9vVd0FDn7NyuKgVofiPfSPgZFVnu1MGSi3C3/KsWDMrStmKrHcw4pttFd3eM+mIgXfVK7YpZcy9BYYDdhrNe3OEk6tSYLlz0sHDFgmeXHVJ+bm3qCj4Om0MzR7nIRGETGJ07bfYyXDazTi5uMcnurW1kCdOqOgG7G9mn3o8PuouV+ueSruEG2yrGRY9sk0gwGZoYwClj7Mvtu/y+LFuFOfNsq+QEx7jq4zzD3v/Ehf2+l7YIwurL8b5lk/rwvSSoeaRfy06bUMOLfH67nuDlRgaEdeyMALqnqnmSY3wF/tq00r7u2brwh+5JpAs/TLcQ1puu7Hq67vW1qOxSMPI4xFyIQzdu6E++g50LrgHhcAOygAx05OK9JxMzkAeUIRfbfYGr41QbfF0CYZRlDLdtguhlUazRv33APGv1xszUNYA0TXmZcbzREmMBaS2GeXfzxlkoxhxiQaf6Y9GU3JOenTDjZq8n8wQQMZKsh56JuQewLfDsP3qkv3Oca17JnROLNloF5hWdqgdTnFJShmOYyV2aCUVW/wVtaLMHY+y72UrlttG5sndbkkqDRCYjdxV8b7fuN2d2IdymtM5e+rrwqFQbQr73xkDoHnFCo+NT142vf+5rmGpK6MIXRv5AGOoUCFHVxKs94gRinKvSZG5yIHDpfO4aPr0x4MKZnbVPNpvfpSyfMKp07vMo88yrcYC+vobkEh5hhYS9OVJLZPbSclz6ZiiLmSAz8rCJ6yF3iw60M+Y6AS3UOx1ydNebeNDqZAnnj5X2pGbO1iibTH7h3OFhJFKY3emHde22dCNPsZAphNQM6tUfIVka7FNkSNsJmfxkXJDa//MUF6Je+GAS+y3WrV31v2xEU161y2u9aFKqr1z8jaJ/LgiBhlKKW3YSqrmk+eZIPTVhCO/NsF2yKK/Gz8+fyuSXrzok1Y1xahZX7EEOv3jT0Ggg8CSnK3W9KLf3o8wlLvBrYm+ucuZrriUkuSDu5",
                        //    "Username": "test_dev_7@shooterstech.net",
                        //    "Password": "abcd1234"
                        //}
                        AuthAccessKey = "ASIA2HUPLTYWVNXWV7MM";
                        AuthSecretKey = "ps/x+8ZryaErLWO69qHio6EY/hhNwZ9ieeaHdyq9";
                        AuthSessionToken = "IQoJb3JpZ2luX2VjEKv//////////wEaCXVzLWVhc3QtMSJHMEUCIGI1S7BGR2WYzH9DzodCDXATytHmdGQjYYYidJhJbSrjAiEAodoaM3qeIHGn9wazwzdhp00qHvbeCzhaLRZFgYYk+0oqzQQI9P//////////ARAAGgw3MDM2MDE1NDg4NDUiDMHw4eDqDjkpdwdRxiqhBBicZtQtKxbDWi2LXLYYU7NBlMhiejC3u7c6OJ9cGUYFQvITvez30j/pxUufT8qIWFMQQB9DMEqKoJrvGTh+BVqutzIR/mcxOGR9vVd0FDn7NyuKgVofiPfSPgZFVnu1MGSi3C3/KsWDMrStmKrHcw4pttFd3eM+mIgXfVK7YpZcy9BYYDdhrNe3OEk6tSYLlz0sHDFgmeXHVJ+bm3qCj4Om0MzR7nIRGETGJ07bfYyXDazTi5uMcnurW1kCdOqOgG7G9mn3o8PuouV+ueSruEG2yrGRY9sk0gwGZoYwClj7Mvtu/y+LFuFOfNsq+QEx7jq4zzD3v/Ehf2+l7YIwurL8b5lk/rwvSSoeaRfy06bUMOLfH67nuDlRgaEdeyMALqnqnmSY3wF/tq00r7u2brwh+5JpAs/TLcQ1puu7Hq67vW1qOxSMPI4xFyIQzdu6E++g50LrgHhcAOygAx05OK9JxMzkAeUIRfbfYGr41QbfF0CYZRlDLdtguhlUazRv33APGv1xszUNYA0TXmZcbzREmMBaS2GeXfzxlkoxhxiQaf6Y9GU3JOenTDjZq8n8wQQMZKsh56JuQewLfDsP3qkv3Oca17JnROLNloF5hWdqgdTnFJShmOYyV2aCUVW/wVtaLMHY+y72UrlttG5sndbkkqDRCYjdxV8b7fuN2d2IdymtM5e+rrwqFQbQr73xkDoHnFCo+NT142vf+5rmGpK6MIXRv5AGOoUCFHVxKs94gRinKvSZG5yIHDpfO4aPr0x4MKZnbVPNpvfpSyfMKp07vMo88yrcYC+vobkEh5hhYS9OVJLZPbSclz6ZiiLmSAz8rCJ6yF3iw60M+Y6AS3UOx1ydNebeNDqZAnnj5X2pGbO1iibTH7h3OFhJFKY3emHde22dCNPsZAphNQM6tUfIVka7FNkSNsJmfxkXJDa//MUF6Je+GAS+y3WrV31v2xEU161y2u9aFKqr1z8jaJ/LgiBhlKKW3YSqrmk+eZIPTVhCO/NsF2yKK/Gz8+fyuSXrzok1Y1xahZX7EEOv3jT0Ggg8CSnK3W9KLf3o8wlLvBrYm+ucuZrriUkuSDu5";

                        AwsSigner AwsSignature = new AwsSigner(AuthAccessKey, AuthSecretKey, uri, AuthSessionToken);
                        responseMessage = await AwsSignature.GetAws4Signature("AwsSignatureVersion4").ConfigureAwait(false);
                        //responseMessage = await httpClient.SendAsync(AuthRequest).ConfigureAwait(false);
                    }
                    else
                    {
                        AssembledHeaders.Add("x-api-key", XApiKey);
                        responseMessage = await httpClient.GetAsyncWithHeaders(uri, AssembledHeaders).ConfigureAwait(false);
                    }
                }
                response.StatusCode = responseMessage.StatusCode;

                logger.Info("API Fetched status: {statuscode} || uri: {url}", responseMessage.StatusCode, uri);

                //Convert the returned body to an object of type T
                var returnedJson = responseMessage.Content.ReadAsStringAsync().Result;

                Dictionary<string, Attribute> tmp;
                //if (response is GetAttributeResponse)
                //    tmp = JsonConvert.DeserializeObject<Dictionary<string, Attribute>>(returnedJson);
                //else
                //returnedJson = "{\"v1.0:ntparc:Three-Position Air Rifle Type\":{\"HierarchicalName\":\"ntparc:Three-Position Air Rifle Type\",\"Owner\":\"OrionAcct001001\",\"RequiredAttributes\":[],\"Version\":\"1.1\",\"Fields\":[{\"Values\":[{\"Value\":\"Sporter\",\"Name\":\"Sporter\"},{\"Value\":\"Precision\",\"Name\":\"Precision\"}],\"DefaultValue\":\"Sporter\",\"FieldName\":\"Three-Position Air Rifle Type\",\"ValueType\":\"STRING\",\"FieldType\":\"CLOSED\"}],\"Description\":\"Three-Position Air Rifle Type\",\"DisplayName\":\"Three-Position Air Rifle Type\",\"SetName\":\"v1.0:ntparc:Three-Position Air Rifle Type\",\"Type\":\"ATTRIBUTE\",\"Designation\":[\"ATHLETE\",\"CLUB\",\"MATCH OFFICIAL\",\"TEAM\",\"TEAM OFFICIAL\",\"USER\"]}}";
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