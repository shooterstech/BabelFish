using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using Scopos.BabelFish.Responses;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Requests.DefinitionAPI;
using Scopos.BabelFish.Responses.DefinitionAPI;
using System.Runtime.CompilerServices;

namespace Scopos.BabelFish.APIClients {
    public class DefinitionAPIClient : APIClient {

        ResponseCache definitionCacheHelper;
        private readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="apiKey"></param>
        public DefinitionAPIClient( string apiKey ) : base( apiKey ) {
            IgnoreLocalCache = false;
        }

        public DefinitionAPIClient( string apiKey, APIStage apiStage ) : base( apiKey, apiStage ) {
			IgnoreLocalCache = false;
		}

        public async Task<GetDefinitionPublicResponse<T>> GetDefinitionAsync<T>( GetDefinitionPublicRequest request, GetDefinitionPublicResponse<T> response ) where T : Definition {

            await this.CallAPIAsync( request, response ).ConfigureAwait( false );
            return response;

        }

        [Obsolete( "Use GetAttributeDefinitionAsync() instead." )]
        public async Task<GetDefinitionPublicResponse<Scopos.BabelFish.DataModel.Definitions.Attribute>> GetAttributeDefinition( SetName setName ) {
            return await GetAttributeDefinitionAsync(setName);
        }

        public async Task<GetDefinitionPublicResponse<Scopos.BabelFish.DataModel.Definitions.Attribute>> GetAttributeDefinitionAsync( SetName setName ) {

            var definitionType = DefinitionType.ATTRIBUTE;

            GetDefinitionPublicRequest request = new GetDefinitionPublicRequest( setName, definitionType );

            GetDefinitionPublicResponse<Scopos.BabelFish.DataModel.Definitions.Attribute> response = new GetDefinitionPublicResponse<Scopos.BabelFish.DataModel.Definitions.Attribute>( request );

            return await GetDefinitionAsync( request, response ).ConfigureAwait( false );
        }

        [Obsolete( "Use GetCourseOfFireDefinitionAsync() instead." )]
        public async Task<GetDefinitionPublicResponse<CourseOfFire>> GetCourseOfFireDefinition( SetName setName ) {
            return await GetCourseOfFireDefinitionAsync(setName);
        }

        public async Task<GetDefinitionPublicResponse<CourseOfFire>> GetCourseOfFireDefinitionAsync( SetName setName ) {

            var definitionType = DefinitionType.COURSEOFFIRE;

            GetDefinitionPublicRequest request = new GetDefinitionPublicRequest( setName, definitionType );

            GetDefinitionPublicResponse<CourseOfFire> response = new GetDefinitionPublicResponse<CourseOfFire>( request );

            return await GetDefinitionAsync( request, response ).ConfigureAwait( false );
        }

        [Obsolete( "Use GetEventStyleDefinitionAsync() instead." )]
        public async Task<GetDefinitionPublicResponse<EventStyle>> GetEventStyleDefinition( SetName setName ) {
            return await GetEventStyleDefinitionAsync( setName );
        }

        public async Task<GetDefinitionPublicResponse<EventStyle>> GetEventStyleDefinitionAsync( SetName setName ) {

            var definitionType = DefinitionType.EVENTSTYLE;

            GetDefinitionPublicRequest request = new GetDefinitionPublicRequest( setName, definitionType );

            GetDefinitionPublicResponse<EventStyle> response = new GetDefinitionPublicResponse<EventStyle>( request );

            return await GetDefinitionAsync( request, response ).ConfigureAwait( false );
        }

        [Obsolete( "Use GetRankingRuleDefinitionAsync() instead." )]
        public async Task<GetDefinitionPublicResponse<RankingRule>> GetRankingRuleDefinition( SetName setName ) {
            return await GetRankingRuleDefinitionAsync(setName);
        }

        public async Task<GetDefinitionPublicResponse<RankingRule>> GetRankingRuleDefinitionAsync( SetName setName ) {

            var definitionType = DefinitionType.RANKINGRULES;

            GetDefinitionPublicRequest request = new GetDefinitionPublicRequest( setName, definitionType );

            GetDefinitionPublicResponse<RankingRule> response = new GetDefinitionPublicResponse<RankingRule>( request );

            return await GetDefinitionAsync( request, response ).ConfigureAwait( false );
        }

