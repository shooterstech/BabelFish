using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.Runtime {
    public static class Initializer {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Sets the x-api-key used thorughout Babelfish, and initializes the JSON serializers and deserializers.
        /// </summary>
        /// <remarks>The Serializers and Deserializers found in Scopos.BabelFish.Helpers.SerializerOptions class. </remarks>
        /// <param name="xApiKey"></param>
        /// <param name="runPreLoad">If true, pre-loads the definition cache with standard set of Definitions. Intended to be set to false during unit testing to avoid excessive API calls.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Initialize( string xApiKey, bool runPreLoad = true ) {

            if (string.IsNullOrEmpty( xApiKey )) {
                throw new ArgumentNullException( "The x-api-key may not be empty or null." );
			}

			if (StartTime == null)
				StartTime = DateTime.UtcNow;

			Scopos.BabelFish.Runtime.Settings.XApiKey = xApiKey;

            SerializerOptions.InitSerializers();

            if (runPreLoad )
                //Choosing not to await the PreLoad command. 
                DefinitionCache.PreLoad();
        }

        public static void Initialize( string xApiKey, string localStoreDirectory, bool runPreLoad = true ) {
            Initialize( xApiKey, runPreLoad );

            UpdateLocalStoreDirectory( localStoreDirectory );
        }

        public static void UpdateLocalStoreDirectory( string localStoreDirectory ) {

            try {
                var dirInfo = new DirectoryInfo( localStoreDirectory );

                AthenaAPIClient.LocalStoreDirectory = dirInfo;
                AttributeValueAPIClient.LocalStoreDirectory = dirInfo;
                ClubsAPIClient.LocalStoreDirectory = dirInfo;
                DefinitionAPIClient.LocalStoreDirectory = dirInfo;
                OrionMatchAPIClient.LocalStoreDirectory = dirInfo;
                ScoposDataClient.LocalStoreDirectory = dirInfo;
                ScoreHistoryAPIClient.LocalStoreDirectory = dirInfo;
                SocialNetworkAPIClient.LocalStoreDirectory = dirInfo;

            } catch (Exception ex) {
                logger.Error( ex, $"Can not set LocalStoreDirectory, with error {ex}." );
            }
        }

        /// <summary>
        /// Clears all cache from the BabelFish library. With option to run the Definition.PreLoad() method to load common Definition files.
        /// </summary>
        /// <param name="runPreLoad">If true, pre-loads the definition cache with standard set of Definitions. Intended to be set to false during unit testing to avoid excessive API calls.</param>
        public static void ClearCache(bool runPreLoad = true ) {
            DefinitionCache.ClearCache();
            ResponseCache.CACHE.ClearCache();

            if ( runPreLoad)
                //Choosing not to await the PreLoad command. 
                DefinitionCache.PreLoad();
        }

        private static DateTime? StartTime = null;

        public static TimeSpan UpTime {
            get {
                if ( StartTime == null )
                    StartTime = DateTime.UtcNow;

                return (TimeSpan) (DateTime.UtcNow - StartTime);
            }
        }
    }
}
