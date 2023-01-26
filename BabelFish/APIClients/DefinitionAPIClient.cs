﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Requests.DefinitionAPI;
using Scopos.BabelFish.Responses.DefinitionAPI;

namespace Scopos.BabelFish.APIClients {
    public class DefinitionAPIClient : APIClient {

        DefinitionCache definitionCacheHelper;
        private readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="apiKey"></param>
        public DefinitionAPIClient( string apiKey ) : base( apiKey ) {
        }

        public DefinitionAPIClient( string apiKey, APIStage apiStage ) : base( apiKey, apiStage ) { }

        public async Task<GetDefinitionResponse<T>> GetDefinition<T>( GetDefinitionRequest request, GetDefinitionResponse<T> response ) where T : new() {
            try {
                // TimeToRun if we use cache
                DateTime startTime = DateTime.Now;

                T definition;
                if (DefinitionCache.CACHE.TryGetDefinition<T>( request.DefinitionType, request.SetName, out definition )) {
                    throw new NotImplementedException( "Definition Cache is not implemented." );
                } else {
                    GetDefinitionRequest requestParameters = new GetDefinitionRequest( request.SetName, request.DefinitionType );

                    await this.CallAPI( request, response ).ConfigureAwait( false );

                    // Save returned definition to cache
                    if (response.Body != null)
                        DefinitionCache.CACHE.SaveDefinition<T>( response.Definition );
                }
            } catch (Exception ex) {
                logger.Error( $"GetDefinition Error: {ex.Message}" );
                response.MessageResponse.Message.Add( $"GetDefinition Error: {ex.Message}" );
                response.MessageResponse.ResponseCodes.Add( "GetDefinition Error" );
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }

            return response;
        }


        public async Task<GetDefinitionResponse<Scopos.BabelFish.DataModel.Definitions.Attribute>> GetAttributeDefinitionAsync( SetName setName ) {

            var definitionType = DefinitionType.ATTRIBUTE;

            GetDefinitionRequest request = new GetDefinitionRequest( setName, definitionType );

            GetDefinitionResponse<Scopos.BabelFish.DataModel.Definitions.Attribute> response = new GetDefinitionResponse<Scopos.BabelFish.DataModel.Definitions.Attribute>( request );

            return await GetDefinition( request, response ).ConfigureAwait( false );
        }


        public async Task<GetDefinitionResponse<CourseOfFire>> GetCourseOfFireDefinition( SetName setName ) {

            var definitionType = DefinitionType.COURSEOFFIRE;

            GetDefinitionRequest request = new GetDefinitionRequest( setName, definitionType );

            GetDefinitionResponse<CourseOfFire> response = new GetDefinitionResponse<CourseOfFire>( request );

            return await GetDefinition( request, response ).ConfigureAwait( false );
        }

        public async Task<GetDefinitionResponse<EventStyle>> GetEventStyleDefinition( SetName setName ) {

            var definitionType = DefinitionType.EVENTSTYLE;

            GetDefinitionRequest request = new GetDefinitionRequest( setName, definitionType );

            GetDefinitionResponse<EventStyle> response = new GetDefinitionResponse<EventStyle>( request );

            return await GetDefinition( request, response ).ConfigureAwait( false );
        }

        public async Task<GetDefinitionResponse<RankingRule>> GetRankingRuleDefinition( SetName setName ) {

            var definitionType = DefinitionType.RANKINGRULES;

            GetDefinitionRequest request = new GetDefinitionRequest( setName, definitionType );

            GetDefinitionResponse<RankingRule> response = new GetDefinitionResponse<RankingRule>( request );

            return await GetDefinition( request, response ).ConfigureAwait( false );
        }

        public async Task<GetDefinitionResponse<StageStyle>> GetStageStyleDefinition( SetName setName ) {

            var definitionType = DefinitionType.STAGESTYLE;

            GetDefinitionRequest request = new GetDefinitionRequest( setName, definitionType );

            GetDefinitionResponse<StageStyle> response = new GetDefinitionResponse<StageStyle>( request );

            return await GetDefinition( request, response ).ConfigureAwait( false );
        }

        public async Task<GetDefinitionResponse<TargetCollection>> GetTargetCollectionDefinition( SetName setName ) {

            var definitionType = DefinitionType.TARGETCOLLECTION;

            GetDefinitionRequest request = new GetDefinitionRequest( setName, definitionType );

            GetDefinitionResponse<TargetCollection> response = new GetDefinitionResponse<TargetCollection>( request );

            return await GetDefinition( request, response ).ConfigureAwait( false );
        }

        public async Task<GetDefinitionResponse<Target>> GetTargetDefinition( SetName setName ) {

            var definitionType = DefinitionType.TARGET;

            GetDefinitionRequest request = new GetDefinitionRequest( setName, definitionType );

            GetDefinitionResponse<Target> response = new GetDefinitionResponse<Target>( request );

            return await GetDefinition( request, response ).ConfigureAwait( false );
        }

        public async Task<GetDefinitionResponse<ScoreFormatCollection>> GetScoreFormatCollectionDefinition( SetName setName ) {

            var definitionType = DefinitionType.SCOREFORMATCOLLECTION;

            GetDefinitionRequest request = new GetDefinitionRequest( setName, definitionType );

            GetDefinitionResponse<ScoreFormatCollection> response = new GetDefinitionResponse<ScoreFormatCollection>( request );

            return await GetDefinition( request, response ).ConfigureAwait( false );
        }

        public async Task<GetDefinitionResponse<ResultListFormat>> GetResultListFormatCollection( SetName setName ) {

            var definitionType = DefinitionType.RESULTLISTFORMAT;

            GetDefinitionRequest request = new GetDefinitionRequest( setName, definitionType );

            GetDefinitionResponse<ResultListFormat> response = new GetDefinitionResponse<ResultListFormat>( request );

            return await GetDefinition( request, response ).ConfigureAwait( false );
        }
    }
}