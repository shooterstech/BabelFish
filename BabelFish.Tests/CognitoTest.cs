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

namespace BabelFish.Tests {
    [TestClass]
    public class CognitoTest {

        private static string clientID = "3mbaq4124a5emsap21krllrcrj";
        private static string poolID = "us-east-1_ujMUC50fP";
        private static string identityPoolID = "us-east-1:ecdb1323-5308-445f-845e-55871ebf14e2";
        private static string username = "test_dev_7@shooterstech.net";
        private static string password = "abcd1234";
        private static string myRandomAssDevicePassword = "simple";

        [TestMethod]
        public async Task LoginTest() {
            AmazonCognitoIdentityProviderClient provider =
                new AmazonCognitoIdentityProviderClient( new Amazon.Runtime.AnonymousAWSCredentials() );
            CognitoUserPool userPool = new CognitoUserPool( poolID, clientID, provider );
            CognitoUser user = new CognitoUser( username, clientID, userPool, provider );
            InitiateSrpAuthRequest authRequest = new InitiateSrpAuthRequest() {
                Password = password                 
            };

            AuthFlowResponse authResponse = await user.StartWithSrpAuthAsync( authRequest ).ConfigureAwait( false );
            var accessToken = authResponse.AuthenticationResult.AccessToken;
            var refreshToken = authResponse.AuthenticationResult.RefreshToken;
            
            CognitoAWSCredentials credentials =
                user.GetCognitoAWSCredentials( identityPoolID, RegionEndpoint.USEast1 );

            var deviceVerifier = user.GenerateDeviceVerifier(
                authResponse.AuthenticationResult.NewDeviceMetadata.DeviceGroupKey,
                myRandomAssDevicePassword,
                username );

            ConfirmDeviceResponse confirmDeviceResponse = await user.ConfirmDeviceAsync(
                accessToken, 
                authResponse.AuthenticationResult.NewDeviceMetadata.DeviceKey,
                "Bob",
                deviceVerifier.PasswordVerifier,
                deviceVerifier.Salt);

            CognitoDevice device = new CognitoDevice(
                authResponse.AuthenticationResult.NewDeviceMetadata.DeviceKey,
                new Dictionary<string, string>(),
                DateTime.Today,
                DateTime.Today,
                DateTime.Today,
                user );
            await device.GetDeviceAsync();
            user.Device = device;
            
            
            user.SessionTokens = new CognitoUserSession( null, null, refreshToken, DateTime.UtcNow, DateTime.UtcNow.AddHours( 1 ) );

            InitiateRefreshTokenAuthRequest refreshRequest = new InitiateRefreshTokenAuthRequest() {
                AuthFlowType = AuthFlowType.REFRESH_TOKEN_AUTH
            };

            
            AuthFlowResponse authResponse2 = await user.StartWithRefreshTokenAuthAsync( refreshRequest ).ConfigureAwait( false );
            var accessToken2 = authResponse2.AuthenticationResult.AccessToken;
            var refreshToken2 = authResponse2.AuthenticationResult.RefreshToken;

            CognitoAWSCredentials credentials2 =
                user.GetCognitoAWSCredentials( identityPoolID, RegionEndpoint.USEast1 );
        }
    }
}
