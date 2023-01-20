using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Amazon.Runtime;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using BabelFish.Runtime.Authentication;
using Amazon.CognitoIdentityProvider.Model;
using NLog;

namespace Scopos.BabelFish.Runtime.Authentication {

    //See:https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/cognito-authentication-extension.html
    //See: https://github.com/aws/aws-sdk-net-extensions-cognito#authenticating-with-secure-remote-protocol-srp

    /// <summary>
    /// Represents a single user (who is hopefully logged in) and their aws cognito credentials. 
    /// 
    /// The code in this class is based on documentation and examples from the following sources.
    /// https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/cognito-authentication-extension.html
    /// https://github.com/aws/aws-sdk-net-extensions-cognito#authenticating-with-secure-remote-protocol-srp
    /// </summary>
    public class UserAuthentication {

        /// <summary>
        /// Called when a new instance of UserAuthentication is constructed using email 
        /// and password, and the authentication into AWS is successful.
        /// </summary>
        public EventHandler<EventArgs> UserAuthenticationSuccessful;

        /// <summary>
        /// Called when a new instance of UserAuthentication is constructed using email 
        /// and password, and the authentication into AWS failed.
        /// </summary>
        public EventHandler<EventArgs> UserAuthenticationFailed;

        //Create a Cognito Identify Provider using anonymous credentials
        private static AmazonCognitoIdentityProviderClient cognitoProvider = new AmazonCognitoIdentityProviderClient( new Amazon.Runtime.AnonymousAWSCredentials() );

        private static CognitoUserPool cognitoUserPool = new CognitoUserPool( AuthenticationConstants.AWSPoolID, AuthenticationConstants.AWSClientID, cognitoProvider );

        private DeviceSecretVerifierConfigType deviceVerifier = null;

        private Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creates a new instance of UserAuthentication and attempts to authenticate
        /// the user (identified by their email) using their password. This flow also assumes
        /// logging on with a new Device and will save the DeviceKey and DeviceGroupKey
        /// to the private variables. The caller is responsible for verifying the device is legit.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public UserAuthentication(string email, string password) {

            logger.Info( $"About to try and authenticate user with email {email} with a new device." );

            this.Email = email;
            this.CognitoUser = new CognitoUser( this.Email, AuthenticationConstants.AWSClientID, cognitoUserPool, cognitoProvider );

            //Starts the authentication process, using only the provided password.
            //This is the only time the password is needed. 
            InitiateSrpAuthRequest authRequest = new InitiateSrpAuthRequest() {
                Password = password
            };

            //Try and authenticate with cognito
            try {
                var taskAuthFlowResponse = this.CognitoUser.StartWithSrpAuthAsync( authRequest );
                var authFlowResponse = taskAuthFlowResponse.Result;

                if (authFlowResponse.AuthenticationResult != null) {
                    //If we get here authentication was successful.
                    this.RefreshToken = authFlowResponse.AuthenticationResult.RefreshToken;
                    this.AccessToken = authFlowResponse.AuthenticationResult.AccessToken;
                    this.IdToken = authFlowResponse.AuthenticationResult.IdToken;
                    this.DeviceKey = authFlowResponse.AuthenticationResult.NewDeviceMetadata.DeviceKey;
                    this.DeviceGroupKey = authFlowResponse.AuthenticationResult.NewDeviceMetadata.DeviceGroupKey;

                    logger.Info( $"Successfully authenticated user with email {email}." );
                    if (UserAuthenticationSuccessful != null)
                        UserAuthenticationSuccessful.Invoke( this, new EventArgs() );
                } else {
                    //If we get there authentication was not successful, b/c we've been given a challenge that needs to be fulfilled.

                    //Not yet sure how best to handle this execution path
                    throw new NotImplementedException();
                }

            } catch (AggregateException ae ) {
                ae.Handle((x) => {
                    if (x is Amazon.CognitoIdentityProvider.Model.NotAuthorizedException) {
                        throw new NotAuthorizedException( x.Message, x, logger );
                    } else {
                        //Not sure what would cause us to get here
                        throw new AuthenticationException( x.Message, x, logger );
                    }
                });
            }

            //After authentication, confirm this device (which is assumed to be a new device) and associated it with the cognito user
            var taskConfirmDeviceResponse = this.CognitoUser.ConfirmDeviceAsync(
                this.AccessToken,
                this.DeviceKey,
                this.DeviceName,
                GetDeviceVerifier().PasswordVerifier,
                GetDeviceVerifier().Salt );
            var confirmDeviceResponse = taskConfirmDeviceResponse.Result;

            CognitoDevice device = new CognitoDevice(
                this.DeviceKey,
                new Dictionary<string, string>(),
                DateTime.Today,
                DateTime.Today,
                DateTime.Today,
                this.CognitoUser );

            var taskGetDevice = device.GetDeviceAsync();
            taskGetDevice.Wait();
            this.CognitoUser.Device = device;
        }

