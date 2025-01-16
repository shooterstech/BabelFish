using Scopos.BabelFish.DataModel.Athena;
using Scopos.BabelFish.DataModel.Definitions;
using System;
using System.Collections.Concurrent;
using System.Text;
using Scopos.BabelFish.Runtime;

namespace Scopos.BabelFish.APIClients {
    public static class DefinitionCache {

        private static ConcurrentDictionary<SetName, Scopos.BabelFish.DataModel.Definitions.Attribute> AttributeCache = new ConcurrentDictionary<SetName, Scopos.BabelFish.DataModel.Definitions.Attribute>();

        private static ConcurrentDictionary<SetName, CourseOfFire> CourseOfFireCache = new ConcurrentDictionary<SetName, CourseOfFire>();

        private static ConcurrentDictionary<SetName, EventAndStageStyleMapping> EventAndStageStyleMappingCache = new ConcurrentDictionary<SetName, EventAndStageStyleMapping>();

        private static ConcurrentDictionary<SetName, EventStyle> EventStyleCache = new ConcurrentDictionary<SetName, EventStyle>();

        private static ConcurrentDictionary<SetName, RankingRule> RankingRuleCache = new ConcurrentDictionary<SetName, RankingRule>();

        private static ConcurrentDictionary<SetName, ResultListFormat> ResultListFormatCache = new ConcurrentDictionary<SetName, ResultListFormat>();

        private static ConcurrentDictionary<SetName, ScoreFormatCollection> ScoreFormatCollectionCache = new ConcurrentDictionary<SetName, ScoreFormatCollection>();

        private static ConcurrentDictionary<SetName, StageStyle> StageStyleCache = new ConcurrentDictionary<SetName, StageStyle>();

        private static ConcurrentDictionary<SetName, Target> TargetCache = new ConcurrentDictionary<SetName, Target>();

        private static ConcurrentDictionary<SetName, TargetCollection> TargetCollectionCache = new ConcurrentDictionary<SetName, TargetCollection>();

