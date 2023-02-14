﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void HappyPathAuthenticationWithNewDevice() {

            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );

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
        [ExpectedException( typeof( NotAuthorizedException ) )]
        public void WrongPassword() {

            //Should throw a NotAuthroizedException
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                "not the right password" );
        }

        [TestMethod]
        public void HappyPathAuthenticationWithExistingDevice() {

            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password,
                Constants.TestDev7Credentials.DeviceKey,
                Constants.TestDev7Credentials.DeviceGroupKey );

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
        public void WrongDeviceKey() {

            //Should throw a DeviceNotKnownException, since we're mixing user 1 with user 7 credentials
            var userAuthentication = new UserAuthentication(
                Constants.TestDev1Credentials.Username,
                Constants.TestDev1Credentials.Password,
                Constants.TestDev7Credentials.DeviceKey,
                Constants.TestDev7Credentials.DeviceGroupKey );
        }

        [TestMethod]
        public void HappyPathAuthenticationWithExistingTokens() {

            //Log in using email and password. Which will get us a valid set of aws tokens
            var userAuthenticationInit = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );

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
            userAuthentication.RefreshTokens(true);

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
            userAuthenticationInit.GenerateIAMCredentials();

            Assert.IsFalse( string.IsNullOrEmpty( userAuthenticationInit.AccessKey ) );
            Assert.IsFalse( string.IsNullOrEmpty( userAuthenticationInit.SecretKey ) );
            Assert.IsFalse( string.IsNullOrEmpty( userAuthenticationInit.SessionToken ) );
        }

        [TestMethod]
        public void GenerateIamCredentials() {

            //Log in using email and password. Which will get us a valid set of aws tokens
            var userAuthenticationInit = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );

            userAuthenticationInit.GenerateIAMCredentials();

            Assert.IsFalse( string.IsNullOrEmpty( userAuthenticationInit.AccessKey ) );
            Assert.IsFalse( string.IsNullOrEmpty( userAuthenticationInit.SecretKey ) );
            Assert.IsFalse( string.IsNullOrEmpty( userAuthenticationInit.SessionToken ) );
        }
    }
}