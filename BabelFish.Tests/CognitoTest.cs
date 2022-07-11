using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;
using Amazon.CognitoIdentityProvider.Model;
using ShootersTech.BabelFish.Authentication;

namespace ShootersTech.BabelFish.Tests {
    [TestClass]
    public class CognitoTest {

        private static string accountID = "703601548845";
        private static string awsRegion = "us-east-1";
        private static string clientID = "3mbaq4124a5emsap21krllrcrj";
        private static string poolID = $"{awsRegion}_ujMUC50fP";
        private static string identityPoolID = $"{awsRegion}:ecdb1323-5308-445f-845e-55871ebf14e2";
        private static string username = "test_dev_7@shooterstech.net";
        private static string password = "abcd1234";
        private static string myRandomAssDevicePassword = "simple";

        [TestMethod]
        public async Task CognitoLogin()
        {
            // Login with username/password and get back tokens from Cognito
            //  NOTE: BabelFish clears Password after successful authentication
            string xApiKey = "ga9HvqN7i14sbzM6WrIb0amzdyIYkej83b8aJaWz";
            Dictionary<string, string> clientParams = new Dictionary<string, string>()
            {
                {"UserName", "test_dev_9@shooterstech.net"},
                {"PassWord", "abcd1234"},
            };
            AuthAPIClient _client = new AuthAPIClient(xApiKey, clientParams);

            var response = _client.CognitoLoginAsync();
            Assert.IsNotNull(response);

            var taskResult = response.Result;
            var objResponse = taskResult.AuthTokens;

            Assert.IsTrue(objResponse.RefreshToken != "");

            var msgResponse = taskResult.MessageResponse;
            Assert.IsNotNull(msgResponse);
            Assert.IsTrue(msgResponse.Message.Count == 0);

            // In addition to AuthTokens object returning new tokens after successful authenticaiton,
            //  base client has GetAuthTokens() to save for future session
            //  and UpdateAuthTokens() to authenticate in same session if alive > 60 minutes
            Dictionary<string, string> newAuthTokens = _client.GetAuthTokens();
            Assert.IsNull(newAuthTokens["PassWord"]);           // cleared after success
            Assert.IsTrue(newAuthTokens["UserName"] != "");     // same as initial input
            Assert.IsTrue(newAuthTokens["RefreshToken"] != ""); // returned Token (valid for 30 days)
            Assert.IsTrue(newAuthTokens["IdToken"] != "");      // returned Token (valid for 60 minutes, renewed)
            Assert.IsTrue(newAuthTokens["AccessToken"] != "");  // returned Token (valid for 60 minutes, renewed)
            Assert.IsTrue(newAuthTokens["DeviceToken"] != "");  // returned Token

            // Use New Tokens to Authenticate subsequent logins
            clientParams.Clear();
            foreach ( KeyValuePair<string,string> kvp in newAuthTokens)
                clientParams.Add(kvp.Key, kvp.Value);
            AuthAPIClient _client2 = new AuthAPIClient(xApiKey, clientParams);

            var response2 = _client2.CognitoLoginAsync();
            Assert.IsNotNull(response);

            var taskResult2 = response2.Result;
            var objResponse2 = taskResult2.AuthTokens;

            Assert.IsTrue(objResponse2.RefreshToken != "");

            var msgResponse2 = taskResult2.MessageResponse;
            Assert.IsNotNull(msgResponse2);
            Assert.IsTrue(msgResponse2.Message.Count == 0);

        }


