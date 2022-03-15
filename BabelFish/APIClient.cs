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
        protected APIClient(string xapikey, string userName = "", string passWord = "", Dictionary<string, string>? incomingUserSettings = null) : this(xapikey)
        {
            this.AuthUsername = userName;
            this.AuthPassword = passWord;
            if ( incomingUserSettings != null )
                SettingsHelper.IncomingUserSettings = incomingUserSettings;
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
                        AuthAccessKey = "ASIA2HUPLTYW2GDAGZMK";
                        AuthSecretKey = "Fg2OmZAkjYaIMcI/zbFRU8T3lTWWxO8fnhZh4bQm";
                        AuthSessionToken =
                            "IQoJb3JpZ2luX2VjEOz//////////wEaCXVzLWVhc3QtMSJHMEUCIGrrBLKgClqlako1SRuxByRC1lMafmgWuzeFHtKEjziRAiEA7sXHzpMpFYpKwFx45KKV1we6IAPCVMhCX89MM9MwgKQqxAQIZRAAGgw3MDM2MDE1NDg4NDUiDBeRVSyiuV+YmDtGHCqhBEz8RoWZo40ll1cxxJZvMAQ/fRUDnGzEYPoM0O+/f1fzqOn1k8a/9RlYN2zZsjNDno4Fx5gdByniK/v6H1ZL40IjfC0b24kpTky4lOd+Dhx+t8JjtPYCjTXWqy1ePGe8t2+ZZwBAxlMj0cXNBaFH2VUurNY92MfRLr1znpXndDtMNrdFryLsMxBehb0At5KQynYjho4N0UCYJtRQV/BH6ArmjVDLz3j1itF4eRUDerTZ/hLHSvUfxUGBdXl2HJEol+GP9ZNaGKSggKhrYzolZvTY1K0Ti5HmTr/mFDx52UQpxea6t/xvV96GgLze1muzCLG5zOoOIdTiQ+c+9jnrQl4AcvyPv38TvGx5+I8PtJpYB1B7Bt9+MntZJQjiJO0OnjowupHitbzZ2IWeJTaOo/1ajrXLnI2jX8Y1dwdjtkqB+NxCJgitJG26pCtvWBMcYYvcsK3u8sCmvcT8MV1ABGBrJ+PUNofAmIC2Y9hINLpGv6up5i56Ah3dfXmzdN3H714yqVByIktZtoNQKrltZovaioD0DJdD1WBbC+7ZVARlaGAMAUqo3iUnlUcGc8vEF+5bmyfPlQHAmxMkJa9StNjYuXt99o56VXiwOArT6oohytdbxNvNJbJKUA3Iy9xxHK5hSblttm66PmJPeXNNrFvAW9HqSNq6kQ4WkVkOzCY4o28yXjjTuD82Gp+GQhkfl73OCtJpqb7c/6kgD6U0H1VFMNqvvpEGOoUC4iXD5N/WkfYWVO1D7e6F411DU3esjkMrYmA7zbvSqaKzsPHkBMtrDRozaohrgmApM59h6m0MXDRw+QGkDG+NIonDtrLmf7XZwvRovGL+QyWJSr9PmVhuBEnCpKp7rgVcI+973yu+DU5yJqZfSf6scuicXljs+fqGBRgMD1KMN1JcryzS0UNATKTn3s4/0hk3waUC7exD+G9AUQxTxfZuhTJThg9U8LDKS1XLbVytPl75KKw0ox95nUzNHTbSvxc12uFum0UzcI7/Pu6Oakzokw0qCDsBnl3masRiuLIS7lJeRpKJyNHG7d1tvaUMXeaSnW72pF4Nl3g7EGqxNR5FSkiXxuGY";

                        AwsSigner AwsSignature = new AwsSigner(AuthAccessKey, AuthSecretKey, uri, AuthSessionToken);
                        responseMessage = await AwsSignature.GetAws4Signature("Aws4RequestSigner")
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