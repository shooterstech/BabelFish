using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.Runtime {

    /// <summary>
    /// Globally used values, such as the x api key to use when making Rest API calls.
    /// </summary>
    public static class Settings {

        /// <summary>
        /// Stage used by API clients constructed without an explicit stage.
        /// Set before initialization, which may create clients while preloading definitions.
        /// Changing this value does not change existing clients.
        /// </summary>
        public static APIStage DefaultApiStage { get; set; } = APIStage.PRODUCTION;

        /// <summary>
        /// The X-API-Key to use when making all Scopos REST API calls.
        /// </summary>
        /// <remarks>The value of XApiKey should be set by calling Scopos.BabelFish.Helpers.Initializer.Initialize()</remarks>
        public static string XApiKey {  get; set; } = string.Empty;

        public static void CheckXApiKey() {
            if (string.IsNullOrEmpty(XApiKey)) {
                throw new XApiKeyNotSetException();
            }
        }
    }
}