        [Obsolete( "Use GetStageStyleDefinitionAsync() instead." )]
        public async Task<GetDefinitionPublicResponse<StageStyle>> GetStageStyleDefinition( SetName setName ) {
            return await GetStageStyleDefinitionAsync(setName);
        }

        public async Task<GetDefinitionPublicResponse<StageStyle>> GetStageStyleDefinitionAsync( SetName setName ) {

            var definitionType = DefinitionType.STAGESTYLE;

            GetDefinitionPublicRequest request = new GetDefinitionPublicRequest( setName, definitionType );

            GetDefinitionPublicResponse<StageStyle> response = new GetDefinitionPublicResponse<StageStyle>( request );

            return await GetDefinitionAsync( request, response ).ConfigureAwait( false );
        }

        [Obsolete( "User GetTargetCollectionDefinitionAsync() instead." )]
        public async Task<GetDefinitionPublicResponse<TargetCollection>> GetTargetCollectionDefinition( SetName setName ) {
            return await GetTargetCollectionDefinitionAsync( setName );
        }

        public async Task<GetDefinitionPublicResponse<TargetCollection>> GetTargetCollectionDefinitionAsync( SetName setName ) {

            var definitionType = DefinitionType.TARGETCOLLECTION;

            GetDefinitionPublicRequest request = new GetDefinitionPublicRequest( setName, definitionType );

            GetDefinitionPublicResponse<TargetCollection> response = new GetDefinitionPublicResponse<TargetCollection>( request );

            return await GetDefinitionAsync( request, response ).ConfigureAwait( false );
        }

        [Obsolete( "Use GetTargetDefinitionAsync instead." )]
        public async Task<GetDefinitionPublicResponse<Target>> GetTargetDefinition( SetName setName ) {
            return await GetTargetDefinitionAsync(setName);
        }

        public async Task<GetDefinitionPublicResponse<Target>> GetTargetDefinitionAsync( SetName setName ) {

            var definitionType = DefinitionType.TARGET;

            GetDefinitionPublicRequest request = new GetDefinitionPublicRequest( setName, definitionType );

            GetDefinitionPublicResponse<Target> response = new GetDefinitionPublicResponse<Target>( request );

            return await GetDefinitionAsync( request, response ).ConfigureAwait( false );
        }

        [Obsolete( "Use GetScoreFormatCollectionDefinitionAsync instead." )]
        public async Task<GetDefinitionPublicResponse<ScoreFormatCollection>> GetScoreFormatCollectionDefinition( SetName setName ) {
            return await GetScoreFormatCollectionDefinitionAsync( setName );
        }

        public async Task<GetDefinitionPublicResponse<ScoreFormatCollection>> GetScoreFormatCollectionDefinitionAsync( SetName setName ) {

            var definitionType = DefinitionType.SCOREFORMATCOLLECTION;

            GetDefinitionPublicRequest request = new GetDefinitionPublicRequest( setName, definitionType );

            GetDefinitionPublicResponse<ScoreFormatCollection> response = new GetDefinitionPublicResponse<ScoreFormatCollection>( request );

            return await GetDefinitionAsync( request, response ).ConfigureAwait( false );
        }

        public async Task<GetDefinitionPublicResponse<EventAndStageStyleMapping>> GetEventAndStageStyhleMappingDefinitionAsync( SetName setName ) {

            var definitionType = DefinitionType.EVENTANDSTAGESTYLEMAPPING;

            GetDefinitionPublicRequest request = new GetDefinitionPublicRequest( setName, definitionType );

            GetDefinitionPublicResponse<EventAndStageStyleMapping> response = new GetDefinitionPublicResponse<EventAndStageStyleMapping>( request );

            return await GetDefinitionAsync( request, response ).ConfigureAwait( false );
        }

        public async Task<GetDefinitionPublicResponse<ResultListFormat>> GetResultListFormatDefinitionAsync( SetName setName ) {

            var definitionType = DefinitionType.RESULTLISTFORMAT;

            GetDefinitionPublicRequest request = new GetDefinitionPublicRequest( setName, definitionType );

            GetDefinitionPublicResponse<ResultListFormat> response = new GetDefinitionPublicResponse<ResultListFormat>( request );

            return await GetDefinitionAsync( request, response ).ConfigureAwait( false );
        }
    }
}