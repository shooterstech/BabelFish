﻿
using Scopos.BabelFish.DataModel;

namespace Scopos.BabelFish.DataModel.Authentication {
    /// <summary>
    /// AWS Auth Tokens Returned
    /// </summary>
    [Serializable]
    public class AuthTokens : BaseClass
    {
        public string RefreshToken { get; set; } = string.Empty;

        public string IdToken { get; set; } = string.Empty;

        public string AccessToken { get; set; } = string.Empty;

        public string DeviceToken { get; set; } = string.Empty;
    }
}