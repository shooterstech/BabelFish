using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Amazon.Runtime;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.CognitoIdentity.Model;
using NLog;
using Scopos.BabelFish.Runtime;

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
        /// Invoked when a new instance of UserAuthentication is constructed using email 
        /// and password, and the authentication into AWS is successful.
        /// </summary>
        public EventHandler<EventArgs> OnUserAuthenticationSuccessful;

        /// <summary>
        /// Invoked when a new instance of UserAuthentication is constructed using email 
        /// and password, and the authentication into AWS failed.
        /// </summary>
        public EventHandler<EventArgs> OnUserAuthenticationFailed;

        /// <summary>
        /// Invoked when a already authenticated user has their Cognito tokens refreshed successfully.
        /// </summary>
        public EventHandler<EventArgs<UserAuthentication>> OnRefreshTokensSuccessful;

        /// <summary>
        /// Invoked when a previously authenticated user attempts to refresh their cognito tokens, 
        /// but the process is unsuccessful.
        /// </summary>
        public EventHandler<EventArgs<UserAuthentication>> OnRefreshTokensFailed;

        public EventHandler<EventArgs<UserAuthentication>> OnGenerateIAMCredentialsSuccessful; 
        public EventHandler<EventArgs<UserAuthentication>> OnGenerateIAMCredentialsFailed;

        //Create a Cognito Identify Provider using anonymous credentials
        private static AmazonCognitoIdentityProviderClient cognitoProvider = new AmazonCognitoIdentityProviderClient( new AnonymousAWSCredentials() );

        private static CognitoUserPool cognitoUserPool = new CognitoUserPool( AuthenticationConstants.AWSPoolID, AuthenticationConstants.AWSClientID, cognitoProvider );

        private static AmazonCognitoIdentityClient identityClient =
            new AmazonCognitoIdentityClient( new AnonymousAWSCredentials() );

        private DeviceSecretVerifierConfigType deviceVerifier = null;

        private Logger logger = LogManager.GetCurrentClassLogger();


        private enum ConstructorType { EMAIL_PASSWORD, EMAIL_PASSWORD_DEVICE, REFRESH_TOKEN };
        private ConstructorType constructorType;
        private bool initCalled = false;
        private InitiateSrpAuthRequest authRequest;
        private string userId = "";

        /// <summary>
        /// Creates a new instance of UserAuthentication and attempts to authenticate
        /// the user (identified by their email) using their password. This flow also assumes
        /// logging on with a new Device and will save the DeviceKey and DeviceGroupKey
        /// to the private variables. The caller is responsible for verifying the device is legit.
        /// 
        /// After the contructor returns, call InitializeAsync() to complete the constructor / initialziation process.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public UserAuthentication( string email, string password ) {

            logger.Info( $"About to try and authenticate user with email {email} with a new device." );

            this.Email = email;
            this.CognitoUser = new CognitoUser( this.Email, AuthenticationConstants.AWSClientID, cognitoUserPool, cognitoProvider );

            //Starts the authentication process, using only the provided password.
            //This is the only time the password is needed. 
            authRequest = new InitiateSrpAuthRequest() {
                Password = password
            };

            initCalled = false;
            constructorType = ConstructorType.EMAIL_PASSWORD;
        }

        /// <summary>
        /// Creates a new instance of UserAuthentication and attempts to authenticate
        /// the user (identified by their email) using their password, deviceKey
        /// and deviceGroupKey.
        /// 
        /// After the contructor returns, call InitializeAsync() to complete the constructor / initialziation process.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="deviceKey"></param>
        /// <param name="deviceGroupKey"></param>
        /// <exception cref="NotImplementedException"></exception>
        /// <exception cref="NotAuthorizedException"></exception>
        /// <exception cref="AuthenticationException"></exception>
        /// <exception cref="DeviceNotKnownException"></exception>
        public UserAuthentication( string email, string password, string deviceKey, string deviceGroupKey ) {

            logger.Info( $"About to try and authenticate user with email {email} with an existing device {deviceKey}." );
            this.Email = email;
            this.DeviceKey = deviceKey;
            this.DeviceGroupKey = deviceGroupKey;
            this.CognitoUser = new CognitoUser( this.Email, AuthenticationConstants.AWSClientID, cognitoUserPool, cognitoProvider );

            //NOTE: Adding the Device to the user before calling .StartWithSrpAuthAsync will result in a bad password error.
            //this.CognitoUser.Device = new CognitoDevice( new DeviceType() { DeviceKey = deviceKey }, this.CognitoUser );

            //NOTE: This mehtod, with passing the DeviceGroupKey and DevicePass seems to work, in that is passes authentication, however, it generates a new device
            authRequest = new InitiateSrpAuthRequest() {
                Password = password,
                DeviceGroupKey = deviceGroupKey,
                DevicePass = AuthenticationConstants.DevicePassword
            };

            initCalled = false;
            constructorType = ConstructorType.EMAIL_PASSWORD_DEVICE;
        }

        /// <summary>
        /// Constructs a new User Authentication instance. To complete the re-authentication process, user should call .RefreshedTokens().
        /// 
        /// After the contructor returns, call InitializeAsync() to complete the constructor / initialziation process.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="refreshToken"></param>
        /// <param name="accessToken"></param>
        /// <param name="idToken"></param>
        /// <param name="expirationTime"></param>
        /// <param name="issuedTime"></param>
        /// <param name="deviceKey"></param>
        /// <param name="deviceGroupKey"></param>
        /// <exception cref="DeviceNotKnownException">Thrown if the device is not known to be assciated with the user.</exception>
        /// <exception cref="AuthenticationException">Thrown if the user could not be re-authenticated.</exception>
        public UserAuthentication( string email, string refreshToken, string accessToken, string idToken, DateTime expirationTime, DateTime issuedTime, string deviceKey, string deviceGroupKey) {

            logger.Info( $"About to try and re-authenticate user with email {email} with an existing device {deviceKey}." );
            this.Email = email;
            this.RefreshToken = refreshToken;
            this.AccessToken = accessToken;
            this.IdToken = idToken;
            this.ExpirationTime = expirationTime;
            this.IssuedTime = issuedTime;
            this.DeviceKey = deviceKey;
            this.DeviceGroupKey = deviceGroupKey;
            this.CognitoUser = new CognitoUser( this.Email, AuthenticationConstants.AWSClientID, cognitoUserPool, cognitoProvider );

            this.CognitoUser.SessionTokens = new CognitoUserSession( idToken, accessToken, refreshToken, issuedTime, expirationTime );

            initCalled = false;
            constructorType = ConstructorType.REFRESH_TOKEN;
        }

        /// <summary>
        /// To complete the Constructor process a number of Async calls need to be made. Because it is not possible
        /// (at least not wise) to call Async within a constructor this InitializaAsync() method is used to complete
        /// the process. It should be called immediatly after the Constructor.
        /// 
        /// If InitializeAsync() is not called an exception InitializeAsyncNotCompletedException() will be thrown.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <exception cref="NotAuthorizedException"></exception>
        /// <exception cref="AuthenticationException"></exception>
        /// <exception cref="DeviceNotKnownException"></exception>
        public async Task InitializeAsync() {

            CognitoDevice device;

            switch (constructorType) {
                case ConstructorType.EMAIL_PASSWORD:
                    //Try and authenticate with cognito
                    try {
                        var authFlowResponse = await this.CognitoUser.StartWithSrpAuthAsync( authRequest );

                        if (authFlowResponse.AuthenticationResult != null) {
                            //If we get here authentication was successful.
                            this.RefreshToken = authFlowResponse.AuthenticationResult.RefreshToken;
                            this.AccessToken = authFlowResponse.AuthenticationResult.AccessToken;
                            this.IdToken = authFlowResponse.AuthenticationResult.IdToken;
                            this.DeviceKey = authFlowResponse.AuthenticationResult.NewDeviceMetadata.DeviceKey;
                            this.DeviceGroupKey = authFlowResponse.AuthenticationResult.NewDeviceMetadata.DeviceGroupKey;
                            this.ExpirationTime = this.CognitoUser.SessionTokens.ExpirationTime;
                            this.IssuedTime = this.CognitoUser.SessionTokens.IssuedTime;

                            logger.Info( $"Successfully authenticated user with email {this.Email}." );
                            if (OnUserAuthenticationSuccessful != null)
                                OnUserAuthenticationSuccessful.Invoke( this, new EventArgs() );
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

                    //After authentication, confirm this device (which is assumed to be a new device) and associated it with the cognito user
                    var confirmDeviceResponse =  await this.CognitoUser.ConfirmDeviceAsync(
                        this.AccessToken,
                        this.DeviceKey,
                        this.DeviceName,
                        GetDeviceVerifier().PasswordVerifier,
                        GetDeviceVerifier().Salt );

                    device = new CognitoDevice(
                        this.DeviceKey,
                        new Dictionary<string, string>(),
                        DateTime.Today,
                        DateTime.Today,
                        DateTime.Today,
                        this.CognitoUser );

                    await device.GetDeviceAsync();
                    this.CognitoUser.Device = device;

                    break;

                case ConstructorType.EMAIL_PASSWORD_DEVICE:

                    //Try and authenticate with cognito
                    try {
                        var authFlowResponse = await this.CognitoUser.StartWithSrpAuthAsync( authRequest );

                        if (authFlowResponse.AuthenticationResult != null) {
                            //If we get here authentication was successful.
                            this.RefreshToken = authFlowResponse.AuthenticationResult.RefreshToken;
                            this.AccessToken = authFlowResponse.AuthenticationResult.AccessToken;
                            this.IdToken = authFlowResponse.AuthenticationResult.IdToken;
                            this.ExpirationTime = this.CognitoUser.SessionTokens.ExpirationTime;
                            this.IssuedTime = this.CognitoUser.SessionTokens.IssuedTime;

                            logger.Info( $"Successfully authenticated user with email {this.Email}." );
                            if (OnUserAuthenticationSuccessful != null)
                                OnUserAuthenticationSuccessful.Invoke( this, new EventArgs() );
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
                        device = new CognitoDevice(
                            this.DeviceKey,
                            new Dictionary<string, string>(),
                            DateTime.Today,
                            DateTime.Today,
                            DateTime.Today,
                            this.CognitoUser );

                        await device.GetDeviceAsync();
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
                    } catch (Amazon.CognitoIdentityProvider.Model.ResourceNotFoundException rnfe) {
                        throw new DeviceNotKnownException( rnfe.Message, rnfe, logger );
                    } 

                    break;

                case ConstructorType.REFRESH_TOKEN:

                    try {
                        device = new CognitoDevice(
                            this.DeviceKey,
                            new Dictionary<string, string>(),
                            DateTime.Today,
                            DateTime.Today,
                            DateTime.Today,
                            this.CognitoUser );

                        await device.GetDeviceAsync();
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

                    break ;
            }

            //Mark that this instance has finished the initalization process
            initCalled = true;
        }

        /// <summary>
        /// Attempts to refresh the user's cognito tokens. 
        /// Invokes the RefreshTokensSuccessful when the tokens are refreshed.
        /// Invoked the RefreshTokensFailed when a failure happens (and then throws one of the Exceptions).
        /// </summary>
        /// <exception cref="AuthenticationException">Thrown if the user could not be re-authenticated.</exception>
        /// <exception cref="ScoposException">Thrown if, not sure why, but maybe a networking issue preventing the re-authentication.</exception>
        /// <exception cref="InitializeAsyncNotCompletedException">Thrown if InitializeAsync() was not called after calling the UserAuthentication constructor.</exception>
        public async Task RefreshTokensAsync(bool refreshNow = false) {

            if (!initCalled)
                throw new InitializeAsyncNotCompletedException( "InitializeAsync() was not called after the UserAuthentication constructor. Can not proceed until after this call is successful." );

            if ( !refreshNow && this.CognitoUser.SessionTokens.ExpirationTime > DateTime.UtcNow.AddMinutes( 1 ) ) {
                logger.Info( $"Purposefully not refreshing tokens for {this.Email} as the ExpirationTime is in the future." );
                return;
            }

            try {
                //We dont' know the actual experation date of the token, so setting it to 1 hour and then will re-fresh them regardless.
                //this.CognitoUser.SessionTokens = new CognitoUserSession( null, null, this.RefreshToken, DateTime.UtcNow, DateTime.UtcNow.AddHours( 1 ) );

                //Create the refresh request object
                InitiateRefreshTokenAuthRequest refreshRequest = new InitiateRefreshTokenAuthRequest() {
                    AuthFlowType = AuthFlowType.REFRESH_TOKEN_AUTH
                };

                //CAll Cognito to refresh the token
                var authFlowResponse = await this.CognitoUser.StartWithRefreshTokenAuthAsync( refreshRequest );

                //Now we have a new accessToken and a new refreshToken, both of which need to be re-saved
                if (authFlowResponse.AuthenticationResult != null) {
                    this.AccessToken = authFlowResponse.AuthenticationResult.AccessToken;
                    this.RefreshToken = authFlowResponse.AuthenticationResult.RefreshToken;
                    this.ExpirationTime = this.CognitoUser.SessionTokens.ExpirationTime;
                    this.IssuedTime = this.CognitoUser.SessionTokens.IssuedTime;

                    if (OnRefreshTokensSuccessful != null)
                        OnRefreshTokensSuccessful.Invoke( this, new EventArgs<UserAuthentication>( this ) );
                } else {

                    //Not sure yet what would cause us to get here.

                    if (OnRefreshTokensFailed != null)
                        OnRefreshTokensFailed.Invoke( this, new EventArgs<UserAuthentication>( this ) );

                    throw new AuthenticationException( $"Unable to perform a token refresh for {this.Email}. Calls to cognito returned, but without reauthenticating the user.", logger );
                }
            } catch ( Exception ex ) {

                //Best guess to get here would be a networking issue. But that's only a guess.

                if (OnRefreshTokensFailed != null)
                    OnRefreshTokensFailed.Invoke( this, new EventArgs<UserAuthentication>( this ) );

                throw new ScoposException( $"Unable to perform a token refresh for {this.Email}.", logger );

            }
        }
        /// <summary>
        /// Emailaddress the user uses to log in with. It is the same as the user's username
        /// </summary>
        public string Email { get; private set; }

        public async Task<string> GetUserIdAsync() {
            if (string.IsNullOrEmpty( this.userId )) {
                var userDetails = await this.CognitoUser.GetUserDetailsAsync();
                this.userId = userDetails.Username;
            }

            return this.userId;
        }

        //NOTE: Purposefully not even keeping a variable for password

        public string RefreshToken { get; private set; }
        public string AccessToken { get; private set; }
        //The expiration time of the access token .. I think
        public DateTime ExpirationTime { get; private set; }
        public DateTime IssuedTime { get; private set; }
        public string IdToken { get; private set; }
        public string DeviceKey { get; private set; }
        public string DeviceGroupKey { get; private set; }
        public string DeviceName {
            get {
                return $"{Email}-{DeviceKey}";
            }
        }
        public DateTime IamCredentialsExpiration { get; private set; } = DateTime.MaxValue;
        public string AccessKey { get; private set; }
        public string SecretKey { get; private set; }
        public string SessionToken { get; private set; }
        public ImmutableCredentials ImmutableCredentials { get; private set; }

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

        /// <summary>
        /// Generates the temporary IAM credentials for the logged in user. These would be 
        /// AccessKey, SecretKey, and SessionToken.
        /// Invokes the GenerateIAMCredentialsSuccessful event on success, and GenerateIAMCredentialsFailed on failure. 
        /// If the credentials do not need to be refreshed (have not expired yet), neither event is invoked.
        /// </summary>
        /// <exception cref="ScoposException">Thrown when we are unabled to retreive temporary credentials. </exception>
        /// <exception cref="InitializeAsyncNotCompletedException">Thrown if InitializeAsync() was not called after calling the UserAuthentication constructor.</exception>
        public async Task GenerateIAMCredentialsAsync() {

            //Call RefreshTokens to reauthenticate if needed.
            await this.RefreshTokensAsync();

            //Only generate if the IAM credentials are empty or its been over an hour, which is when they expire.
            if (IamCredentialsExpiration < DateTime.UtcNow)
                return;

            try {
                //Using the idToken, obtain the access key, secret access key, and session token
                //First step get the Id of the Cognito User
                var logins = new Dictionary<string, string>() {
                    { $"cognito-idp.{AuthenticationConstants.AWSRegion}.amazonaws.com/{AuthenticationConstants.AWSPoolID}", this.IdToken }
                };

                var getIDRequest = new Amazon.CognitoIdentity.Model.GetIdRequest() {
                    AccountId = AuthenticationConstants.AWSAccountID,
                    IdentityPoolId = AuthenticationConstants.AWSIdentityPoolID,
                    Logins = logins
                };

                var getIdResponse = await identityClient.GetIdAsync( getIDRequest );

                //Second step, get the Credentials for the Identity we just learned
                var getCredentialsForIdentityRequest = new GetCredentialsForIdentityRequest() {
                    IdentityId = getIdResponse.IdentityId,
                    Logins = logins
                };

                var getCredentialsForIdentityResponse = await identityClient.GetCredentialsForIdentityAsync( getCredentialsForIdentityRequest );

                IamCredentialsExpiration = getCredentialsForIdentityResponse.Credentials.Expiration;
                AccessKey = getCredentialsForIdentityResponse.Credentials.AccessKeyId;
                SecretKey = getCredentialsForIdentityResponse.Credentials.SecretKey;
                SessionToken = getCredentialsForIdentityResponse.Credentials.SessionToken;

                ImmutableCredentials = new ImmutableCredentials( AccessKey, SecretKey, SessionToken );

                if (OnGenerateIAMCredentialsSuccessful != null)
                    OnGenerateIAMCredentialsSuccessful.Invoke( this, new EventArgs<UserAuthentication>( this ) );

            } catch (Exception ex) {
                //Currently not sure what would cause an exception to be thrown, possible networking issues, but catch them anyway.

                if (OnGenerateIAMCredentialsFailed != null)
                    OnGenerateIAMCredentialsFailed.Invoke( this, new EventArgs<UserAuthentication>( this ) );

                throw new ScoposException( $"Unable to get IAM credentials for {this.Email}", ex, logger );
            }
        }

        
        /// <summary>
        /// Removes devices from the user, if they have not been used in the last 45 days.
        /// </summary>
        /// <returns></returns>
        public async Task<int> CleanUpOldDevicesAsync() {

            var listOfDevices = await this.CognitoUser.ListDevicesAsync( 60, null );
            var count = 0;

            foreach ( var device in listOfDevices ) {
                if ( (DateTime.Now - device.LastAuthenticated).TotalDays > 45 ) {
                    await device.ForgetDeviceAsync();
                    count++;
                }
            }

            return count;
        }
    }
}
