using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.Runtime.Authentication;
using Scopos.BabelFish.Runtime;

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
            Assert.IsFalse( string.IsNullOrEmpty( userAuthentication.DeviceKey ) );
            Assert.IsFalse( string.IsNullOrEmpty( userAuthentication.DeviceGroupKey ) );
            Assert.IsFalse( string.IsNullOrEmpty( userAuthentication.DeviceName ) );
            Assert.IsNotNull( userAuthentication.CognitoUser );
            Assert.IsNotNull( userAuthentication.CognitoUser.Device );

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
            Assert.IsFalse( string.IsNullOrEmpty( userAuthentication.DeviceKey ) );
            Assert.IsFalse( string.IsNullOrEmpty( userAuthentication.DeviceGroupKey ) );
            Assert.IsFalse( string.IsNullOrEmpty( userAuthentication.DeviceName ) );
            Assert.IsNotNull( userAuthentication.CognitoUser );
            Assert.IsNotNull( userAuthentication.CognitoUser.Device );

        }

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
                userAuthenticationInit.IssuedTime,
                userAuthenticationInit.DeviceKey,
                userAuthenticationInit.DeviceGroupKey );
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
            Assert.IsFalse( string.IsNullOrEmpty( userAuthentication.DeviceKey ) );
            Assert.IsFalse( string.IsNullOrEmpty( userAuthentication.DeviceGroupKey ) );
            Assert.IsFalse( string.IsNullOrEmpty( userAuthentication.DeviceName ) );
            Assert.IsNotNull( userAuthentication.CognitoUser );
            Assert.IsNotNull( userAuthentication.CognitoUser.Device );

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
    }
}
