﻿using Scopos.BabelFish.DataModel.Athena;
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

        private static ConcurrentDictionary<SetName, TargetCollectionDefinition> TargetCollectionCache = new ConcurrentDictionary<SetName, TargetCollectionDefinition>();

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
                throw new ScoposAPIException( $"Unable to retreive CourseOfFire definition {setName}. {response.StatusCode} -> {response.MessageResponse.ToString()}" );
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
                throw new ScoposAPIException( $"Unable to retreive CourseOfFire definition {setName}. {response.StatusCode} -> {response.MessageResponse.ToString()}" );
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
                throw new ScoposAPIException( $"Unable to retreive EventAndStageStyleMapping definition {setName}. {response.StatusCode} -> {response.MessageResponse.ToString()}" );
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
                throw new ScoposAPIException( $"Unable to retreive EventStyle definition {setName}. {response.StatusCode} -> {response.MessageResponse.ToString()}" );
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
                throw new ScoposAPIException( $"Unable to retreive RankingRule definition {setName}. {response.StatusCode} -> {response.MessageResponse.ToString()}" );
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
                throw new ScoposAPIException( $"Unable to retreive ResultListFormat definition {setName}. {response.StatusCode} -> {response.MessageResponse.ToString()}" );
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
                throw new ScoposAPIException( $"Unable to retreive ScoreFormatCollection definition {setName}. {response.StatusCode} -> {response.MessageResponse.ToString()}" );
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
                throw new ScoposAPIException( $"Unable to retreive StageStyle definition {setName}. {response.StatusCode} -> {response.MessageResponse.ToString()}" );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        /// <exception cref="ScoposAPIException" />
        public static async Task<TargetCollectionDefinition> GetTargetCollectionDefinitionAsync( SetName setName ) {
            if (TargetCollectionCache.TryGetValue( setName, out TargetCollectionDefinition c )) { return c; }

            var response = await DefinitionFetcher.FETCHER.GetTargetCollectionDefinitionAsync( setName );
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {

                var definition = response.Definition;

                TargetCollectionCache.TryAdd( setName, definition );
                return definition;
            } else {
                throw new ScoposAPIException( $"Unable to retreive TargetCollection definition {setName}. {response.StatusCode} -> {response.MessageResponse.ToString()}" );
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
                throw new ScoposAPIException( $"Unable to retreive Target definition {setName}. {response.StatusCode} -> {response.MessageResponse.ToString()}" );
            }
        }

        public static void ClearCaches() {
            AttributeCache.Clear();
            CourseOfFireCache.Clear();
            EventAndStageStyleMappingCache.Clear();
            RankingRuleCache.Clear();
            ResultListFormatCache.Clear();
            ScoreFormatCollectionCache.Clear();
            TargetCache.Clear();
            TargetCollectionCache.Clear();
        }
    }
}
