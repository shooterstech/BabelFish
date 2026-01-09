using System.Collections.Concurrent;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Requests.DefinitionAPI;
using Scopos.BabelFish.Responses.DefinitionAPI;

namespace Scopos.BabelFish.APIClients {
    public static class DefinitionCache {

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        /*
         * We want to cache both the definition and if a set name is not found (likely b/c it doesn't exist). However, it has been
         * observed (annoyingly) that sometimes the Rest API will return a Not Found, even with the definition exists. As such, we need
         * a mechanism to re-check periodically. We do that by recording whne the NotFound was last observed. 
         */
        private static ConcurrentDictionary<SetName, Scopos.BabelFish.DataModel.Definitions.Attribute> AttributeCache = new ConcurrentDictionary<SetName, Scopos.BabelFish.DataModel.Definitions.Attribute>();
        private static ConcurrentDictionary<SetName, DateTime> AttributeNotFoundCache = new ConcurrentDictionary<SetName, DateTime>();

        private static ConcurrentDictionary<SetName, CourseOfFire> CourseOfFireCache = new ConcurrentDictionary<SetName, CourseOfFire>();
        private static ConcurrentDictionary<SetName, DateTime> CourseOfFireNotFoundCache = new ConcurrentDictionary<SetName, DateTime>();

        private static ConcurrentDictionary<SetName, EventAndStageStyleMapping> EventAndStageStyleMappingCache = new ConcurrentDictionary<SetName, EventAndStageStyleMapping>();
        private static ConcurrentDictionary<SetName, DateTime> EventAndStageStyleMappingNotFoundCache = new ConcurrentDictionary<SetName, DateTime>();

        private static ConcurrentDictionary<SetName, EventStyle> EventStyleCache = new ConcurrentDictionary<SetName, EventStyle>();
        private static ConcurrentDictionary<SetName, DateTime> EventStyleNotFoundCache = new ConcurrentDictionary<SetName, DateTime>();

        private static ConcurrentDictionary<SetName, RankingRule> RankingRuleCache = new ConcurrentDictionary<SetName, RankingRule>();
        private static ConcurrentDictionary<SetName, DateTime> RankingRuleNotFoundCache = new ConcurrentDictionary<SetName, DateTime>();

        private static ConcurrentDictionary<SetName, ResultListFormat> ResultListFormatCache = new ConcurrentDictionary<SetName, ResultListFormat>();
        private static ConcurrentDictionary<SetName, DateTime> ResultListFormatNotFoundCache = new ConcurrentDictionary<SetName, DateTime>();

        private static ConcurrentDictionary<SetName, ScoreFormatCollection> ScoreFormatCollectionCache = new ConcurrentDictionary<SetName, ScoreFormatCollection>();
        private static ConcurrentDictionary<SetName, DateTime> ScoreFormatCollectionNotFoundCache = new ConcurrentDictionary<SetName, DateTime>();

        private static ConcurrentDictionary<SetName, StageStyle> StageStyleCache = new ConcurrentDictionary<SetName, StageStyle>();
        private static ConcurrentDictionary<SetName, DateTime> StageStyleNotFoundCache = new ConcurrentDictionary<SetName, DateTime>();

        private static ConcurrentDictionary<SetName, Target> TargetCache = new ConcurrentDictionary<SetName, Target>();
        private static ConcurrentDictionary<SetName, DateTime> TargetNotFoundCache = new ConcurrentDictionary<SetName, DateTime>();

        private static ConcurrentDictionary<SetName, TargetCollection> TargetCollectionCache = new ConcurrentDictionary<SetName, TargetCollection>();
        private static ConcurrentDictionary<SetName, DateTime> TargetCollectionNotFoundCache = new ConcurrentDictionary<SetName, DateTime>();


        private const int NOT_FOUND_RECHECK_TIME = 60; //In seconds

        /// <summary>
        /// Set to true, to allow the Definition Cache to automatically check, and if avaliable, downlaod newer minor versions of Definition Files.
        /// <para>The runtime.Initializer has the same property .AutoDownloadNewDefinitionVersions, as an easier facade to set / unset.</para>
        /// </summary>
        /// 
        /// <remarks>
        /// <list type="bullet">
        /// <listheader>In general the value of AutoDownloadNewDefinitionVersions shoudl be </listheader>
        /// <item>true, if running on a server (e.g. Rezults).</item>
        /// <item>false, if running on a stand alone application (e.g. Orion).</item>
        /// </list>
        /// </remarks>
        public static bool AutoDownloadNewDefinitionVersions = false;

        /// <summary>
        /// Preloads the Definiiton Cache with commmon definitions. If used, should help with some start up time.
        /// </summary>
        /// <returns></returns>
        public static async Task PreLoadAsync() {
            List<string> attributeDefinitionsToLoad = new List<string>() {
                "v1.0:ntparc:Three-Position Air Rifle Type",
                "v1.0:orion:Profile Name",
                "v1.0:orion:Date of Birth",
                "v2.0:orion:Email Address",
                "v2.0:orion:Phone Number",
                "v1.0:orion:Address",
                "v1.0:orion:High School Graduating Class",
                "v1.0:orion:Pronouns",
                "v1.0:orion:Gender",
                "v1.0:orion:Collegiate Class",
                "v1.0:orion:NCAA ID",
                "v1.0:orion:Air Rifle Training Category",
                "v1.0:orion:Air Pistol Training Category",
                "v1.0:orion:Hometown",
                "v1.0:orion:Account URL",
                "v1.0:orion:Score History Public",
                "v1.0:orion:Score History Personal",
                "v1.0:orion:Score History Coach",
                "v1.0:usas:Paralympic" };

            List<string> scoreFormatsToLoad = new List<string>() {
                "v1.0:orion:Standard Score Formats",
                "v1.0:orion:Standard Averaged Score Formats"
            };

            List<string> targetDefinitionsToLoad = new List<string>() {
                 "v1.0:issf:10m Air Rifle",
                 "v1.0:issf:10m Air Pistol"
            };

            List<SetName> attributeSetNamesToTryAgain = new List<SetName>();
            List<SetName> scoreFormatSetNamesToTryAgain = new List<SetName>();
            List<SetName> targetSetNamesToTryAgain = new List<SetName>();

            //First attempt 
            foreach (var definition in attributeDefinitionsToLoad) {
                var setName = SetName.Parse( definition );
                try {
                    await GetAttributeDefinitionAsync( setName );
                } catch (DefinitionNotFoundException) {
                    //We should never get here, if we do, there was an error in the Rest API
                    attributeSetNamesToTryAgain.Add( setName );
                    AttributeNotFoundCache.Clear();
                } catch (Exception e) {
                    //We should never get here, if we do, there was an error in the Rest API
                    attributeSetNamesToTryAgain.Add( setName );
                }
            }

            foreach (var definition in scoreFormatsToLoad) {
                var setName = SetName.Parse( definition );
                try {
                    await GetScoreFormatCollectionDefinitionAsync( setName );
                } catch (DefinitionNotFoundException) {
                    //We should never get here, if we do, there was an error in the Rest API
                    scoreFormatSetNamesToTryAgain.Add( setName );
                    AttributeNotFoundCache.Clear();
                } catch (Exception e) {
                    //We should never get here, if we do, there was an error in the Rest API
                    scoreFormatSetNamesToTryAgain.Add( setName );
                }
            }

            foreach (var definition in targetDefinitionsToLoad) {
                var setName = SetName.Parse( definition );
                try {
                    await GetTargetDefinitionAsync( setName );
                } catch (DefinitionNotFoundException) {
                    //We should never get here, if we do, there was an error in the Rest API
                    targetSetNamesToTryAgain.Add( setName );
                    AttributeNotFoundCache.Clear();
                } catch (Exception e) {
                    //We should never get here, if we do, there was an error in the Rest API
                    targetSetNamesToTryAgain.Add( setName );
                }
            }

            //Second attempt
            if (attributeSetNamesToTryAgain.Count > 0
                || scoreFormatSetNamesToTryAgain.Count > 0
                || targetSetNamesToTryAgain.Count > 0) {
                Thread.Sleep( 10000 );

                foreach (var sn in attributeSetNamesToTryAgain) {
                    try {
                        await GetAttributeDefinitionAsync( sn );
                    } catch (Exception) {
                        ;
                    }
                }

                foreach (var sn in scoreFormatSetNamesToTryAgain) {
                    try {
                        await GetScoreFormatCollectionDefinitionAsync( sn );
                    } catch (Exception) {
                        ;
                    }
                }

                foreach (var sn in targetSetNamesToTryAgain) {
                    try {
                        await GetTargetDefinitionAsync( sn );
                    } catch (Exception) {
                        ;
                    }
                }
            }
        }

