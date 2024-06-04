using Scopos.BabelFish.DataModel.Athena;
using Scopos.BabelFish.DataModel.Definitions;
using System;
using System.Collections.Concurrent;
using System.Text;

namespace Scopos.BabelFish.APIClients {
    public static class DefinitionCache {

        private static ConcurrentDictionary<SetName, CourseOfFire> CourseOfFireCache = new ConcurrentDictionary<SetName, CourseOfFire>();

        private static ConcurrentDictionary<SetName, EventAndStageStyleMapping> EventAndStageStyleMappingCache = new ConcurrentDictionary<SetName, EventAndStageStyleMapping>();

        private static ConcurrentDictionary<SetName, RankingRule> RankingRuleCache = new ConcurrentDictionary<SetName, RankingRule>();

        private static ConcurrentDictionary<SetName, ResultListFormat> ResultListFormatCache = new ConcurrentDictionary<SetName, ResultListFormat>();

        private static ConcurrentDictionary<SetName, ScoreFormatCollection> ScoreFormatCollectionCache = new ConcurrentDictionary<SetName, ScoreFormatCollection>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        /// <exception cref="XApiKeyNotSetException" />
        /// <exception cref="ScoposAPIException" />
        public static async Task<CourseOfFire> GetCourseOfFireDefinitionAsync( SetName setName ) {

            if (CourseOfFireCache.TryGetValue( setName, out CourseOfFire c )) { return c; }

            var response = await DefinitionFetcher.FETCHER.GetCourseOfFireDefinitionAsync( setName );
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                var definition = response.Definition;

                CourseOfFireCache.TryAdd( setName, definition );
                return definition;
            } else {
                throw new ScoposAPIException( $"Unable to retreive CourseOfFire definition {setName}. {response.StatusCode} -> {response.MessageResponse.ToString()}" );
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        /// <exception cref="XApiKeyNotSetException" />
        /// <exception cref="ScoposAPIException" />
        public static async Task<EventAndStageStyleMapping> GetEventAndStageStyleMappingDefinitionAsync( SetName setName ) {
            if (EventAndStageStyleMappingCache.TryGetValue( setName, out EventAndStageStyleMapping c )) { return c; }

            var response = await DefinitionFetcher.FETCHER.GetEventAndStageStyleMappingDefinitionAsync( setName );
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                var definition = response.Definition;

                EventAndStageStyleMappingCache.TryAdd( setName, definition );
                return definition;
            } else {
                throw new ScoposAPIException( $"Unable to retreive EventAndStageStyleMapping definition {setName}. {response.StatusCode} -> {response.MessageResponse.ToString()}" );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        /// <exception cref="XApiKeyNotSetException" />
        /// <exception cref="ScoposAPIException" />
        public static async Task<RankingRule> GetRankingRuleDefinitionAsync( SetName setName ) {
            if (RankingRuleCache.TryGetValue( setName, out RankingRule c )) { return c; }

            var response = await DefinitionFetcher.FETCHER.GetRankingRuleDefinitionAsync( setName );
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {

                var definition = response.Definition;

                RankingRuleCache.TryAdd( setName, definition );
                return definition;
            } else {
                throw new ScoposAPIException( $"Unable to retreive RankingRule definition {setName}. {response.StatusCode} -> {response.MessageResponse.ToString()}" );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        /// <exception cref="XApiKeyNotSetException" />
        /// <exception cref="ScoposAPIException" />
        public static async Task<ResultListFormat> GetResultListFormatDefinitionAsync( SetName setName ) {
            if (ResultListFormatCache.TryGetValue( setName, out ResultListFormat c )) { return c; }

            var response = await DefinitionFetcher.FETCHER.GetResultListFormatDefinitionAsync( setName );
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {

                var definition = response.Definition;

                ResultListFormatCache.TryAdd( setName, definition );
                return definition;
            } else {
                throw new ScoposAPIException( $"Unable to retreive ResultListFormat definition {setName}. {response.StatusCode} -> {response.MessageResponse.ToString()}" );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        /// <exception cref="XApiKeyNotSetException" />
        /// <exception cref="ScoposAPIException" />
        public static async Task<ScoreFormatCollection> GetScoreFormatCollectionDefinitionAsync( SetName setName ) {
            if (ScoreFormatCollectionCache.TryGetValue( setName, out ScoreFormatCollection c )) { return c; }

            var response = await DefinitionFetcher.FETCHER.GetScoreFormatCollectionDefinitionAsync( setName );
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {

                var definition = response.Definition;

                ScoreFormatCollectionCache.TryAdd( setName, definition );
                return definition;
            } else {
                throw new ScoposAPIException( $"Unable to retreive EventAndStageStyleMapping definition {setName}. {response.StatusCode} -> {response.MessageResponse.ToString()}" );
            }
        }

        public static void ClearCaches() {
            CourseOfFireCache.Clear();
            EventAndStageStyleMappingCache.Clear();
            RankingRuleCache.Clear();
            ResultListFormatCache.Clear();
            ScoreFormatCollectionCache.Clear();
        }
    }
}
