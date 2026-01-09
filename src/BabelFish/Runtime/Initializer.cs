using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Scopos.BabelFish.Runtime {
#pragma warning restore IDE0130 // Namespace does not match folder structure

    /// <summary>
    /// Static class, who's Initialize() method should be called at the start of any program using BabelFish.
    /// 
    /// <example>
    /// <code>
    /// var xApiKey = "GyaHV300my60rs2ylKug5aUgFnYBj6GrU6V1WE33";
    /// var localStoreDirectory = @"C:\temp";
    /// Scopos.BabelFish.Runtime.Settings.Initialize( xApiKey, localStoreDirectory, false );
    /// </code>
    /// </example>
    /// </summary>
    public static class Initializer {

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Sets the x-api-key used thorughout Babelfish, and initializes the JSON serializers and deserializers.
        /// <para>To obtain an x-api-key visit <see href="https://support.scopos.tech/index.html?x-api-key.html">Scopos' support pages</see> to learn how.</para>
        /// </summary>
        /// <remarks>The Serializers and Deserializers found in Scopos.BabelFish.Helpers.SerializerOptions class. </remarks>
        /// <param name="xApiKey"></param>
        /// <param name="runPreLoad">If true, pre-loads the definition cache with standard set of Definitions. Intended to be set to false during unit testing to avoid excessive API calls.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Initialize( string xApiKey, bool runPreLoad = true ) {

            if (string.IsNullOrEmpty( xApiKey )) {
                throw new ArgumentNullException( "The x-api-key may not be empty or null." );
            }

            if (_startTime == null)
                _startTime = DateTime.UtcNow;

            Settings.XApiKey = xApiKey;
            CheckForSharedXApiKey();

            SerializerOptions.InitSerializers();

            if (runPreLoad)
                //Choosing not to await the PreLoad command. 
                _ = DefinitionCache.PreLoadAsync();
        }

        /// <summary>
        /// Sets the x-api-key used thorughout Babelfish, and initializes the JSON serializers and deserializers.
        /// <para>To obtain an x-api-key visit <see href="https://support.scopos.tech/index.html?x-api-key.html">Scopos' support pages</see> to learn how.</para>
        /// </summary>
        /// <param name="xApiKey"></param>
        /// <param name="localStoreDirectory">Full path to where cached REST API responses, includeing definition files, are stored.</param>
        /// <param name="runPreLoad">If true, pre-loads the definition cache with standard set of Definitions. Intended to be set to false during unit testing to avoid excessive API calls.</param>
        public static void Initialize( string xApiKey, string localStoreDirectory, bool runPreLoad = true ) {
            Initialize( xApiKey, runPreLoad );

            UpdateLocalStoreDirectory( localStoreDirectory );
        }

        /// <summary>
        /// Sets the full path to where cached REST API responses, includeing definition files, are stored.
        /// <para>The default path is c:/temp/.</para>
        /// <para>The user must have write permission to this directory.</para>
        /// </summary>
        /// <param name="localStoreDirectory"></param>
        public static void UpdateLocalStoreDirectory( string localStoreDirectory ) {

            try {
                var dirInfo = new DirectoryInfo( localStoreDirectory );
                if (!dirInfo.Exists) {
                    dirInfo.Create();
                }

                AthenaAPIClient.LocalStoreDirectory = dirInfo;
                AttributeValueAPIClient.LocalStoreDirectory = dirInfo;
                ClubsAPIClient.LocalStoreDirectory = dirInfo;
                DefinitionAPIClient.LocalStoreDirectory = dirInfo;
                OrionMatchAPIClient.LocalStoreDirectory = dirInfo;
                ScoposDataClient.LocalStoreDirectory = dirInfo;
                ScoreHistoryAPIClient.LocalStoreDirectory = dirInfo;
                SocialNetworkAPIClient.LocalStoreDirectory = dirInfo;

            } catch (Exception ex) {
                _logger.Error( ex, $"Can not set LocalStoreDirectory, with error {ex}." );
            }
        }

        /// <summary>
        /// Gets and Sets the Definition Cache's property to automatically download new definition versions when avaliable from the REST API.
        /// </summary>
        public static bool AutoDownloadNewDefinitionVersions {
            get {
                return DefinitionCache.AutoDownloadNewDefinitionVersions;
            }
            set {
                DefinitionCache.AutoDownloadNewDefinitionVersions = value;
            }
        }

        /// <summary>
        /// Clears all cache from the BabelFish library. With option to run the Definition.PreLoad() method to load common Definition files.
        /// </summary>
        /// <param name="runPreLoad">If true, pre-loads the definition cache with standard set of Definitions. Intended to be set to false during unit testing to avoid excessive API calls.</param>
        public static void ClearCache( bool runPreLoad = true ) {
            DefinitionCache.ClearCache();
            ResponseCache.CACHE.ClearCache();
            EventComposite.ClearCache();
            SetName.ClearCache();

            if (runPreLoad)
                //Choosing not to await the PreLoad command. 
                _ = DefinitionCache.PreLoadAsync();
        }

        private static DateTime? _startTime = null;

        /// <summary>
        /// Returns the uptime of this application.
        /// </summary>
        public static TimeSpan UpTime {
            get {
                if (_startTime == null)
                    _startTime = DateTime.UtcNow;

                return (TimeSpan)(DateTime.UtcNow - _startTime);
            }
        }

        /// <summary>
        /// Checks to see if the user set a shared key as the x-api-key. Provices a warning to the user,
        /// both in the log file and console, if true.
        /// </summary>
        public static void CheckForSharedXApiKey() {

            if (Settings.XApiKey == "GyaHV300my60rs2ylKug5aUgFnYBj6GrU6V1WE33") {
                string message = """
                    ###############################################################################
                    # WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING 
                    # You are using a shared X-API-Key.
                    # While this key will work, it is both rate limited and shared. By using it
                    # you may receive error 429 too many requests, or cause other users to receive
                    # this error.
                    #
                    # Only use this key for limited time. To apply for a key for Orion for
                    # Club license holder use, academic use, or application development vist
                    # Scopos's support website at
                    # https://support.scopos.tech/index.html?x-api-key.html
                    ###############################################################################
                    """;

                Console.WriteLine( message );
                _logger.Warn( message );
            }
        }
    }
}