        [TestMethod]
        public async Task LoginTest() {

            //See:https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/cognito-authentication-extension.html
            //See: https://github.com/aws/aws-sdk-net-extensions-cognito#authenticating-with-secure-remote-protocol-srp

            //Create anonymous credentials to create a Cognito Identify Provider
            AmazonCognitoIdentityProviderClient provider =
                new AmazonCognitoIdentityProviderClient( new Amazon.Runtime.AnonymousAWSCredentials() );

            //Set up the initial request when we don't have a refresh token. Obviously assumes an existing user.
            CognitoUserPool userPool = new CognitoUserPool( poolID, clientID, provider );
            CognitoUser user = new CognitoUser( username, clientID, userPool, provider );
            //This is the only time we need the user's password (assuming the soon to be obtained refresh token stays valid)
            InitiateSrpAuthRequest authRequest = new InitiateSrpAuthRequest() {
                Password = password
            };

            //Send the request to Cognito for authentication
            AuthFlowResponse authResponse = await user.StartWithSrpAuthAsync( authRequest ).ConfigureAwait( false );
            //TODO: Determine the response when the username/password don't match.


            //The returned authResponse may present a second set of challenges. 
            // * New Passwword -> RespondToNewPasswordRequiredAsync()
            // * MFA Response -> RespondToSmsMfaAuthAsync() or RespondToMfaAuthAsyn (probable doesn't apply to us)

            //Both the AccessToken (good for 60 minutes by default) and RefreshToken (good for 30 days) need to be saved
            //DeviceKey also needs to be saved. See below, may also need to save DeviceGroupKey, and the device password.
            //IdToken is needed to retrieive AWS Credentials, access key, secret access key, and session token
            var accessToken = authResponse.AuthenticationResult.AccessToken;
            var refreshToken = authResponse.AuthenticationResult.RefreshToken;
            var idToken = authResponse.AuthenticationResult.IdToken;
            var deviceKey = authResponse.AuthenticationResult.NewDeviceMetadata.DeviceKey;

            //Now we can get AWS Credentials for the user
            CognitoAWSCredentials credentials =
                user.GetCognitoAWSCredentials( identityPoolID, RegionEndpoint.USEast1 );

            var restAPICredntials = await GetRestAPICredentials( user.SessionTokens.IdToken );

            //Above, when we made the StartWithSrpAuthAsync Call, the call also generates a temporary 'Device'
            //Now need to tell Cognito to confirm this temporary device.
            //TODO: Determine if we will need to remember the device password.
            //TODO: Determine how device authentication will work with users signing onto www server (instead of an App).

            //Device Verification happens locally. the Salt and password verifier gets sent to Cognito in the next .ConfirmDeviceAsync() call
            var deviceVerifier = user.GenerateDeviceVerifier(
                authResponse.AuthenticationResult.NewDeviceMetadata.DeviceGroupKey,
                myRandomAssDevicePassword,
                username );

            ConfirmDeviceResponse confirmDeviceResponse = await user.ConfirmDeviceAsync(
                accessToken,
                deviceKey,
                "Bob",
                deviceVerifier.PasswordVerifier,
                deviceVerifier.Salt );

            //Now attach the newly attached Device to the user. We need this step to successfully complete the StartWithRefreshTokenAuthAsync() call
            //NOTE There may be a better way to do this, but method here is to first create a generic Device, with the known 
            //device key. Then Use GetDeviceAsync() to pull the real details from Cognito
            CognitoDevice device = new CognitoDevice(
                deviceKey,
                new Dictionary<string, string>(),
                DateTime.Today,
                DateTime.Today,
                DateTime.Today,
                user );
            await device.GetDeviceAsync();
            user.Device = device;

            //Now pretend we need ot fast foward in time and refresh the tokens

            //See: https://github.com/aws/aws-sdk-net-extensions-cognito#authenticating-using-a-refresh-token-from-a-previous-session

            //I'm actaully not sure what this step does ..
            user.SessionTokens = new CognitoUserSession( null, null, refreshToken, DateTime.UtcNow, DateTime.UtcNow.AddHours( 1 ) );

            //Create the refresh request object
            InitiateRefreshTokenAuthRequest refreshRequest = new InitiateRefreshTokenAuthRequest() {
                AuthFlowType = AuthFlowType.REFRESH_TOKEN_AUTH
            };

            //CAll Cognito to refresh the token
            AuthFlowResponse authResponse2 = await user.StartWithRefreshTokenAuthAsync( refreshRequest ).ConfigureAwait( false );

            //Now we have a new accessToken and a new refreshToken, both of which need to be re-saved
            var accessToken2 = authResponse2.AuthenticationResult.AccessToken;
            var refreshToken2 = authResponse2.AuthenticationResult.RefreshToken;

            //Again, we can get new credentials for signing API requests
            CognitoAWSCredentials credentials2 =
                user.GetCognitoAWSCredentials( identityPoolID, RegionEndpoint.USEast1 );

            var restAPICredntials2 = await GetRestAPICredentials( user.SessionTokens.IdToken );
        }

        public async Task<RestAPICredentials> GetRestAPICredentials( string idToken ) {

            AmazonCognitoIdentityClient identityClient =
                new AmazonCognitoIdentityClient( new Amazon.Runtime.AnonymousAWSCredentials() );

            //Using the idToken, obtain the access key, secret access key, and session token
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

            //Second step, get the Credentials for the Identity we just learned
            var getCredentialsForIdentityRequest = new Amazon.CognitoIdentity.Model.GetCredentialsForIdentityRequest() {
                IdentityId = getIDResponse.IdentityId,
                Logins = logins
            };
            var getCredentialsForIdentityResponse = await identityClient.GetCredentialsForIdentityAsync( getCredentialsForIdentityRequest ).ConfigureAwait( false );

            //YAY now we can make Rest API Calls
            return new RestAPICredentials() {
                AccessKey = getCredentialsForIdentityResponse.Credentials.AccessKeyId,
                SecretKey = getCredentialsForIdentityResponse.Credentials.SecretKey,
                SessionToken = getCredentialsForIdentityResponse.Credentials.SessionToken
            };
        }
    }

    public class RestAPICredentials {

        public RestAPICredentials() { }

        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string SessionToken { get; set; }
    }
}