        #region Attribute

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        /// /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        /// <exception cref="DefinitionNotFoundException" />
        /// <exception cref="ScoposAPIException" />
        public static async Task<Scopos.BabelFish.DataModel.Definitions.Attribute> GetAttributeDefinitionAsync( SetName setName ) {

            //Try and pull from cache. If there is a cached value, in a seperate Task (note we are not calling await) check for a newer version
            if (AttributeCache.TryGetValue( setName, out Scopos.BabelFish.DataModel.Definitions.Attribute a )) {

                if (AutoDownloadNewDefinitionVersions)
                    //Purposefully not awaiting this call
                    DownloadNewMinorVersionIfAvaliableAsync( a );

                return a;
            }

            DateTime lastChecked;
            if (AttributeNotFoundCache.TryGetValue( setName, out lastChecked ) && (DateTime.UtcNow - lastChecked).TotalSeconds < NOT_FOUND_RECHECK_TIME)
                throw new DefinitionNotFoundException( $"Attribute definition '{setName}' not found. " );

            var response = await DefinitionFetcher.FETCHER.GetAttributeDefinitionAsync( setName );
            if (response.HasOkStatusCode) {
                var definition = response.Definition;

                AttributeCache.TryAdd( setName, definition );
                return definition;
            } else if (response.RestApiStatusCode == System.Net.HttpStatusCode.NotFound) {
                //Also cache any NotFound requests
                AttributeNotFoundCache.TryAdd( setName, DateTime.UtcNow );
                throw new DefinitionNotFoundException( $"Attribute definition '{setName}' not found. " );
            } else {
                throw new ScoposAPIException( $"Unable to retreive Attribute definition {setName}. Overall: {response.OverallStatusCode}, REST API {response.RestApiStatusCode}" );
            }
        }

        /// <summary>
        /// Tries and returns the Attribute requested, if it has already been loaded into the cache.
        /// Returns false, if it has not been loaded yet. Then tries and reads or downloads it in the background. Which means 
        /// the definition may be avalaible at a latter time (once the getting is successful).
        /// </summary>
        /// <param name="setName"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool TryGetAttributeDefinition( SetName setName, out Scopos.BabelFish.DataModel.Definitions.Attribute def ) {

            //Try and read from the cache. If its previously been loaded, then return it. 
            if (AttributeCache.TryGetValue( setName, out def )) {

                return true;
            }

            //If it is not loaded, make a call to read / download it.
            //Purposefully not awaiting this call. This way this method may remain synchronous, and the download can happen in the background.
            GetAttributeDefinitionAsync( setName );

            return false;
        }

        /// <summary>
        /// Method checks to see if there is a new minor release avaliable for the past in Definition.
        /// If so, it tries and download it and update the cache.
        /// </summary>
        /// <param name="def"></param>
        /// <returns>Boolean indicating if there was a new minor release avaliable and if it was successful in downloading it.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static async Task<bool> DownloadNewMinorVersionIfAvaliableAsync( Scopos.BabelFish.DataModel.Definitions.Attribute def ) {
            if (def == null)
                throw new ArgumentNullException( nameof( def ) );

            try {
                if (await def.IsVersionUpdateAvaliableAsync()) {

                    SetName setName = def.GetSetName( true );

                    //Make a request, that ignores all of our local caching
                    var request = new GetDefinitionPublicRequest( setName, def.Type ) {
                        IgnoreInMemoryCache = true,
                        IgnoreFileSystemCache = true,
                        IgnoreRestAPICache = true
                    };
                    var response = new GetDefinitionPublicResponse<Scopos.BabelFish.DataModel.Definitions.Attribute>( request );

                    await DefinitionFetcher.FETCHER.GetDefinitionAsync<Scopos.BabelFish.DataModel.Definitions.Attribute>( request, response );
                    if (response.HasOkStatusCode) {
                        AttributeCache[setName] = response.Definition;
                        return true;
                    }
                }
            } catch (Exception ex) {
                _logger.Error( ex, $"Caught error while trying to check and download if there was a new {def.Type} Definition for {def.SetName}" );
                //Swallowing the error, as its simple enough to say the operation was not successful.
            }

