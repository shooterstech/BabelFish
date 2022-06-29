namespace ShootersTech.DataModel.Authentication
{
    /// <summary>
    /// AWS Auth Tokens Returned
    /// </summary>
    [Serializable]
    public class AuthTokens
    {
        public string RefreshToken { get; set; } = string.Empty;

        public string IdToken { get; set; } = string.Empty;

        public string AccessToken { get; set; } = string.Empty;

        public string DeviceToken { get; set; } = string.Empty;
    }
}