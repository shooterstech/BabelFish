using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.Runtime.Authentication;
using Scopos.BabelFish.Runtime;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Tests.Authentication {

    /// <summary>
    /// Set of test methods designed to test the UserAuthentication constructors that
    /// authenticate a user with email (username) and password.
    /// </summary>
    [TestClass]
    public class AuthenticationTests {

        /// <summary>
        /// Tests the happy path, correct username and password using a new device. The user should be logged in and the device attached to the user.
        /// </summary>
        [TestMethod]
        public async Task HappyPathAuthenticationWithNewDevice() {

            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await userAuthentication.InitializeAsync();

            Assert.IsFalse( string.IsNullOrEmpty( userAuthentication.Email ) );
            Assert.IsFalse( string.IsNullOrEmpty( userAuthentication.RefreshToken ) );
            Assert.IsFalse( string.IsNullOrEmpty( userAuthentication.AccessToken ) );
            Assert.IsFalse( string.IsNullOrEmpty( userAuthentication.IdToken ) );
            //Assert.IsFalse( string.IsNullOrEmpty( userAuthentication.DeviceKey ) );
            //Assert.IsFalse( string.IsNullOrEmpty( userAuthentication.DeviceGroupKey ) );
            //Assert.IsFalse( string.IsNullOrEmpty( userAuthentication.DeviceName ) );
            Assert.IsNotNull( userAuthentication.CognitoUser );
            //Assert.IsNotNull( userAuthentication.CognitoUser.Device );

        }

        [TestMethod]
        public async Task TestAuthenticationFromCognitoUser()
        {
            var goodAuth = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password);
            await goodAuth.InitializeAsync();

            var userAuthentication = new UserAuthentication(goodAuth.CognitoUser);
            Assert.IsFalse(string.IsNullOrEmpty(userAuthentication.RefreshToken));
            Assert.IsFalse(string.IsNullOrEmpty(userAuthentication.AccessToken));
            Assert.IsFalse(string.IsNullOrEmpty(userAuthentication.IdToken));
            Assert.IsNotNull(userAuthentication.CognitoUser);

            await userAuthentication.GenerateIAMCredentialsAsync(); //no init call needed
        }

        /// <summary>
        /// Tests that an exception is thrown if the wrong password is used.
        /// </summary>
        [TestMethod]
        [ExpectedException( typeof( Scopos.BabelFish.Runtime.Authentication.NotAuthorizedException ) )]
        public async Task WrongPassword() {

            //Should throw a NotAuthroizedException
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                "not the right password" );
            await userAuthentication.InitializeAsync();
        }

        /*
        [TestMethod]
        public async Task HappyPathAuthenticationWithExistingDevice() {

            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password,
                Constants.TestDev7Credentials.DeviceKey,
                Constants.TestDev7Credentials.DeviceGroupKey );
            await userAuthentication.InitializeAsync();

            Assert.IsFalse( string.IsNullOrEmpty( userAuthentication.Email ) );
            Assert.IsFalse( string.IsNullOrEmpty( userAuthentication.RefreshToken ) );
            Assert.IsFalse( string.IsNullOrEmpty( userAuthentication.AccessToken ) );
            Assert.IsFalse( string.IsNullOrEmpty( userAuthentication.IdToken ) );
            //Assert.IsFalse( string.IsNullOrEmpty( userAuthentication.DeviceKey ) );
            //Assert.IsFalse( string.IsNullOrEmpty( userAuthentication.DeviceGroupKey ) );
            //Assert.IsFalse( string.IsNullOrEmpty( userAuthentication.DeviceName ) );
            Assert.IsNotNull( userAuthentication.CognitoUser );
            Assert.IsNotNull( userAuthentication.CognitoUser.Device );

        }
        */

        /*
        [TestMethod]
        [ExpectedException( typeof( DeviceNotKnownException ) )]
        public async Task WrongDeviceKey() {

            //Should throw a DeviceNotKnownException, since we're mixing user 1 with user 7 credentials
            var userAuthentication = new UserAuthentication(
                Constants.TestDev1Credentials.Username,
                Constants.TestDev1Credentials.Password,
                Constants.TestDev7Credentials.DeviceKey,
                Constants.TestDev7Credentials.DeviceGroupKey );
            await userAuthentication.InitializeAsync();
        }
        */

        [TestMethod]
        public async Task HappyPathAuthenticationWithExistingTokens() {

            //Log in using email and password. Which will get us a valid set of aws tokens
            var userAuthenticationInit = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await userAuthenticationInit.InitializeAsync();

            //Use the above token to generate a new UserAuthentication and log in with them.
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                userAuthenticationInit.RefreshToken,
                userAuthenticationInit.AccessToken,
                userAuthenticationInit.IdToken,
                userAuthenticationInit.ExpirationTime,
                userAuthenticationInit.IssuedTime );
            await userAuthentication.InitializeAsync();

            //going to test that the RefreshTokenSuccess event does and RefreshedTokensFailed event does not get invoked.
            int onSuccessCount = 0;
            int onFailureCount = 0;
            EventHandler<EventArgs<UserAuthentication>> onSuccessHandler = delegate ( object sender, EventArgs<UserAuthentication> args ) {
                onSuccessCount++;
            };
            EventHandler<EventArgs<UserAuthentication>> onFailureHandler = delegate ( object sender, EventArgs<UserAuthentication> args ) {
                onFailureCount++;
            };
            userAuthentication.OnRefreshTokensSuccessful += onSuccessHandler;
            userAuthentication.OnRefreshTokensFailed += onFailureHandler;

            //Passing true forces the tokens to refresh, regardless of Expiration time. Inreal life, one would not need to call .RefreshToken normally, let alone eith true.
            await userAuthentication.RefreshTokensAsync(true);

            Assert.IsFalse( string.IsNullOrEmpty( userAuthentication.Email ) );
            Assert.IsFalse( string.IsNullOrEmpty( userAuthentication.RefreshToken ) );
            Assert.IsFalse( string.IsNullOrEmpty( userAuthentication.AccessToken ) );
            Assert.IsFalse( string.IsNullOrEmpty( userAuthentication.IdToken ) );
            //Assert.IsFalse( string.IsNullOrEmpty( userAuthentication.DeviceKey ) );
            //Assert.IsFalse( string.IsNullOrEmpty( userAuthentication.DeviceGroupKey ) );
            //Assert.IsFalse( string.IsNullOrEmpty( userAuthentication.DeviceName ) );
            Assert.IsNotNull( userAuthentication.CognitoUser );
            //Assert.IsNotNull( userAuthentication.CognitoUser.Device );

            //Check that the access Id tokens did indeed refresh.
            //QUESTION: Are the supposed to all get refreshed ? 
            Assert.AreNotEqual( userAuthenticationInit.AccessToken, userAuthentication.AccessToken );

            Assert.AreEqual( 1, onSuccessCount );
            Assert.AreEqual( 0, onFailureCount );

            //Make sure we can genreate iam credentials
            await userAuthenticationInit.GenerateIAMCredentialsAsync();

            Assert.IsFalse( string.IsNullOrEmpty( userAuthenticationInit.AccessKey ) );
            Assert.IsFalse( string.IsNullOrEmpty( userAuthenticationInit.SecretKey ) );
            Assert.IsFalse( string.IsNullOrEmpty( userAuthenticationInit.SessionToken ) );
        }

        [TestMethod]
        public async Task GenerateIamCredentials() {

            //Log in using email and password. Which will get us a valid set of aws tokens
            var userAuthenticationInit = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await userAuthenticationInit.InitializeAsync();

            await userAuthenticationInit.GenerateIAMCredentialsAsync();

            Assert.IsFalse( string.IsNullOrEmpty( userAuthenticationInit.AccessKey ) );
            Assert.IsFalse( string.IsNullOrEmpty( userAuthenticationInit.SecretKey ) );
            Assert.IsFalse( string.IsNullOrEmpty( userAuthenticationInit.SessionToken ) );
        }

        [TestMethod]
        public async Task GetCognitoUserDetialsTests() {

            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await userAuthentication.InitializeAsync();

            var userId = await userAuthentication.GetUserIdAsync();

            Assert.AreEqual( Constants.TestDev7UserId, userId );

        }
        
        /// <summary>
         /// Attempts to clean up Devices attached to our test users. This isn't a real unit test, it just cleans things up.
         /// Ideally this method would be marked with the ClassCleanup attribute. However, when doing so, VS doesn't want 
         /// to run any of the tests. To get around this limitation, trying ot run this last. 
         /// </summary>
        [TestMethod]
        public async Task Z_Cleanup() {

            foreach (var users in Constants.TestDevCredentialsList) {
                var userAuthenticationInit = new UserAuthentication(
                    users.Username,
                    users.Password );
                await userAuthenticationInit.InitializeAsync();

                int count = await userAuthenticationInit.CleanUpOldDevicesAsync();
            }
        }

        /// <summary>
        /// The place where Erik will test code, but is not a unit test.
        /// </summary>
        [TestMethod]
        public async Task EriksPlayground() {

            var userAuthentication1 = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await userAuthentication1.InitializeAsync();

            var email = userAuthentication1.Email;
            var refreshToken = userAuthentication1.RefreshToken;
            var accessToken = userAuthentication1.AccessToken;
            var idToken = userAuthentication1.IdToken;
            //var deviceKey = userAuthentication1.DeviceKey;
            //var deviceGroupKey = userAuthentication1.DeviceGroupKey;
            //var deviceName = userAuthentication1.DeviceName;
            var expirationTime = userAuthentication1.ExpirationTime;
            var issuedTime = userAuthentication1.IssuedTime;

            OrionMatchAPIClient matchClient = new OrionMatchAPIClient( Constants.X_API_KEY );
            var getMatch1 = await matchClient.GetMatchAuthenticatedAsync( new MatchID( "1.2038.2024071609575863.0" ), userAuthentication1 );
            Assert.AreEqual( System.Net.HttpStatusCode.OK, getMatch1.StatusCode );

            userAuthentication1.RefreshTokensAsync( true );
            var getMatch2 = await matchClient.GetMatchAuthenticatedAsync( new MatchID( "1.1.2024061414175605.0" ), userAuthentication1 );
            Assert.AreEqual( System.Net.HttpStatusCode.OK, getMatch2.StatusCode );

            //var userAuthentication2 = new UserAuthentication( email, refreshToken, accessToken, idToken, expirationTime, issuedTime, deviceKey, deviceGroupKey );
            //await userAuthentication2.InitializeAsync();
            //var getMatch3 = await matchClient.GetMatchAuthenticatedAsync( new MatchID( "1.2138.2024061413535429.0" ), userAuthentication2 );
            //Assert.AreEqual( System.Net.HttpStatusCode.OK, getMatch3.StatusCode );

            //Assert.AreNotEqual( userAuthentication1.AccessToken, userAuthentication2.AccessToken );
            //Assert.AreNotEqual( userAuthentication1.IdToken, userAuthentication2.IdToken );
            //Assert.AreNotEqual( userAuthentication1.RefreshToken, userAuthentication2.RefreshToken );
        }
    }
}