        /// <summary>
        /// Preloads the Definiiton Cache with commmon definitions. If used, should help with some start up time.
        /// </summary>
        /// <returns></returns>
        public static async Task PreLoad() {
            await GetAttributeDefinitionAsync( SetName.Parse( "v1.0:ntparc:Three-Position Air Rifle Type" ) );
            await GetAttributeDefinitionAsync( SetName.Parse( "v1.0:orion:Profile Name" ) );
            await GetAttributeDefinitionAsync( SetName.Parse( "v1.0:orion:Date of Birth" ) );
            await GetAttributeDefinitionAsync( SetName.Parse( "v2.0:orion:Email Address" ) );
            await GetAttributeDefinitionAsync( SetName.Parse( "v2.0:orion:Phone Number" ) );
            await GetAttributeDefinitionAsync( SetName.Parse( "v1.0:orion:Address" ) );
            await GetAttributeDefinitionAsync( SetName.Parse( "v1.0:orion:High School Graduating Class" ) );
            await GetAttributeDefinitionAsync( SetName.Parse( "v1.0:orion:Pronouns" ) );
            await GetAttributeDefinitionAsync( SetName.Parse( "v1.0:orion:Gender" ) );
            await GetAttributeDefinitionAsync( SetName.Parse( "v1.0:orion:Collegiate Class" ) );
            await GetAttributeDefinitionAsync( SetName.Parse( "v1.0:orion:NCAA ID" ) );
            await GetAttributeDefinitionAsync( SetName.Parse( "v1.0:orion:Air Rifle Training Category" ) );
            await GetAttributeDefinitionAsync( SetName.Parse( "v1.0:orion:Air Pistol Training Category" ) );
            await GetAttributeDefinitionAsync( SetName.Parse( "v1.0:orion:Hometown" ) );
            await GetAttributeDefinitionAsync( SetName.Parse( "v1.0:orion:Account URL" ) );
            await GetAttributeDefinitionAsync( SetName.Parse( "v1.0:orion:Score History Public" ) );
            await GetAttributeDefinitionAsync( SetName.Parse( "v1.0:orion:Score History Personal" ) );
            await GetAttributeDefinitionAsync( SetName.Parse( "v1.0:orion:Score History Coach" ) );
            await GetAttributeDefinitionAsync( SetName.Parse( "v1.0:usas:Paralympic") );

            await GetScoreFormatCollectionDefinitionAsync( SetName.Parse( "v1.0:orion:Standard Score Formats" ) );
            await GetScoreFormatCollectionDefinitionAsync( SetName.Parse( "v1.0:orion:Standard Averaged Score Formats" ) );

            await GetTargetDefinitionAsync( SetName.Parse( "v1.0:issf:10m Air Rifle" ) );
            await GetTargetDefinitionAsync( SetName.Parse( "v1.0:issf:10m Air Pistol" ) );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        /// /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        /// <exception cref="ScoposAPIException" />
        public static async Task<Scopos.BabelFish.DataModel.Definitions.Attribute> GetAttributeDefinitionAsync( SetName setName ) {

            if (AttributeCache.TryGetValue( setName, out Scopos.BabelFish.DataModel.Definitions.Attribute a )) { return a; }

            var response = await DefinitionFetcher.FETCHER.GetAttributeDefinitionAsync( setName );
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                var definition = response.Definition;

                AttributeCache.TryAdd( setName, definition );
                return definition;
            } else {
                throw new ScoposAPIException( $"Unable to retreive CourseOfFire definition {setName}. {response.StatusCode}" );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        /// <exception cref="ScoposAPIException" />
        public static async Task<CourseOfFire> GetCourseOfFireDefinitionAsync( SetName setName ) {

            if (CourseOfFireCache.TryGetValue( setName, out CourseOfFire c )) { return c; }

            var response = await DefinitionFetcher.FETCHER.GetCourseOfFireDefinitionAsync( setName );
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                var definition = response.Definition;

                CourseOfFireCache.TryAdd( setName, definition );
                return definition;
            } else {
                throw new ScoposAPIException( $"Unable to retreive CourseOfFire definition {setName}. {response.StatusCode}" );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        /// <exception cref="ScoposAPIException" />
        public static async Task<EventAndStageStyleMapping> GetEventAndStageStyleMappingDefinitionAsync( SetName setName ) {
            if (EventAndStageStyleMappingCache.TryGetValue( setName, out EventAndStageStyleMapping c )) { return c; }

            var response = await DefinitionFetcher.FETCHER.GetEventAndStageStyleMappingDefinitionAsync( setName );
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                var definition = response.Definition;

                EventAndStageStyleMappingCache.TryAdd( setName, definition );
                return definition;
            } else {
                throw new ScoposAPIException( $"Unable to retreive EventAndStageStyleMapping definition {setName}. {response.StatusCode}" );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        /// <exception cref="ScoposAPIException" />
        public static async Task<EventStyle> GetEventStyleDefinitionAsync( SetName setName ) {
            if (EventStyleCache.TryGetValue( setName, out EventStyle c )) { return c; }

            var response = await DefinitionFetcher.FETCHER.GetEventStyleDefinitionAsync( setName );
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                var definition = response.Definition;

                EventStyleCache.TryAdd( setName, definition );
                return definition;
            } else {
                throw new ScoposAPIException( $"Unable to retreive EventStyle definition {setName}. {response.StatusCode}" );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        /// <exception cref="ScoposAPIException" />
        public static async Task<RankingRule> GetRankingRuleDefinitionAsync( SetName setName ) {
            if (RankingRuleCache.TryGetValue( setName, out RankingRule c )) { return c; }

            var response = await DefinitionFetcher.FETCHER.GetRankingRuleDefinitionAsync( setName );
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {

                var definition = response.Definition;

                RankingRuleCache.TryAdd( setName, definition );
                return definition;
            } else {
                throw new ScoposAPIException( $"Unable to retreive RankingRule definition {setName}. {response.StatusCode}" );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        /// <exception cref="ScoposAPIException" />
        public static async Task<ResultListFormat> GetResultListFormatDefinitionAsync( SetName setName ) {
            if (ResultListFormatCache.TryGetValue( setName, out ResultListFormat c )) { return c; }

            var response = await DefinitionFetcher.FETCHER.GetResultListFormatDefinitionAsync( setName );
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {

                var definition = response.Definition;

                ResultListFormatCache.TryAdd( setName, definition );
                return definition;
            } else {
                throw new ScoposAPIException( $"Unable to retreive ResultListFormat definition {setName}. {response.StatusCode}" );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        /// <exception cref="ScoposAPIException" />
        public static async Task<ScoreFormatCollection> GetScoreFormatCollectionDefinitionAsync( SetName setName ) {
            if (ScoreFormatCollectionCache.TryGetValue( setName, out ScoreFormatCollection c )) { return c; }

            var response = await DefinitionFetcher.FETCHER.GetScoreFormatCollectionDefinitionAsync( setName );
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {

                var definition = response.Definition;

                ScoreFormatCollectionCache.TryAdd( setName, definition );
                return definition;
            } else {
                throw new ScoposAPIException( $"Unable to retreive ScoreFormatCollection definition {setName}. {response.StatusCode}" );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        /// <exception cref="ScoposAPIException" />
        public static async Task<StageStyle> GetStageStyleDefinitionAsync( SetName setName ) {
            if (StageStyleCache.TryGetValue( setName, out StageStyle c )) { return c; }

            var response = await DefinitionFetcher.FETCHER.GetStageStyleDefinitionAsync( setName );
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                var definition = response.Definition;

                StageStyleCache.TryAdd( setName, definition );
                return definition;
            } else {
                throw new ScoposAPIException( $"Unable to retreive StageStyle definition {setName}. {response.StatusCode}" );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        /// <exception cref="ScoposAPIException" />
        public static async Task<TargetCollection> GetTargetCollectionDefinitionAsync( SetName setName ) {
            if (TargetCollectionCache.TryGetValue( setName, out TargetCollection c )) { return c; }

            var response = await DefinitionFetcher.FETCHER.GetTargetCollectionDefinitionAsync( setName );
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {

                var definition = response.Definition;

                TargetCollectionCache.TryAdd( setName, definition );
                return definition;
            } else {
                throw new ScoposAPIException( $"Unable to retreive TargetCollection definition {setName}. {response.StatusCode}" );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        /// <exception cref="ScoposAPIException" />
        public static async Task<Target> GetTargetDefinitionAsync( SetName setName ) {
            if (TargetCache.TryGetValue( setName, out Target t )) { return t; }

            var response = await DefinitionFetcher.FETCHER.GetTargetDefinitionAsync( setName );
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {

                var definition = response.Definition;

                TargetCache.TryAdd( setName, definition );
                return definition;
            } else {
                throw new ScoposAPIException( $"Unable to retreive Target definition {setName}. {response.StatusCode}" );
            }
        }

        public static void ClearCaches() {
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
        }
    }
}