            return false;
        }

        #endregion

        #region CourseOfFire

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        /// <exception cref="DefinitionNotFoundException" />
        /// <exception cref="ScoposAPIException" />
        public static async Task<CourseOfFire> GetCourseOfFireDefinitionAsync( SetName setName ) {

            if (CourseOfFireCache.TryGetValue( setName, out CourseOfFire c )) {

                if (AutoDownloadNewDefinitionVersions)
                    //Purposefully not awaiting this call
                    DownloadNewMinorVersionIfAvaliableAsync( c );

                return c;
            }

            DateTime lastChecked;
            if (CourseOfFireNotFoundCache.TryGetValue( setName, out lastChecked ) && (DateTime.UtcNow - lastChecked).TotalSeconds < NOT_FOUND_RECHECK_TIME)
                throw new DefinitionNotFoundException( $"CourseOfFire definition '{setName}' not found. " );

            var response = await DefinitionFetcher.FETCHER.GetCourseOfFireDefinitionAsync( setName );
            if (response.HasOkStatusCode) {
                var definition = response.Definition;

                CourseOfFireCache.TryAdd( setName, definition );
                return definition;
            } else if (response.RestApiStatusCode == System.Net.HttpStatusCode.NotFound) {
                //Also cache any NotFound requests
                CourseOfFireNotFoundCache.TryAdd( setName, DateTime.UtcNow );
                throw new DefinitionNotFoundException( $"CourseOfFire definition '{setName}' not found. " );
            } else {
                throw new ScoposAPIException( $"Unable to retreive CourseOfFire definition {setName}. Overall: {response.OverallStatusCode}, REST API {response.RestApiStatusCode}" );
            }
        }

        /// <summary>
        /// Tries and returns the CourseOfFire requested, if it has already been loaded into the cache.
        /// Returns false, if it has not been loaded yet. Then tries and reads or downloads it in the background. Which means 
        /// the definition may be avalaible at a latter time (once the getting is successful).
        /// </summary>
        /// <param name="setName"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool TryGetCourseOfFireDefinition( SetName setName, out CourseOfFire c ) {

            //Try and read from the cache. If its previously been loaded, then return it. 
            if (CourseOfFireCache.TryGetValue( setName, out c )) {

                return true;
            }

            //If it is not loaded, make a call to read / download it.
            //Purposefully not awaiting this call. This way this method may remain synchronous, and the download can happen in the background.
            GetCourseOfFireDefinitionAsync( setName );

            return false;
        }

        /// <summary>
        /// Method checks to see if there is a new minor release avaliable for the past in Definition.
        /// If so, it tries and download it and update the cache.
        /// </summary>
        /// <param name="def"></param>
        /// <returns>Boolean indicating if there was a new minor release avaliable and if it was successful in downloading it.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static async Task<bool> DownloadNewMinorVersionIfAvaliableAsync( CourseOfFire def ) {
            if (def == null)
                throw new ArgumentNullException( nameof( def ) );

            try {
                if (await def.IsVersionUpdateAvaliableAsync()) {

                    SetName setName = def.GetSetName( true );

                    //Make a request, that ignores all of our local caching
                    var request = new GetDefinitionPublicRequest( setName, def.Type ) {
                        IgnoreInMemoryCache = true,
                        IgnoreFileSystemCache = true,
                        IgnoreRestAPICache = true
                    };
                    var response = new GetDefinitionPublicResponse<CourseOfFire>( request );

                    await DefinitionFetcher.FETCHER.GetDefinitionAsync<CourseOfFire>( request, response );
                    if (response.HasOkStatusCode) {
                        CourseOfFireCache[setName] = response.Definition;
                        return true;
                    }
                }
            } catch (Exception ex) {
                _logger.Error( ex, $"Caught error while trying to check and download if there was a new {def.Type} Definition for {def.SetName}" );
                //Swallowing the error, as its simple enough to say the operation was not successful.
            }

            return false;
        }

        #endregion

        #region EVENT AND STAGE STYLE MAPPING

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        /// <exception cref="DefinitionNotFoundException" />
        /// <exception cref="ScoposAPIException" />
        public static async Task<EventAndStageStyleMapping> GetEventAndStageStyleMappingDefinitionAsync( SetName setName ) {

            if (EventAndStageStyleMappingCache.TryGetValue( setName, out EventAndStageStyleMapping c )) {

                if (AutoDownloadNewDefinitionVersions)
                    //Purposefully not awaiting this call
                    DownloadNewMinorVersionIfAvaliableAsync( c );

                return c;
            }

            DateTime lastChecked;
            if (EventAndStageStyleMappingNotFoundCache.TryGetValue( setName, out lastChecked ) && (DateTime.UtcNow - lastChecked).TotalSeconds < NOT_FOUND_RECHECK_TIME)
                throw new DefinitionNotFoundException( $"EventAndStageStyleMapping definition '{setName}' not found. " );

            var response = await DefinitionFetcher.FETCHER.GetEventAndStageStyleMappingDefinitionAsync( setName );
            if (response.HasOkStatusCode) {
                var definition = response.Definition;

                EventAndStageStyleMappingCache.TryAdd( setName, definition );
                return definition;
            } else if (response.RestApiStatusCode == System.Net.HttpStatusCode.NotFound) {
                //Also cache any NotFound requests
                EventAndStageStyleMappingNotFoundCache.TryAdd( setName, DateTime.UtcNow );
                throw new DefinitionNotFoundException( $"EventAndStageStyleMapping definition '{setName}' not found. " );
            } else {
                throw new ScoposAPIException( $"Unable to retreive EventAndStageStyleMapping definition {setName}. Overall: {response.OverallStatusCode}, REST API {response.RestApiStatusCode}" );
            }
        }

        /// <summary>
        /// Tries and returns the EVENT AND STAGE STYLE MAPPING requested, if it has already been loaded into the cache.
        /// Returns false, if it has not been loaded yet. Then tries and reads or downloads it in the background. Which means 
        /// the definition may be avalaible at a latter time (once the getting is successful).
        /// </summary>
        /// <param name="setName"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool TryGetEventAndStageStyleMappingDefinition( SetName setName, out EventAndStageStyleMapping def ) {

            //Try and read from the cache. If its previously been loaded, then return it. 
            if (EventAndStageStyleMappingCache.TryGetValue( setName, out def )) {

                return true;
            }

            //If it is not loaded, make a call to read / download it.
            //Purposefully not awaiting this call. This way this method may remain synchronous, and the download can happen in the background.
            GetEventAndStageStyleMappingDefinitionAsync( setName );

            return false;
        }

        /// <summary>
        /// Method checks to see if there is a new minor release avaliable for the past in Definition.
        /// If so, it tries and download it and update the cache.
        /// </summary>
        /// <param name="def"></param>
        /// <returns>Boolean indicating if there was a new minor release avaliable and if it was successful in downloading it.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static async Task<bool> DownloadNewMinorVersionIfAvaliableAsync( EventAndStageStyleMapping def ) {
            if (def == null)
                throw new ArgumentNullException( nameof( def ) );

            try {
                if (await def.IsVersionUpdateAvaliableAsync()) {

                    SetName setName = def.GetSetName( true );

                    //Make a request, that ignores all of our local caching
                    var request = new GetDefinitionPublicRequest( setName, def.Type ) {
                        IgnoreInMemoryCache = true,
                        IgnoreFileSystemCache = true,
                        IgnoreRestAPICache = true
                    };
                    var response = new GetDefinitionPublicResponse<EventAndStageStyleMapping>( request );

                    await DefinitionFetcher.FETCHER.GetDefinitionAsync<EventAndStageStyleMapping>( request, response );
                    if (response.HasOkStatusCode) {
                        EventAndStageStyleMappingCache[setName] = response.Definition;
                        return true;
                    }
                }
            } catch (Exception ex) {
                _logger.Error( ex, $"Caught error while trying to check and download if there was a new {def.Type} Definition for {def.SetName}" );
                //Swallowing the error, as its simple enough to say the operation was not successful.
            }

            return false;
        }

        #endregion

        #region EVENT STYLE

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        /// <exception cref="DefinitionNotFoundException" />
        /// <exception cref="ScoposAPIException" />
        public static async Task<EventStyle> GetEventStyleDefinitionAsync( SetName setName ) {

            if (EventStyleCache.TryGetValue( setName, out EventStyle c )) {

                if (AutoDownloadNewDefinitionVersions)
                    //Purposefully not awaiting this call
                    DownloadNewMinorVersionIfAvaliableAsync( c );

                return c;
            }

            DateTime lastChecked;
            if (EventStyleNotFoundCache.TryGetValue( setName, out lastChecked ) && (DateTime.UtcNow - lastChecked).TotalSeconds < NOT_FOUND_RECHECK_TIME)
                throw new DefinitionNotFoundException( $"EventStyle definition '{setName}' not found. " );

            var response = await DefinitionFetcher.FETCHER.GetEventStyleDefinitionAsync( setName );
            if (response.HasOkStatusCode) {
                var definition = response.Definition;

                EventStyleCache.TryAdd( setName, definition );
                return definition;
            } else if (response.RestApiStatusCode == System.Net.HttpStatusCode.NotFound) {
                //Also cache any NotFound requests
                EventStyleNotFoundCache.TryAdd( setName, DateTime.UtcNow );
                throw new DefinitionNotFoundException( $"EventStyle definition '{setName}' not found. " );
            } else {
                throw new ScoposAPIException( $"Unable to retreive EventStyle definition {setName}. Overall: {response.OverallStatusCode}, REST API {response.RestApiStatusCode}" );
            }
        }

        /// <summary>
        /// Tries and returns the EVENT STYLE requested, if it has already been loaded into the cache.
        /// Returns false, if it has not been loaded yet. Then tries and reads or downloads it in the background. Which means 
        /// the definition may be avalaible at a latter time (once the getting is successful).
        /// </summary>
        /// <param name="setName"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool TryGetEventStyleDefinition( SetName setName, out EventStyle def ) {

            //Try and read from the cache. If its previously been loaded, then return it. 
            if (EventStyleCache.TryGetValue( setName, out def )) {

                return true;
            }

            //If it is not loaded, make a call to read / download it.
            //Purposefully not awaiting this call. This way this method may remain synchronous, and the download can happen in the background.
            GetEventStyleDefinitionAsync( setName );

            return false;
        }

        /// <summary>
        /// Method checks to see if there is a new minor release avaliable for the past in Definition.
        /// If so, it tries and download it and update the cache.
        /// </summary>
        /// <param name="def"></param>
        /// <returns>Boolean indicating if there was a new minor release avaliable and if it was successful in downloading it.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static async Task<bool> DownloadNewMinorVersionIfAvaliableAsync( EventStyle def ) {
            if (def == null)
                throw new ArgumentNullException( nameof( def ) );

            try {
                if (await def.IsVersionUpdateAvaliableAsync()) {

                    SetName setName = def.GetSetName( true );

                    //Make a request, that ignores all of our local caching
                    var request = new GetDefinitionPublicRequest( setName, def.Type ) {
                        IgnoreInMemoryCache = true,
                        IgnoreFileSystemCache = true,
                        IgnoreRestAPICache = true
                    };
                    var response = new GetDefinitionPublicResponse<EventStyle>( request );

                    await DefinitionFetcher.FETCHER.GetDefinitionAsync<EventStyle>( request, response );
                    if (response.HasOkStatusCode) {
                        EventStyleCache[setName] = response.Definition;
                        return true;
                    }
                }
            } catch (Exception ex) {
                _logger.Error( ex, $"Caught error while trying to check and download if there was a new {def.Type} Definition for {def.SetName}" );
                //Swallowing the error, as its simple enough to say the operation was not successful.
            }

            return false;
        }

        #endregion

        #region RANKING RULE

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        /// <exception cref="DefinitionNotFoundException" />
        /// <exception cref="ScoposAPIException" />
        public static async Task<RankingRule> GetRankingRuleDefinitionAsync( SetName setName ) {

            if (RankingRuleCache.TryGetValue( setName, out RankingRule c )) {

                if (AutoDownloadNewDefinitionVersions)
                    //Purposefully not awaiting this call
                    DownloadNewMinorVersionIfAvaliableAsync( c );

                return c;
            }

            DateTime lastChecked;
            if (RankingRuleNotFoundCache.TryGetValue( setName, out lastChecked ) && (DateTime.UtcNow - lastChecked).TotalSeconds < NOT_FOUND_RECHECK_TIME)
                throw new DefinitionNotFoundException( $"RankingRule definition '{setName}' not found. " );

            var response = await DefinitionFetcher.FETCHER.GetRankingRuleDefinitionAsync( setName );
            if (response.HasOkStatusCode) {
                var definition = response.Definition;

                RankingRuleCache.TryAdd( setName, definition );
                return definition;
            } else if (response.RestApiStatusCode == System.Net.HttpStatusCode.NotFound) {
                //Also cache any NotFound requests
                RankingRuleNotFoundCache.TryAdd( setName, DateTime.UtcNow );
                throw new DefinitionNotFoundException( $"RankingRule definition '{setName}' not found. " );
            } else {
                throw new ScoposAPIException( $"Unable to retreive RankingRule definition {setName}. Overall: {response.OverallStatusCode}, REST API {response.RestApiStatusCode}" );
            }
        }

        /// <summary>
        /// Tries and returns the RANKING RULE requested, if it has already been loaded into the cache.
        /// Returns false, if it has not been loaded yet. Then tries and reads or downloads it in the background. Which means 
        /// the definition may be avalaible at a latter time (once the getting is successful).
        /// </summary>
        /// <param name="setName"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool TryGetRankingRuleDefinition( SetName setName, out RankingRule def ) {

            //Try and read from the cache. If its previously been loaded, then return it. 
            if (RankingRuleCache.TryGetValue( setName, out def )) {

                return true;
            }

            //If it is not loaded, make a call to read / download it.
            //Purposefully not awaiting this call. This way this method may remain synchronous, and the download can happen in the background.
            GetRankingRuleDefinitionAsync( setName );

            return false;
        }

        /// <summary>
        /// Method checks to see if there is a new minor release avaliable for the past in Definition.
        /// If so, it tries and download it and update the cache.
        /// </summary>
        /// <param name="def"></param>
        /// <returns>Boolean indicating if there was a new minor release avaliable and if it was successful in downloading it.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static async Task<bool> DownloadNewMinorVersionIfAvaliableAsync( RankingRule def ) {
            if (def == null)
                throw new ArgumentNullException( nameof( def ) );

            try {
                if (await def.IsVersionUpdateAvaliableAsync()) {

                    SetName setName = def.GetSetName( true );

                    //Make a request, that ignores all of our local caching
                    var request = new GetDefinitionPublicRequest( setName, def.Type ) {
                        IgnoreInMemoryCache = true,
                        IgnoreFileSystemCache = true,
                        IgnoreRestAPICache = true
                    };
                    var response = new GetDefinitionPublicResponse<RankingRule>( request );

                    await DefinitionFetcher.FETCHER.GetDefinitionAsync<RankingRule>( request, response );
                    if (response.HasOkStatusCode) {
                        RankingRuleCache[setName] = response.Definition;
                        return true;
                    }
                }
            } catch (Exception ex) {
                _logger.Error( ex, $"Caught error while trying to check and download if there was a new {def.Type} Definition for {def.SetName}" );
                //Swallowing the error, as its simple enough to say the operation was not successful.
            }

            return false;
        }

        #endregion

        #region RESULT LIST FORMAT

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        /// <exception cref="DefinitionNotFoundException" />
        /// <exception cref="ScoposAPIException" />
        public static async Task<ResultListFormat> GetResultListFormatDefinitionAsync( SetName setName ) {

            if (ResultListFormatCache.TryGetValue( setName, out ResultListFormat c )) {

                if (AutoDownloadNewDefinitionVersions)
                    //Purposefully not awaiting this call
                    DownloadNewMinorVersionIfAvaliableAsync( c );

                return c;
            }

            DateTime lastChecked;
            if (ResultListFormatNotFoundCache.TryGetValue( setName, out lastChecked ) && (DateTime.UtcNow - lastChecked).TotalSeconds < NOT_FOUND_RECHECK_TIME)
                throw new DefinitionNotFoundException( $"ResultListFormat definition '{setName}' not found. " );

            var response = await DefinitionFetcher.FETCHER.GetResultListFormatDefinitionAsync( setName );
            if (response.HasOkStatusCode) {
                var definition = response.Definition;

                ResultListFormatCache.TryAdd( setName, definition );
                return definition;
            } else if (response.RestApiStatusCode == System.Net.HttpStatusCode.NotFound) {
                //Also cache any NotFound requests
                ResultListFormatNotFoundCache.TryAdd( setName, DateTime.UtcNow );
                throw new DefinitionNotFoundException( $"ResultListFormat definition '{setName}' not found. " );
            } else {
                throw new ScoposAPIException( $"Unable to retreive ResultListFormat definition {setName}. Overall: {response.OverallStatusCode}, REST API {response.RestApiStatusCode}" );
            }
        }

        /// <summary>
        /// Tries and returns the RESULT LIST FORMAT requested, if it has already been loaded into the cache.
        /// Returns false, if it has not been loaded yet. Then tries and reads or downloads it in the background. Which means 
        /// the definition may be avalaible at a latter time (once the getting is successful).
        /// </summary>
        /// <param name="setName"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool TryGetResultListFormatDefinition( SetName setName, out ResultListFormat def ) {

            //Try and read from the cache. If its previously been loaded, then return it. 
            if (ResultListFormatCache.TryGetValue( setName, out def )) {

                return true;
            }

            //If it is not loaded, make a call to read / download it.
            //Purposefully not awaiting this call. This way this method may remain synchronous, and the download can happen in the background.
            GetResultListFormatDefinitionAsync( setName );

            return false;
        }

        /// <summary>
        /// Method checks to see if there is a new minor release avaliable for the past in Definition.
        /// If so, it tries and download it and update the cache.
        /// </summary>
        /// <param name="def"></param>
        /// <returns>Boolean indicating if there was a new minor release avaliable and if it was successful in downloading it.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static async Task<bool> DownloadNewMinorVersionIfAvaliableAsync( ResultListFormat def ) {
            if (def == null)
                throw new ArgumentNullException( nameof( def ) );

            try {
                if (await def.IsVersionUpdateAvaliableAsync()) {

                    SetName setName = def.GetSetName( true );

                    //Make a request, that ignores all of our local caching
                    var request = new GetDefinitionPublicRequest( setName, def.Type ) {
                        IgnoreInMemoryCache = true,
                        IgnoreFileSystemCache = true,
                        IgnoreRestAPICache = true
                    };
                    var response = new GetDefinitionPublicResponse<ResultListFormat>( request );

                    await DefinitionFetcher.FETCHER.GetDefinitionAsync<ResultListFormat>( request, response );
                    if (response.HasOkStatusCode) {
                        ResultListFormatCache[setName] = response.Definition;
                        return true;
                    }
                }
            } catch (Exception ex) {
                _logger.Error( ex, $"Caught error while trying to check and download if there was a new {def.Type} Definition for {def.SetName}" );
                //Swallowing the error, as its simple enough to say the operation was not successful.
            }

            return false;
        }

        #endregion

        #region SCORE FORMAT COLLECTION

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        /// <exception cref="DefinitionNotFoundException" />
        /// <exception cref="ScoposAPIException" />
        public static async Task<ScoreFormatCollection> GetScoreFormatCollectionDefinitionAsync( SetName setName ) {

            if (ScoreFormatCollectionCache.TryGetValue( setName, out ScoreFormatCollection c )) {

                if (AutoDownloadNewDefinitionVersions)
                    //Purposefully not awaiting this call
                    DownloadNewMinorVersionIfAvaliableAsync( c );

                return c;
            }

            DateTime lastChecked;
            if (ScoreFormatCollectionNotFoundCache.TryGetValue( setName, out lastChecked ) && (DateTime.UtcNow - lastChecked).TotalSeconds < NOT_FOUND_RECHECK_TIME)
                throw new DefinitionNotFoundException( $"ScoreFormatCollection definition '{setName}' not found. " );

            var response = await DefinitionFetcher.FETCHER.GetScoreFormatCollectionDefinitionAsync( setName );
            if (response.HasOkStatusCode) {
                var definition = response.Definition;

                ScoreFormatCollectionCache.TryAdd( setName, definition );
                return definition;
            } else if (response.RestApiStatusCode == System.Net.HttpStatusCode.NotFound) {
                //Also cache any NotFound requests
                ScoreFormatCollectionNotFoundCache.TryAdd( setName, DateTime.UtcNow );
                throw new DefinitionNotFoundException( $"ScoreFormatCollection definition '{setName}' not found. " );
            } else {
                throw new ScoposAPIException( $"Unable to retreive ScoreFormatCollection definition {setName}. Overall: {response.OverallStatusCode}, REST API {response.RestApiStatusCode}" );
            }
        }

        /// <summary>
        /// Tries and returns the SCORE FORMAT COLLECTION requested, if it has already been loaded into the cache.
        /// Returns false, if it has not been loaded yet. Then tries and reads or downloads it in the background. Which means 
        /// the definition may be avalaible at a latter time (once the getting is successful).
        /// </summary>
        /// <param name="setName"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool TryGetScoreFormatCollectionDefinition( SetName setName, out ScoreFormatCollection def ) {

            //Try and read from the cache. If its previously been loaded, then return it. 
            if (ScoreFormatCollectionCache.TryGetValue( setName, out def )) {

                return true;
            }

            //If it is not loaded, make a call to read / download it.
            //Purposefully not awaiting this call. This way this method may remain synchronous, and the download can happen in the background.
            GetScoreFormatCollectionDefinitionAsync( setName );

            return false;
        }

        /// <summary>
        /// Method checks to see if there is a new minor release avaliable for the past in Definition.
        /// If so, it tries and download it and update the cache.
        /// </summary>
        /// <param name="def"></param>
        /// <returns>Boolean indicating if there was a new minor release avaliable and if it was successful in downloading it.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static async Task<bool> DownloadNewMinorVersionIfAvaliableAsync( ScoreFormatCollection def ) {
            if (def == null)
                throw new ArgumentNullException( nameof( def ) );

            try {
                if (await def.IsVersionUpdateAvaliableAsync()) {

                    SetName setName = def.GetSetName( true );

                    //Make a request, that ignores all of our local caching
                    var request = new GetDefinitionPublicRequest( setName, def.Type ) {
                        IgnoreInMemoryCache = true,
                        IgnoreFileSystemCache = true,
                        IgnoreRestAPICache = true
                    };
                    var response = new GetDefinitionPublicResponse<ScoreFormatCollection>( request );

                    await DefinitionFetcher.FETCHER.GetDefinitionAsync<ScoreFormatCollection>( request, response );
                    if (response.HasOkStatusCode) {
                        ScoreFormatCollectionCache[setName] = response.Definition;
                        return true;
                    }
                }
            } catch (Exception ex) {
                _logger.Error( ex, $"Caught error while trying to check and download if there was a new {def.Type} Definition for {def.SetName}" );
                //Swallowing the error, as its simple enough to say the operation was not successful.
            }

            return false;
        }

        #endregion

        #region STAGE STYLE

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        /// <exception cref="DefinitionNotFoundException" />
        /// <exception cref="ScoposAPIException" />
        public static async Task<StageStyle> GetStageStyleDefinitionAsync( SetName setName ) {

            if (StageStyleCache.TryGetValue( setName, out StageStyle c )) {

                if (AutoDownloadNewDefinitionVersions)
                    //Purposefully not awaiting this call
                    DownloadNewMinorVersionIfAvaliableAsync( c );

                return c;
            }

            DateTime lastChecked;
            if (StageStyleNotFoundCache.TryGetValue( setName, out lastChecked ) && (DateTime.UtcNow - lastChecked).TotalSeconds < NOT_FOUND_RECHECK_TIME)
                throw new DefinitionNotFoundException( $"StageStyle definition '{setName}' not found. " );

            var response = await DefinitionFetcher.FETCHER.GetStageStyleDefinitionAsync( setName );
            if (response.HasOkStatusCode) {
                var definition = response.Definition;

                StageStyleCache.TryAdd( setName, definition );
                return definition;
            } else if (response.RestApiStatusCode == System.Net.HttpStatusCode.NotFound) {
                //Also cache any NotFound requests
                StageStyleNotFoundCache.TryAdd( setName, DateTime.UtcNow );
                throw new DefinitionNotFoundException( $"StageStyle definition '{setName}' not found. " );
            } else {
                throw new ScoposAPIException( $"Unable to retreive StageStyle definition {setName}. Overall:  {response.OverallStatusCode} , REST API {response.RestApiStatusCode}" );
            }
        }

        /// <summary>
        /// Tries and returns the STAGE STYLE requested, if it has already been loaded into the cache.
        /// Returns false, if it has not been loaded yet. Then tries and reads or downloads it in the background. Which means 
        /// the definition may be avalaible at a latter time (once the getting is successful).
        /// </summary>
        /// <param name="setName"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool TryGetStageStyleDefinition( SetName setName, out StageStyle def ) {

            //Try and read from the cache. If its previously been loaded, then return it. 
            if (StageStyleCache.TryGetValue( setName, out def )) {

                return true;
            }

            //If it is not loaded, make a call to read / download it.
            //Purposefully not awaiting this call. This way this method may remain synchronous, and the download can happen in the background.
            GetStageStyleDefinitionAsync( setName );

            return false;
        }

        /// <summary>
        /// Method checks to see if there is a new minor release avaliable for the past in Definition.
        /// If so, it tries and download it and update the cache.
        /// </summary>
        /// <param name="def"></param>
        /// <returns>Boolean indicating if there was a new minor release avaliable and if it was successful in downloading it.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static async Task<bool> DownloadNewMinorVersionIfAvaliableAsync( StageStyle def ) {
            if (def == null)
                throw new ArgumentNullException( nameof( def ) );

            try {
                if (await def.IsVersionUpdateAvaliableAsync()) {

                    SetName setName = def.GetSetName( true );

                    //Make a request, that ignores all of our local caching
                    var request = new GetDefinitionPublicRequest( setName, def.Type ) {
                        IgnoreInMemoryCache = true,
                        IgnoreFileSystemCache = true,
                        IgnoreRestAPICache = true
                    };
                    var response = new GetDefinitionPublicResponse<StageStyle>( request );

                    await DefinitionFetcher.FETCHER.GetDefinitionAsync<StageStyle>( request, response );
                    if (response.HasOkStatusCode) {
                        StageStyleCache[setName] = response.Definition;
                        return true;
                    }
                }
            } catch (Exception ex) {
                _logger.Error( ex, $"Caught error while trying to check and download if there was a new {def.Type} Definition for {def.SetName}" );
                //Swallowing the error, as its simple enough to say the operation was not successful.
            }

            return false;
        }

        #endregion

        #region TARGET COLLECTION

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        /// <exception cref="DefinitionNotFoundException" />
        /// <exception cref="ScoposAPIException" />
        public static async Task<TargetCollection> GetTargetCollectionDefinitionAsync( SetName setName ) {

            if (TargetCollectionCache.TryGetValue( setName, out TargetCollection c )) {

                if (AutoDownloadNewDefinitionVersions)
                    //Purposefully not awaiting this call
                    DownloadNewMinorVersionIfAvaliableAsync( c );

                return c;
            }

            DateTime lastChecked;
            if (TargetCollectionNotFoundCache.TryGetValue( setName, out lastChecked ) && (DateTime.UtcNow - lastChecked).TotalSeconds < NOT_FOUND_RECHECK_TIME)
                throw new DefinitionNotFoundException( $"TargetCollection definition '{setName}' not found. " );

            var response = await DefinitionFetcher.FETCHER.GetTargetCollectionDefinitionAsync( setName );
            if (response.HasOkStatusCode) {
                var definition = response.Definition;

                TargetCollectionCache.TryAdd( setName, definition );
                return definition;
            } else if (response.RestApiStatusCode == System.Net.HttpStatusCode.NotFound) {
                //Also cache any NotFound requests
                TargetCollectionNotFoundCache.TryAdd( setName, DateTime.UtcNow );
                throw new DefinitionNotFoundException( $"TargetCollection definition '{setName}' not found. " );
            } else {
                throw new ScoposAPIException( $"Unable to retreive TargetCollection definition {setName}. Overall:  {response.OverallStatusCode} , REST API {response.RestApiStatusCode}" );
            }
        }

        /// <summary>
        /// Tries and returns the TARGET COLLECTION requested, if it has already been loaded into the cache.
        /// Returns false, if it has not been loaded yet. Then tries and reads or downloads it in the background. Which means 
        /// the definition may be avalaible at a latter time (once the getting is successful).
        /// </summary>
        /// <param name="setName"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool TryGetTargetCollectionDefinition( SetName setName, out TargetCollection def ) {

            //Try and read from the cache. If its previously been loaded, then return it. 
            if (TargetCollectionCache.TryGetValue( setName, out def )) {

                return true;
            }

            //If it is not loaded, make a call to read / download it.
            //Purposefully not awaiting this call. This way this method may remain synchronous, and the download can happen in the background.
            GetTargetCollectionDefinitionAsync( setName );

            return false;
        }

        /// <summary>
        /// Method checks to see if there is a new minor release avaliable for the past in Definition.
        /// If so, it tries and download it and update the cache.
        /// </summary>
        /// <param name="def"></param>
        /// <returns>Boolean indicating if there was a new minor release avaliable and if it was successful in downloading it.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static async Task<bool> DownloadNewMinorVersionIfAvaliableAsync( TargetCollection def ) {
            if (def == null)
                throw new ArgumentNullException( nameof( def ) );

            try {
                if (await def.IsVersionUpdateAvaliableAsync()) {

                    SetName setName = def.GetSetName( true );

                    //Make a request, that ignores all of our local caching
                    var request = new GetDefinitionPublicRequest( setName, def.Type ) {
                        IgnoreInMemoryCache = true,
                        IgnoreFileSystemCache = true,
                        IgnoreRestAPICache = true
                    };
                    var response = new GetDefinitionPublicResponse<TargetCollection>( request );

                    await DefinitionFetcher.FETCHER.GetDefinitionAsync<TargetCollection>( request, response );
                    if (response.HasOkStatusCode) {
                        TargetCollectionCache[setName] = response.Definition;
                        return true;
                    }
                }
            } catch (Exception ex) {
                _logger.Error( ex, $"Caught error while trying to check and download if there was a new {def.Type} Definition for {def.SetName}" );
                //Swallowing the error, as its simple enough to say the operation was not successful.
            }

            return false;
        }

        #endregion

        #region TARGET

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        /// <exception cref="DefinitionNotFoundException" />
        /// <exception cref="ScoposAPIException" />
        public static async Task<Target> GetTargetDefinitionAsync( SetName setName ) {

            if (TargetCache.TryGetValue( setName, out Target t )) {

                if (AutoDownloadNewDefinitionVersions)
                    //Purposefully not awaiting this call
                    DownloadNewMinorVersionIfAvaliableAsync( t );

                return t;
            }

            DateTime lastChecked;
            if (TargetNotFoundCache.TryGetValue( setName, out lastChecked ) && (DateTime.UtcNow - lastChecked).TotalSeconds < NOT_FOUND_RECHECK_TIME)
                throw new DefinitionNotFoundException( $"Target definition '{setName}' not found. " );

            var response = await DefinitionFetcher.FETCHER.GetTargetDefinitionAsync( setName );
            if (response.HasOkStatusCode) {
                var definition = response.Definition;

                TargetCache.TryAdd( setName, definition );
                return definition;
            } else if (response.RestApiStatusCode == System.Net.HttpStatusCode.NotFound) {
                //Also cache any NotFound requests
                TargetNotFoundCache.TryAdd( setName, DateTime.UtcNow );
                throw new DefinitionNotFoundException( $"Target definition '{setName}' not found. " );
            } else {
                throw new ScoposAPIException( $"Unable to retreive Target definition {setName}. Overall:  {response.OverallStatusCode} , REST API {response.RestApiStatusCode}" );
            }
        }

        /// <summary>
        /// Tries and returns the TARGET requested, if it has already been loaded into the cache.
        /// Returns false, if it has not been loaded yet. Then tries and reads or downloads it in the background. Which means 
        /// the definition may be avalaible at a latter time (once the getting is successful).
        /// </summary>
        /// <param name="setName"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool TryGetTargetDefinition( SetName setName, out Target def ) {

            //Try and read from the cache. If its previously been loaded, then return it. 
            if (TargetCache.TryGetValue( setName, out def )) {

                return true;
            }

            //If it is not loaded, make a call to read / download it.
            //Purposefully not awaiting this call. This way this method may remain synchronous, and the download can happen in the background.
            GetTargetDefinitionAsync( setName );

            return false;
        }

        /// <summary>
        /// Method checks to see if there is a new minor release avaliable for the past in Definition.
        /// If so, it tries and download it and update the cache.
        /// </summary>
        /// <param name="def"></param>
        /// <returns>Boolean indicating if there was a new minor release avaliable and if it was successful in downloading it.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static async Task<bool> DownloadNewMinorVersionIfAvaliableAsync( Target def ) {
            if (def == null)
                throw new ArgumentNullException( nameof( def ) );

            try {
                if (await def.IsVersionUpdateAvaliableAsync()) {

                    SetName setName = def.GetSetName( true );

                    //Make a request, that ignores all of our local caching
                    var request = new GetDefinitionPublicRequest( setName, def.Type ) {
                        IgnoreInMemoryCache = true,
                        IgnoreFileSystemCache = true,
                        IgnoreRestAPICache = true
                    };
                    var response = new GetDefinitionPublicResponse<Target>( request );

                    await DefinitionFetcher.FETCHER.GetDefinitionAsync<Target>( request, response );
                    if (response.HasOkStatusCode) {
                        TargetCache[setName] = response.Definition;
                        return true;
                    }
                }
            } catch (Exception ex) {
                _logger.Error( ex, $"Caught error while trying to check and download if there was a new {def.Type} Definition for {def.SetName}" );
                //Swallowing the error, as its simple enough to say the operation was not successful.
            }

            return false;
        }

        #endregion

        public static async Task<bool> DownloadNewMinorVersionIfAvaliableAsync( Definition def ) {

            switch (def.Type) {
                case DefinitionType.ATTRIBUTE:
                    return await DownloadNewMinorVersionIfAvaliableAsync( (Scopos.BabelFish.DataModel.Definitions.Attribute)def );

                case DefinitionType.COURSEOFFIRE:
                    return await DownloadNewMinorVersionIfAvaliableAsync( (CourseOfFire)def );

                case DefinitionType.EVENTSTYLE:
                    return await DownloadNewMinorVersionIfAvaliableAsync( (EventStyle)def );

                case DefinitionType.EVENTANDSTAGESTYLEMAPPING:
                    return await DownloadNewMinorVersionIfAvaliableAsync( (EventAndStageStyleMapping)def );

                case DefinitionType.RANKINGRULES:
                    return await DownloadNewMinorVersionIfAvaliableAsync( (RankingRule)def );

                case DefinitionType.RESULTLISTFORMAT:
                    return await DownloadNewMinorVersionIfAvaliableAsync( (ResultListFormat)def );

                case DefinitionType.SCOREFORMATCOLLECTION:
                    return await DownloadNewMinorVersionIfAvaliableAsync( (ScoreFormatCollection)def );

                case DefinitionType.STAGESTYLE:
                    return await DownloadNewMinorVersionIfAvaliableAsync( (StageStyle)def );

                case DefinitionType.TARGET:
                    return await DownloadNewMinorVersionIfAvaliableAsync( (Target)def );

                case DefinitionType.TARGETCOLLECTION:
                    return await DownloadNewMinorVersionIfAvaliableAsync( (TargetCollection)def );

                default:
                    //Shouldn't ever get here
                    return false;
            }
        }

        public static async Task<Definition?> GetDefinitionAsync( DefinitionType type, SetName setName ) {

            switch (type) {
                case DefinitionType.ATTRIBUTE:
                    return await GetAttributeDefinitionAsync( setName );

                case DefinitionType.COURSEOFFIRE:
                    return await GetCourseOfFireDefinitionAsync( setName );

                case DefinitionType.EVENTSTYLE:
                    return await GetEventStyleDefinitionAsync( setName );

                case DefinitionType.EVENTANDSTAGESTYLEMAPPING:
                    return await GetEventAndStageStyleMappingDefinitionAsync( setName );

                case DefinitionType.RANKINGRULES:
                    return await GetRankingRuleDefinitionAsync( setName );

                case DefinitionType.RESULTLISTFORMAT:
                    return await GetResultListFormatDefinitionAsync( setName );

                case DefinitionType.SCOREFORMATCOLLECTION:
                    return await GetScoreFormatCollectionDefinitionAsync( setName );

                case DefinitionType.STAGESTYLE:
                    return await GetStageStyleDefinitionAsync( setName );

                case DefinitionType.TARGET:
                    return await GetTargetDefinitionAsync( setName );

                case DefinitionType.TARGETCOLLECTION:
                    return await GetTargetCollectionDefinitionAsync( setName );

                default:
                    //Shouldn't ever get here
                    return null;
            }
        }

        /// <summary>
        /// Clears all cached responses from the DefinitionCache.
        /// </summary>
        public static void ClearCache() {
            AttributeCache.Clear();
            CourseOfFireCache.Clear();
            EventAndStageStyleMappingCache.Clear();
            EventStyleCache.Clear();
            RankingRuleCache.Clear();
            ResultListFormatCache.Clear();
            ScoreFormatCollectionCache.Clear();
            StageStyleCache.Clear();
            TargetCache.Clear();
            TargetCollectionCache.Clear();

            AttributeNotFoundCache.Clear();
            EventAndStageStyleMappingNotFoundCache.Clear();
            CourseOfFireNotFoundCache.Clear();
            EventStyleNotFoundCache.Clear();
            RankingRuleNotFoundCache.Clear();
            ResultListFormatNotFoundCache.Clear();
            ScoreFormatCollectionNotFoundCache.Clear();
            StageStyleNotFoundCache.Clear();
            TargetNotFoundCache.Clear();
            TargetCollectionNotFoundCache.Clear();
        }

        /// <summary>
        /// Effectively for unit testing only. Returns the number of definitions in the cache
        /// </summary>
        /// <param name="definitionType"></param>
        /// <returns></returns>
        public static int GetCacheSize( DefinitionType definitionType ) {

            switch (definitionType) {
                case DefinitionType.ATTRIBUTE:
                    return AttributeCache.Count;

                case DefinitionType.COURSEOFFIRE:
                    return CourseOfFireCache.Count;

                case DefinitionType.EVENTSTYLE:
                    return EventStyleCache.Count;

                case DefinitionType.EVENTANDSTAGESTYLEMAPPING:
                    return EventAndStageStyleMappingCache.Count;

                case DefinitionType.RANKINGRULES:
                    return RankingRuleCache.Count;

                case DefinitionType.RESULTLISTFORMAT:
                    return ResultListFormatCache.Count;

                case DefinitionType.SCOREFORMATCOLLECTION:
                    return ScoreFormatCollectionCache.Count;

                case DefinitionType.STAGESTYLE:
                    return StageStyleCache.Count;

                case DefinitionType.TARGET:
                    return TargetCache.Count;

                case DefinitionType.TARGETCOLLECTION:
                    return TargetCollectionCache.Count;

                default:
                    //Shouldn't ever get here
                    return 0;
            }
        }

    }
}
