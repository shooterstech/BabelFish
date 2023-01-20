using System;
using System.Collections.Generic;
using System.Text;

using Amazon;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.Runtime.Authentication {
    public class AWSCognitoAuthenticationOld {
        /* Documentation
         * See:https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/cognito-authentication-extension.html
         * See: https://github.com/aws/aws-sdk-net-extensions-cognito#authenticating-with-secure-remote-protocol-srp
         *https://aws.amazon.com/blogs/mobile/use-csharp-to-register-and-authenticate-with-amazon-cognito-user-pools/
         */

        // Known, Fixed Variables
        private static string awsRegion = "us-east-1";
        private static string poolID = $"{awsRegion}_ujMUC50fP";
        private static string identityPoolID = $"{awsRegion}:ecdb1323-5308-445f-845e-55871ebf14e2";
        private static string accountID = "703601548845";
        private static string clientID = "3mbaq4124a5emsap21krllrcrj"; //orionmobileapp

        /*
        //private static string clientID = "7nkt8c3i8o6uf2d627ut292gln"; //BabelFishTest
        //private static string clientID = "3njrf7kqhib2j354hefpr04gkq"; //Magento
        //private static string clientSecret = "vifpfgbst6teh3gol6bget2emvvig3hloojri517bkn5clk6dlo"; //Magento
        private string myRandomAssDevicePassword = "simple";

        // Incoming variables for user-specific queries
        private string username = string.Empty; //"test_dev_7@Scopos.net";
        private string password = string.Empty; //"abcd1234";
        private string refreshToken = string.Empty;

        // Additional variables for queries
        private string accessToken = string.Empty;
        private string idToken = string.Empty; // Used to re-auth, good for 60 minutes
        private static string deviceKey = string.Empty;
        */

        private UserCredentialsOld credentials;

        /// <summary>
        /// Public constructor
        /// </summary>
        public AWSCognitoAuthenticationOld( UserCredentialsOld credentials ) {
            this.credentials = credentials;
        }

        /*
        // Result passed back for API request re-signing
        public string AccessKey { get; private set; } = string.Empty;
        public string SecretKey { get; private set; } = string.Empty;
        public string SessionToken { get; private set; } = string.Empty;
        public string LastException { get; private set; } = string.Empty;
        public string RefreshToken
        {
            get
            {
                return refreshToken;
            }
        }
        public string AccessToken
        {
            get
            {
                return accessToken;
            }
        }
        public string IdToken
        {
            get
            {
                return idToken;
            }
        }
        public string DeviceToken
        {
            get
            {
                return deviceKey;
            }
        }

        /// <summary>
        /// Set everything relevant for authentication from SettingsHelper
        /// </summary>
        //TODO: Decide which all are needed for desired functionality
        private void LoadParameters()
        {
            username = Helpers.SettingsHelper.UserSettings[AuthEnums.UserName.ToString()] ?? "";
            password = Helpers.SettingsHelper.UserSettings[AuthEnums.PassWord.ToString()] ?? "";
            refreshToken = Helpers.SettingsHelper.UserSettings[AuthEnums.RefreshToken.ToString()] ?? "";
            idToken = Helpers.SettingsHelper.UserSettings[AuthEnums.IdToken.ToString()] ?? "";
            accessToken = Helpers.SettingsHelper.UserSettings[AuthEnums.AccessToken.ToString()] ?? "";
            deviceKey = Helpers.SettingsHelper.UserSettings[AuthEnums.DeviceToken.ToString()] ?? "";
            myRandomAssDevicePassword = $"devicepassword_for_{username}";
        }
        */

        /// <summary>
        /// Clear parameters for reset/re-login
        /// </summary>
        private void ClearParameters() {
            credentials.RefreshToken = "";
            credentials.IdToken = "";
            credentials.AccessToken = "";
            credentials.DeviceId = "";
        }

        /// <summary>
        /// Validate minimum incoming parameters to process
        /// </summary>
        /// <returns></returns>
        private bool ValidateParameters() {
            if (credentials.RefreshToken == "" && (credentials.Username == "" || credentials.Password == ""))
                return false;
            else if (credentials.RefreshToken != "" && (credentials.IdToken == "" || credentials.AccessToken == "" || credentials.DeviceId == ""))
                return false;
            else
                return true;
        }

        /// <summary>
        /// Entry point to calculate credentials
        /// </summary>
        /// <returns>true or false</returns>
        public async Task<bool> GetCognitoCredentialsAsync() {
            try {

                if (ValidateParameters()) {
                    // Note for 2nd run: This is user Setup only;
                    // Does not authenticate; I think the key is to figure out that process?
                    credentials.CognitoUser = InitializeCognitoUser();

                    AuthenticateCognitoUserAsync().Wait();

                    if (credentials.LastException == "")
                        SetRestAPICredentials().Wait();
                    if (!string.IsNullOrEmpty( credentials.AccessKeyId ) && !string.IsNullOrEmpty( credentials.SecretKey ) && !string.IsNullOrEmpty( credentials.SessionToken ))
                        return true;
                    else
                        return false;
                } else {
                    credentials.LastException = "Invalid Authentication Parameters";
                    return false;
                }
            } catch (Exception ex) {
                credentials.LastException = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Authenticate based on incoming parameters
        /// </summary>
        private async Task AuthenticateCognitoUserAsync() {
            try {
                credentials.LastException = string.Empty;
                AuthFlowResponse authResponse = null;

                //Send the request to Cognito for authentication
                if (IsLoggingIn())
                    // First run through to Authenticate with Username/Password
                    authResponse = await CredentialsCreateNewAsync().ConfigureAwait( false );
                else {
                    // Subsequent runs with RefreshToken
                    authResponse = await CredentialsRefreshAsync().ConfigureAwait( true );
                }

                if (authResponse == null && credentials.LastException == "Invalid Username or Password") {
                    //TODO: Check what expired RefreshToken returns and return appropriate error
                    //LastException += " or Invalid Username or Password!";
                } else if (authResponse != null) {
                    //The returned authResponse may present a second set of challenges. 
                    // authResponse is null and the ChallengeName property describes the next challenge
                    // * New Passwword -> RespondToNewPasswordRequiredAsync()
                    // * MFA Response -> RespondToSmsMfaAuthAsync() or RespondToMfaAuthAsyn (probable doesn't apply to us)
                    //Challenge Name: https://docs.aws.amazon.com/cognito-user-identity-pools/latest/APIReference/API_AdminInitiateAuth.html#API_AdminInitiateAuth_ResponseSyntax
                    if (authResponse.AuthenticationResult == null) {
                        if (authResponse.ChallengeName == ChallengeNameType.NEW_PASSWORD_REQUIRED) {
                            credentials.LastException = "New Password Required";
                        } else if (authResponse.ChallengeName == ChallengeNameType.SMS_MFA) {
                            credentials.LastException = "MFA Code required";
                        }
                        ClearParameters();
                    } else {
                        //Both the AccessToken (good for 60 minutes by default) and RefreshToken (good for 30 days) need to be saved
                        //DeviceKey also needs to be saved. See below, may also need to save DeviceGroupKey, and the device password.
                        //IdToken is needed to retrieive AWS Credentials, access key, secret access key, and session token
                        credentials.AccessToken = authResponse.AuthenticationResult.AccessToken;
                        credentials.RefreshToken = authResponse.AuthenticationResult.RefreshToken;
                        credentials.IdToken = authResponse.AuthenticationResult.IdToken;
                        if (IsLoggingIn()) {
                            credentials.DeviceId = authResponse.AuthenticationResult.NewDeviceMetadata.DeviceKey;
                            await LoadUserDevice( authResponse ).ConfigureAwait( false );
                        } else
                            credentials.DeviceId = credentials.CognitoUser.Device.DeviceKey;
                    }
                } // idTokens not expired so re-user
            } catch (Exception ex) {
                credentials.LastException = ex.Message;
            }
        }

        #region UserFunctions
        /// <summary>
        /// Instantiate a Cognito User object
        /// </summary>
        /// <returns>CognitoUser object (unauthenticated)</returns>
        private CognitoUser InitializeCognitoUser() {
            CognitoUser user = null;

            //Create anonymous credentials to create a Cognito Identify Provider
            AmazonCognitoIdentityProviderClient provider =
                new AmazonCognitoIdentityProviderClient( new AnonymousAWSCredentials() );

            //Set up the initial request when we don't have a refresh token. Obviously assumes an existing user.
            CognitoUserPool userPool = new CognitoUserPool( poolID, clientID, provider );

            return new CognitoUser( credentials.Username, clientID, userPool, provider );
        }

        /// <summary>
        /// CognitoUser AWS Credentials - assumes valid, authenticated user
        /// </summary>
        /// <returns>CognitoAWSCredentials object</returns>
        private async Task<CognitoAWSCredentials> GetCognitoUserAWSCredentials() {
            CognitoAWSCredentials cognitoAwsCredentials = null;
            try {
                //Now we can get AWS Credentials for the user
                cognitoAwsCredentials = credentials.CognitoUser.GetCognitoAWSCredentials( identityPoolID, RegionEndpoint.USEast1 );
            } catch (Exception ex) {
                credentials.LastException = ex.Message;
            }
            return cognitoAwsCredentials;
        }

        /// <summary>
        /// Load Device into cognitoUser object
        /// </summary>
        /// <param name="authResponse"></param>
        private async Task LoadUserDevice( AuthFlowResponse authResponse = null ) {
            try {
                //TODO: Rich raised question about creating new device every refresh;
                //  Check if there is a way to do one Device to rule them all with this connection??

                //Above, when we made the StartWithSrpAuthAsync Call, the call also generates a temporary 'Device'
                //Now need to tell Cognito to confirm this temporary device.
                //TODO: Determine how device authentication will work with users signing onto www server (instead of an App).

                if (IsLoggingIn()) {
                    //Device Verification happens locally. the Salt and password verifier gets sent to Cognito in the next .ConfirmDeviceAsync() call
                    var deviceVerifier = credentials.CognitoUser.GenerateDeviceVerifier(
                        authResponse.AuthenticationResult.NewDeviceMetadata.DeviceGroupKey,
                        credentials.DevicePassword,
                        credentials.Username );

                    ConfirmDeviceResponse confirmDeviceResponse = await credentials.CognitoUser.ConfirmDeviceAsync(
                        credentials.AccessToken,
                        credentials.DeviceId,
                        $"{credentials.Username}_{credentials.DeviceId}",
                        deviceVerifier.PasswordVerifier,
                        deviceVerifier.Salt ).ConfigureAwait( true );
                }

                //Now attach the newly attached Device to the user. We need this step to successfully complete the StartWithRefreshTokenAuthAsync() call
                //NOTE There may be a better way to do this, but method here is to first create a generic Device, with the known 
                //device key. Then Use GetDeviceAsync() to pull the real details from Cognito
                CognitoDevice device = new CognitoDevice(
                    credentials.DeviceId,
                    new Dictionary<string, string>(),
                    DateTime.Today,
                    DateTime.Today,
                    DateTime.Today,
                    credentials.CognitoUser );
                await device.GetDeviceAsync();
                credentials.CognitoUser.Device = device;
            } catch (Exception ex) {
                credentials.LastException = ex.Message;
            }
        }

        /// <summary>
        /// Get Cognito UserId after we get IdToken
        /// </summary>
        /// <param name="idToken"></param>
        /// <returns></returns>
        private async Task<Amazon.CognitoIdentity.Model.GetIdResponse> GetCognitoUserIdByIdToken( string idToken ) {
            try {
                AmazonCognitoIdentityClient identityClient =
                    new AmazonCognitoIdentityClient( new AnonymousAWSCredentials() );

                //First step get the Id of the Cognito User
                var logins = new Dictionary<string, string>() {
                                { $"cognito-idp.{awsRegion}.amazonaws.com/{poolID}", idToken }
                };
                var getIDRequest = new Amazon.CognitoIdentity.Model.GetIdRequest() {
                    AccountId = accountID,
                    IdentityPoolId = identityPoolID,
                    Logins = logins
                };
                var getIDResponse = await identityClient.GetIdAsync( getIDRequest ).ConfigureAwait( false );

                return getIDResponse;

            } catch (Exception ex) {
                credentials.LastException = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// Cognito User Object from AccessId
        /// Added by Adam for 2nd Authentication testing
        /// Thought I needed sub but not working so just used email
        ///     which is just username
        /// </summary>
        /// <param name="accessId"></param>
        /// <returns>GetUserResponse->UserAttributes: sub, birthdate, email_verified, given_name, family_name, email</returns>
        [Obsolete( "What the fuck does this even do?" )]
        public async Task<GetUserResponse> GetCognitoUserByAccessId( string accessId ) {
            //https://stackoverflow.com/questions/50057066/aws-cogntio-how-to-make-authentication-request-in-net-after-signup-signin
            AmazonCognitoIdentityProviderClient providerClient =
                new AmazonCognitoIdentityProviderClient( new AnonymousAWSCredentials(), FallbackRegionFactory.GetRegionEndpoint() );
            Task<GetUserResponse> responseTask =
            providerClient.GetUserAsync( new GetUserRequest {
                AccessToken = credentials.AccessToken
            } );
            GetUserResponse responseObject = await responseTask.ConfigureAwait( false );

            return responseObject;
        }

        #endregion UserFunctions

        #region CredentialRetrievalFunctions

        /// <summary>
        /// Authenticate initially with username/password
        /// </summary>
        /// <returns>AuthFlowResponse object</returns>
        private async Task<AuthFlowResponse> CredentialsCreateNewAsync() {
            AuthFlowResponse returnResponse = null;
            try {
                //This is the only time we need the user's password (assuming the soon to be obtained refresh token stays valid)
                InitiateSrpAuthRequest authRequest = new InitiateSrpAuthRequest() {
                    Password = credentials.Password
                };

                //Send the request to Cognito for authentication
                returnResponse = await credentials.CognitoUser.StartWithSrpAuthAsync( authRequest ).ConfigureAwait( false );
            } catch (Exception ex) {
                credentials.LastException = ex.Message;
            }
            return returnResponse;
        }

        private async Task<AuthFlowResponse> CredentialsRefreshAsync() {
            AuthFlowResponse returnResponse = null;
            try {
                //Refresh AccesID, IdToken based on RefreshToken
                //https://docs.aws.amazon.com/cognito/latest/developerguide/amazon-cognito-user-pools-authentication-flow.html
                //See: https://github.com/aws/aws-sdk-net-extensions-cognito#authenticating-using-a-refresh-token-from-a-previous-session

                //EXPIRED ONE FOR TESTING
                //idToken = "eyJraWQiOiJOOE1mT0tBYkowRU9ObERYTThBVk9TOU5MVWVueVVMV29mXC9JTUcyTE9ROD0iLCJhbGciOiJSUzI1NiJ9.eyJzdWIiOiIyNmYzMjIyNy1kNDI4LTQxZjYtYjIyNC1iZWVkN2I2ZTg4NTAiLCJjb2duaXRvOmdyb3VwcyI6WyJEZXZlbG9wbWVudCJdLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwiYmlydGhkYXRlIjoiMTk4MC0wMy0xMiIsImlzcyI6Imh0dHBzOlwvXC9jb2duaXRvLWlkcC51cy1lYXN0LTEuYW1hem9uYXdzLmNvbVwvdXMtZWFzdC0xX3VqTVVDNTBmUCIsImNvZ25pdG86dXNlcm5hbWUiOiIyNmYzMjIyNy1kNDI4LTQxZjYtYjIyNC1iZWVkN2I2ZTg4NTAiLCJnaXZlbl9uYW1lIjoiQ2hyaXMiLCJhdWQiOiIzbWJhcTQxMjRhNWVtc2FwMjFrcmxscmNyaiIsImV2ZW50X2lkIjoiMDA0OTYwMDQtNDk1Yy00MzcxLTg4MDEtZDE2NzVkYmJiNTA3IiwidG9rZW5fdXNlIjoiaWQiLCJhdXRoX3RpbWUiOjE2NTAzOTM0MDAsImV4cCI6MTY1MDM5NzAwMCwiaWF0IjoxNjUwMzkzNDAwLCJmYW1pbHlfbmFtZSI6IkpvbmVzIiwiZW1haWwiOiJ0ZXN0X2Rldl83QHNob290ZXJzdGVjaC5uZXQifQ.RSGf8RZD0ZTMgzgTP1O1qJrnNSkyztEMEoO0JcWTiSY9S5JfNW3LV-4gycxfewQ6Hj_jiBMmIDYzo_r4k5m5yC9DWGvUoAYpO0dnrsKBslC5wWrysgjKoptmJykKcLRGAudviqdszS7h1bXuzkXzPFzbr1gF9ftoKWWOAqrCks0OWlSASMHPm8DVfFeaHahmwDSLxXW7FwIIvsom9Y9Q6QAU7rWfg5eSDD4g9UezCG1Lvk4OVL9mQKsftfH8reUErHOMyL-__NSic8bbd5o2sK8b1uKZNkbDsGDf5rBQFC7wkhtm5flUT2Dr-1bgBNOciVGIEH-qWYmy1sW1oiFpAg";

                //I'm actaully not sure what this step does ..
                //cognitoUser.SessionTokens = new CognitoUserSession(null, null, refreshToken, DateTime.UtcNow, DateTime.UtcNow.AddHours(1));
                credentials.CognitoUser.SessionTokens = new CognitoUserSession( credentials.IdToken, credentials.AccessToken, credentials.RefreshToken, DateTime.UtcNow, DateTime.UtcNow.AddHours( 1 ) );
                await LoadUserDevice().ConfigureAwait( false );

                // only renew idToken if expired, otherwise passes through existing
                var checkToken = await GetCognitoUserIdByIdToken( credentials.IdToken ).ConfigureAwait( false );
                if (checkToken == null) {
                    //Create the refresh request object
                    InitiateRefreshTokenAuthRequest refreshRequest = new InitiateRefreshTokenAuthRequest() {
                        AuthFlowType = AuthFlowType.REFRESH_TOKEN_AUTH
                    };

                    //Call Cognito to refresh the token
                    returnResponse = await credentials.CognitoUser.StartWithRefreshTokenAuthAsync( refreshRequest ).ConfigureAwait( false );
                    credentials.LastException = "";
                }
            } catch (Exception ex) {
                credentials.LastException = ex.Message;
            }
            return returnResponse;
        }

        private async Task CredentialsReloadAsync() {
            //https://docs.aws.amazon.com/cognito/latest/developerguide/amazon-cognito-user-pools-using-tokens-verifying-a-jwt.html
            // A valid (unexpired) refresh token must present,
            // and the ID and access tokens must have a minimum remaining validity time of 5 minutes.

            try {
                //EXPIRED ONE FOR TESTING
                //idToken = "eyJraWQiOiJOOE1mT0tBYkowRU9ObERYTThBVk9TOU5MVWVueVVMV29mXC9JTUcyTE9ROD0iLCJhbGciOiJSUzI1NiJ9.eyJzdWIiOiIyNmYzMjIyNy1kNDI4LTQxZjYtYjIyNC1iZWVkN2I2ZTg4NTAiLCJjb2duaXRvOmdyb3VwcyI6WyJEZXZlbG9wbWVudCJdLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwiYmlydGhkYXRlIjoiMTk4MC0wMy0xMiIsImlzcyI6Imh0dHBzOlwvXC9jb2duaXRvLWlkcC51cy1lYXN0LTEuYW1hem9uYXdzLmNvbVwvdXMtZWFzdC0xX3VqTVVDNTBmUCIsImNvZ25pdG86dXNlcm5hbWUiOiIyNmYzMjIyNy1kNDI4LTQxZjYtYjIyNC1iZWVkN2I2ZTg4NTAiLCJnaXZlbl9uYW1lIjoiQ2hyaXMiLCJhdWQiOiIzbWJhcTQxMjRhNWVtc2FwMjFrcmxscmNyaiIsImV2ZW50X2lkIjoiMDA0OTYwMDQtNDk1Yy00MzcxLTg4MDEtZDE2NzVkYmJiNTA3IiwidG9rZW5fdXNlIjoiaWQiLCJhdXRoX3RpbWUiOjE2NTAzOTM0MDAsImV4cCI6MTY1MDM5NzAwMCwiaWF0IjoxNjUwMzkzNDAwLCJmYW1pbHlfbmFtZSI6IkpvbmVzIiwiZW1haWwiOiJ0ZXN0X2Rldl83QHNob290ZXJzdGVjaC5uZXQifQ.RSGf8RZD0ZTMgzgTP1O1qJrnNSkyztEMEoO0JcWTiSY9S5JfNW3LV-4gycxfewQ6Hj_jiBMmIDYzo_r4k5m5yC9DWGvUoAYpO0dnrsKBslC5wWrysgjKoptmJykKcLRGAudviqdszS7h1bXuzkXzPFzbr1gF9ftoKWWOAqrCks0OWlSASMHPm8DVfFeaHahmwDSLxXW7FwIIvsom9Y9Q6QAU7rWfg5eSDD4g9UezCG1Lvk4OVL9mQKsftfH8reUErHOMyL-__NSic8bbd5o2sK8b1uKZNkbDsGDf5rBQFC7wkhtm5flUT2Dr-1bgBNOciVGIEH-qWYmy1sW1oiFpAg";

                //Add our incoming IdToken for authentication
                credentials.CognitoUser.SessionTokens = new CognitoUserSession( credentials.IdToken, credentials.AccessToken, credentials.RefreshToken, DateTime.UtcNow, DateTime.UtcNow.AddHours( 1 ) );

                //HERE WE NEED TO CONNECT TO AWS AND AUTHORIZE USER RATHER THAN CheckOrRenewIdToken THAT ERRORS ON EXPIRATION
                await CheckOrRenewIdToken().ConfigureAwait( true );
            } catch (Exception ex) {
                //Invalid Login Token, Refresh
                CredentialsRefreshAsync().Wait();
            }
        }

        private async Task CheckOrRenewIdToken() {

            //https://aws.amazon.com/premiumsupport/knowledge-center/decode-verify-cognito-json-token/
            //Apart from the signature, it's also a best practice to verify the following:
            //The token isn't expired.
            //The audience("aud") specified in the payload matches the app client ID created in the Amazon Cognito user pool.
            //            var base64EncodedBytes = System.Convert.FromBase64String(idToken);
            //            string test = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

            //???            await cognitoUser.GetUserDetailsAsync().ConfigureAwait(true);
            //??? if idToken < 60 minutes?
            //??? use RefreshToken to renew

            // Figured out this returns error if token expired
            var checkToken = await GetCognitoUserIdByIdToken( credentials.IdToken ).ConfigureAwait( false );
            if (checkToken == null) {
                if (credentials.LastException.Contains( "Invalid login token. Token expired" )) {
                    credentials.IdToken = string.Empty;
                    CredentialsRefreshAsync().Wait();
                }
            }
        }

        #endregion CredentialRetrievalFunctions

        /// <summary>
        /// Identify logic path
        /// </summary>
        /// <returns></returns>
        public bool IsLoggingIn() {
            if (credentials.Password != null && credentials.Password != "")
                return true;
            else
                return false;
        }

        public async Task SetRestAPICredentials() {
            try {
                await GetCognitoUserAWSCredentials().ConfigureAwait( false );

                if (credentials.CognitoUser.SessionTokens != null) {
                    string idToken = credentials.CognitoUser.SessionTokens.IdToken;

                    AmazonCognitoIdentityClient identityClient =
                        new AmazonCognitoIdentityClient( new AnonymousAWSCredentials() );

                    //Using the idToken, obtain the access key, secret access key, and session token
                    //First step get the Id of the Cognito User
                    var logins = new Dictionary<string, string>() {
                            { $"cognito-idp.{awsRegion}.amazonaws.com/{poolID}", idToken }
                        };
                    var getIDResponse = await GetCognitoUserIdByIdToken( idToken ).ConfigureAwait( true );

                    //Second step, get the Credentials for the Identity we just learned
                    var getCredentialsForIdentityRequest = new Amazon.CognitoIdentity.Model.GetCredentialsForIdentityRequest() {
                        IdentityId = getIDResponse.IdentityId,
                        Logins = logins
                    };
                    var getCredentialsForIdentityResponse = await identityClient.GetCredentialsForIdentityAsync( getCredentialsForIdentityRequest ).ConfigureAwait( false );

                    credentials.AccessToken = getCredentialsForIdentityResponse.Credentials.AccessKeyId;
                    credentials.SecretKey = getCredentialsForIdentityResponse.Credentials.SecretKey;
                    credentials.SessionToken = getCredentialsForIdentityResponse.Credentials.SessionToken;
                }
            } catch (Exception ex) {
                credentials.LastException = ex.Message;
            }
        }

    }
}
