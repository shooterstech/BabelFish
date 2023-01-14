using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.Requests.Authentication.Credentials;
using Scopos.BabelFish.Responses.Authentication.Credentials;
using Newtonsoft.Json;

namespace Scopos.BabelFish.DataModel.Authentication.Credentials
{
    /// <summary>
    /// AWS Credentials
    /// </summary>
    [Serializable]
    public class Credential
    {
        private AWSCognitoAuthentication CognitoAuthentication = new AWSCognitoAuthentication();

        public string AccessKeyId { get; set; } = string.Empty;

        #region AWSSigning
        public string AccessKey
        {
            get {
                return AccessKeyId;
            }
            set
            {
                AccessKeyId = value;
            }
        }

        public string SecretKey { get; set; } = string.Empty;

        public string SessionToken { get; set; } = string.Empty;
        #endregion AWSSigning

        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        #region CognitoAuthenticationSpecific
        public string RefreshToken { get; set; } = string.Empty;

        public string AccessToken { get; set; } = string.Empty;

        public string IdToken { get; set; } = string.Empty;

        public string DeviceToken { get; set; } = string.Empty;

        public string LastException { get; private set; } = string.Empty;
        #endregion CognitoAuthenticationSpecific

        #region AWS4SigningToken
        [Obsolete("XApiKey for deprecated GetCredentials()")]
        private const string InternalXApiKey = "uONGn6tHGw14kreLdqbfJ9rwR2C55uS8a9rGnmIf";


        /// <summary>
        /// Track last time the temporary tokens were generated
        /// These are good for 15 minutes so re-generate if still being used after that
        /// https://docs.aws.amazon.com/AmazonS3/latest/API/sig-v4-authenticating-requests.html
        /// The signed portions (using AWS Signatures) of requests are valid within 15 minutes of the timestamp in the request. An unauthorized party who has access to a signed request can modify the unsigned portions of the request without affecting the request's validity in the 15 minute window.
        /// Using null to ignore refresh because permanent tokens
        /// </summary>
        public DateTime? ContinuationToken { get; set; } = null;
        private double AWSExpirationLimit { get; } = 15;

        public bool TokensExpired()
        {
            if (ContinuationToken == null ||
                (DateTime.Now - ContinuationToken).Value < TimeSpan.FromMinutes(AWSExpirationLimit))
                return false;
            else
                return true;
        }
        /// <summary>
        /// Shooter's Tech GetCredentials API function 
        /// </summary>
        /// <returns></returns>
        [Obsolete("Deprecated, use Cognito Authentication")]
        private async Task<bool> GetCredentials()
        {
            GetCredentialsResponse response = new GetCredentialsResponse();
            try
            {
                GetCredentialsRequest requestParameters = new GetCredentialsRequest(Username, Password);
//                await this.CallAPI(requestParameters, response);

                if (response.Credential != null)
                {
                    if (response.Credential.AccessKeyId != string.Empty &&
                        response.Credential.SecretKey != string.Empty &&
                        response.Credential.SessionToken != string.Empty)
                    {
                        AccessKeyId = response.Credential.AccessKeyId;
                        SecretKey = response.Credential.SecretKey;
                        SessionToken = response.Credential.SessionToken;

                        ContinuationToken = DateTime.Now;
                        return true;
                    }

                    return false;
                }
            }
            finally { }

            return false;
        }
        #endregion AWS4SigningToken

        /// <summary>
        /// Get temporary credentials based on username/password and populated AccessKey, SecretKey, and SessionToken
        /// </summary>
        /// <returns>True if Existing Tokens not expired or new Tokens retrieved successfully</returns>
        public async Task<bool> GetTempCredentials()
        {
            try
            {
                if (TokensExpired())
                {
                    if (await CognitoAuthentication.GetCognitoCredentialsAsync().ConfigureAwait(false))
                    {
                        // Temp tokens for Signature
                        AccessKeyId = CognitoAuthentication.AccessKey;
                        SecretKey = CognitoAuthentication.SecretKey;
                        SessionToken = CognitoAuthentication.SessionToken;
                        // Re-authenticate without password
                        RefreshToken = CognitoAuthentication.RefreshToken;
                        AccessToken = CognitoAuthentication.AccessToken;
                        IdToken = CognitoAuthentication.IdToken;
                        DeviceToken = CognitoAuthentication.DeviceToken;

                        // Reset password and environment for subsequent refresh
                        if (RefreshToken != "")
                        {
                            // Populate Session Temp tokens for Signature
                            SettingsHelper.UserSettings[AuthEnums.AccessKey.ToString()] = AccessKeyId;
                            SettingsHelper.UserSettings[AuthEnums.SecretKey.ToString()] = SecretKey;
                            SettingsHelper.UserSettings[AuthEnums.SessionToken.ToString()] = SessionToken;
                            // Populate Session Cognito for re-authenticating
                            SettingsHelper.UserSettings[AuthEnums.RefreshToken.ToString()] = RefreshToken;
                            SettingsHelper.UserSettings[AuthEnums.DeviceToken.ToString()] = DeviceToken;
                            SettingsHelper.UserSettings[AuthEnums.AccessToken.ToString()] = AccessToken;
                            SettingsHelper.UserSettings[AuthEnums.IdToken.ToString()] = IdToken;
                            SettingsHelper.UserSettings[AuthEnums.PassWord.ToString()] = null;
                        }

                        ContinuationToken = DateTime.Now;
                        return true;
                    }
                    else
                    {
                        LastException = CognitoAuthentication.LastException;
                        return false;
                    }
                }
                else
                {
                    AccessKeyId = Helpers.SettingsHelper.UserSettings[AuthEnums.AccessKey.ToString()];
                    SecretKey = Helpers.SettingsHelper.UserSettings[AuthEnums.SecretKey.ToString()];
                    SessionToken = Helpers.SettingsHelper.UserSettings[AuthEnums.SessionToken.ToString()];
                    return true;
                }
            }
            catch (Exception ex) {
                LastException = ex.ToString();
                return false;
            }
        }

        /// <summary>
        /// Perm Tokens assumes (AccessKey,SecretKey) set in incoming settings
        /// </summary>
        /// <returns></returns>
        public bool IsPermToken()
        {
            if ((
                (SettingsHelper.SettingIsNullOrEmpty(AuthEnums.AccessKey.ToString()) ||
                SettingsHelper.SettingIsNullOrEmpty(AuthEnums.SecretKey.ToString())) &&
                SettingsHelper.SettingIsNullOrEmpty(AuthEnums.SessionToken.ToString())
                )
                ||
                (!SettingsHelper.SettingIsNullOrEmpty(AuthEnums.AccessKey.ToString()) &&
                !SettingsHelper.SettingIsNullOrEmpty(AuthEnums.SecretKey.ToString()) &&
                !SettingsHelper.SettingIsNullOrEmpty(AuthEnums.SessionToken.ToString()))
               )
                return false;
            else
            {
                return true;
            }
        }

        public override string ToString()
        {
            return $"Credentials for {this.Username}";
        }

    }
}