        public UserAuthentication( string email, string password, string deviceKey, string deviceGroupKey ) {

            logger.Info( $"About to try and authenticate user with email {email} with an existing device {deviceKey}." );
            this.Email = email;
            this.DeviceKey= deviceKey;
            this.DeviceGroupKey= deviceGroupKey;
            this.CognitoUser = new CognitoUser( this.Email, AuthenticationConstants.AWSClientID, cognitoUserPool, cognitoProvider );

            InitiateSrpAuthRequest authRequest = new InitiateSrpAuthRequest() {
                Password = password,
                DeviceGroupKey = deviceGroupKey,
                DevicePass = AuthenticationConstants.DevicePassword,
                DeviceVerifier = GetDeviceVerifier().PasswordVerifier
            };

            //Try and authenticate with cognito
            try {
                var taskAuthFlowResponse = this.CognitoUser.StartWithSrpAuthAsync( authRequest );
                var authFlowResponse = taskAuthFlowResponse.Result;

                if (authFlowResponse.AuthenticationResult != null) {
                    //If we get here authentication was successful.
                    this.RefreshToken = authFlowResponse.AuthenticationResult.RefreshToken;
                    this.AccessToken = authFlowResponse.AuthenticationResult.AccessToken;
                    this.IdToken = authFlowResponse.AuthenticationResult.IdToken;

                    logger.Info( $"Successfully authenticated user with email {email}." );
                    if (UserAuthenticationSuccessful != null)
                        UserAuthenticationSuccessful.Invoke( this, new EventArgs() );
                } else {
                    //If we get there authentication was not successful, b/c we've been given a challenge that needs to be fulfilled.

                    //Not yet sure how best to handle this execution path
                    throw new NotImplementedException();
                }

            } catch (AggregateException ae) {
                ae.Handle( ( x ) => {
                    if (x is Amazon.CognitoIdentityProvider.Model.NotAuthorizedException) {
                        throw new NotAuthorizedException( x.Message, x, logger );
                    } else {
                        //Not sure what would cause us to get here
                        throw new AuthenticationException( x.Message, x, logger );
                    }
                } );
            }

            //Oddly, the flow above allows the user to authenticate even when the device is not associated with te user. Howerver, the code below which tries and associates the device with the user will throw an exception if it is not known.

            try {
                CognitoDevice device = new CognitoDevice(
                    this.DeviceKey,
                    new Dictionary<string, string>(),
                    DateTime.Today,
                    DateTime.Today,
                    DateTime.Today,
                    this.CognitoUser );

                var taskGetDevice = device.GetDeviceAsync();
                taskGetDevice.Wait();
                this.CognitoUser.Device = device;
            } catch (AggregateException ae) {
                ae.Handle( ( x ) => {
                    if (x is Amazon.CognitoIdentityProvider.Model.ResourceNotFoundException) {
                        //Thrown if the device is not known to be associated with the user.
                        throw new DeviceNotKnownException( x.Message, x, logger );
                    } else {
                        //Not sure what would cause us to get here
                        throw new AuthenticationException( x.Message, x, logger );
                    }
                } );
            }
        }

        public UserAuthentication(string email, string refreshToken, string accessToken, string iDToken, string deviceKey, string deviceGroupKey) {


            logger.Info( $"About to try and authenticate user with email {email} with an existing device {deviceKey}." );
            this.Email = email;
            this.RefreshToken = refreshToken;
            this.AccessToken = accessToken;
            this.IdToken= iDToken;
            this.DeviceKey = deviceKey;
            this.DeviceGroupKey = deviceGroupKey;
            this.CognitoUser = new CognitoUser( this.Email, AuthenticationConstants.AWSClientID, cognitoUserPool, cognitoProvider );

            try {
                CognitoDevice device = new CognitoDevice(
                    this.DeviceKey,
                    new Dictionary<string, string>(),
                    DateTime.Today,
                    DateTime.Today,
                    DateTime.Today,
                    this.CognitoUser );

                var taskGetDevice = device.GetDeviceAsync();
                taskGetDevice.Wait();
                this.CognitoUser.Device = device;
            } catch (AggregateException ae) {
                ae.Handle( ( x ) => {
                    if (x is Amazon.CognitoIdentityProvider.Model.ResourceNotFoundException) {
                        //Thrown if the device is not known to be associated with the user.
                        throw new DeviceNotKnownException( x.Message, x, logger );
                    } else {
                        //Not sure what would cause us to get here
                        throw new AuthenticationException( x.Message, x, logger );
                    }
                } );
            }
        }

        /// <summary>
        /// Emailaddress the user uses to log in with. It is the same as the user's username
        /// </summary>
        public string Email { get; private set; }

        //NOTE: Purposefully not even keeping a variable for password

        public string RefreshToken { get; private set; }
        public string AccessToken { get; private set; }
        public string IdToken { get; private set; }
        public string DeviceKey { get; private set; }
        public string DeviceGroupKey { get; private set; }
        public string DeviceName {
            get {
                return $"{Email}-{DeviceKey}";
            }
        }

        public CognitoUser CognitoUser { get; private set; }


        /// <summary>
        /// Internal function to generate and return a device verifier.
        /// </summary>
        /// <returns></returns>
        private DeviceSecretVerifierConfigType GetDeviceVerifier() {
            if (deviceVerifier == null) {
                if (string.IsNullOrEmpty( this.DeviceGroupKey ))
                    throw new ArgumentNullException( "In order to generate a device verifier, the property this.DeviceGroupKey must be set." );

                if (string.IsNullOrEmpty( this.Email ))
                    throw new ArgumentNullException( "In order to generate a device verifier, the property this.Email must be set." );

                deviceVerifier = this.CognitoUser.GenerateDeviceVerifier(
                this.DeviceGroupKey,
                AuthenticationConstants.DevicePassword,
                this.Email );

            }

            return deviceVerifier;
        }
    }
}
