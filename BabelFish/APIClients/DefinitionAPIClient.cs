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

        public async Task<GetDefinitionPublicResponse<T>> GetDefinition<T>( GetDefinitionPublicRequest request, GetDefinitionPublicResponse<T> response ) where T : Definition {
            try {
                // TimeToRun if we use cache
                DateTime startTime = DateTime.Now;

                T definition;
                if (!IgnoreLocalCache &&
                    !request.IgnoreLocalCache &&
                    DefinitionCache.CACHE.TryGetDefinition<T>( request.DefinitionType, request.SetName, out definition )) {
                    response.TimeToRun = DateTime.Now - startTime;

                    //We found a copy of the definition in our local cache. No need to make the actual API call. Instead, generate
                    //a response object that makes it look like we made the request. 

                    response.StatusCode = System.Net.HttpStatusCode.OK;
                    JObject fakeBody = new JObject {
                        { "Title" , $"Cached response for {request.DefinitionType} {request.SetName}" },
                        { "Message" , new JArray() {$"Cached response for {request.DefinitionType} {request.SetName}" } },
                        { request.SetName.ToString(), JObject.FromObject( definition ) }
                    };

                    response.MessageResponse = fakeBody.ToObject<MessageResponse>();
                    response.Body = fakeBody;

                } else {

                    await this.CallAPI( request, response ).ConfigureAwait( false );

                    // Save returned definition to cache
                    if (response.Body != null)
                        DefinitionCache.CACHE.SaveDefinition<T>( request.SetName, response.Definition );
                }
            } catch (Exception ex) {
                logger.Error( $"GetDefinition Error: {ex.Message}" );
                response.MessageResponse.Message.Add( $"GetDefinition Error: {ex.Message}" );
                response.MessageResponse.ResponseCodes.Add( "GetDefinition Error" );
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }

            return response;
        }

        /// <summary>
        /// Indicates if the local cache should be ignored and always pull the definition from the Rest API.
        /// The default value is false, meaning to use the local cache.
        /// The option to ignore local cache can either be wet at the API Client level, or on a per request level.
        /// </summary>
        public bool IgnoreLocalCache { get; set; } = false;


        public async Task<GetDefinitionPublicResponse<Scopos.BabelFish.DataModel.Definitions.Attribute>> GetAttributeDefinitionAsync( SetName setName ) {

            var definitionType = DefinitionType.ATTRIBUTE;

            GetDefinitionPublicRequest request = new GetDefinitionPublicRequest( setName, definitionType );

            GetDefinitionPublicResponse<Scopos.BabelFish.DataModel.Definitions.Attribute> response = new GetDefinitionPublicResponse<Scopos.BabelFish.DataModel.Definitions.Attribute>( request );

            return await GetDefinition( request, response ).ConfigureAwait( false );
        }


        public async Task<GetDefinitionPublicResponse<CourseOfFire>> GetCourseOfFireDefinition( SetName setName ) {

            var definitionType = DefinitionType.COURSEOFFIRE;

            GetDefinitionPublicRequest request = new GetDefinitionPublicRequest( setName, definitionType );

            GetDefinitionPublicResponse<CourseOfFire> response = new GetDefinitionPublicResponse<CourseOfFire>( request );

            return await GetDefinition( request, response ).ConfigureAwait( false );
        }

        public async Task<GetDefinitionPublicResponse<EventStyle>> GetEventStyleDefinition( SetName setName ) {

            var definitionType = DefinitionType.EVENTSTYLE;

            GetDefinitionPublicRequest request = new GetDefinitionPublicRequest( setName, definitionType );

            GetDefinitionPublicResponse<EventStyle> response = new GetDefinitionPublicResponse<EventStyle>( request );

            return await GetDefinition( request, response ).ConfigureAwait( false );
        }

        public async Task<GetDefinitionPublicResponse<RankingRule>> GetRankingRuleDefinition( SetName setName ) {

            var definitionType = DefinitionType.RANKINGRULES;

            GetDefinitionPublicRequest request = new GetDefinitionPublicRequest( setName, definitionType );

            GetDefinitionPublicResponse<RankingRule> response = new GetDefinitionPublicResponse<RankingRule>( request );

            return await GetDefinition( request, response ).ConfigureAwait( false );
        }

        public async Task<GetDefinitionPublicResponse<StageStyle>> GetStageStyleDefinition( SetName setName ) {

            var definitionType = DefinitionType.STAGESTYLE;

            GetDefinitionPublicRequest request = new GetDefinitionPublicRequest( setName, definitionType );

            GetDefinitionPublicResponse<StageStyle> response = new GetDefinitionPublicResponse<StageStyle>( request );

            return await GetDefinition( request, response ).ConfigureAwait( false );
        }

        public async Task<GetDefinitionPublicResponse<TargetCollection>> GetTargetCollectionDefinition( SetName setName ) {

            var definitionType = DefinitionType.TARGETCOLLECTION;

            GetDefinitionPublicRequest request = new GetDefinitionPublicRequest( setName, definitionType );

            GetDefinitionPublicResponse<TargetCollection> response = new GetDefinitionPublicResponse<TargetCollection>( request );

            return await GetDefinition( request, response ).ConfigureAwait( false );
        }

        public async Task<GetDefinitionPublicResponse<Target>> GetTargetDefinition( SetName setName ) {

            var definitionType = DefinitionType.TARGET;

            GetDefinitionPublicRequest request = new GetDefinitionPublicRequest( setName, definitionType );

            GetDefinitionPublicResponse<Target> response = new GetDefinitionPublicResponse<Target>( request );

            return await GetDefinition( request, response ).ConfigureAwait( false );
        }

        public async Task<GetDefinitionPublicResponse<ScoreFormatCollection>> GetScoreFormatCollectionDefinition( SetName setName ) {

            var definitionType = DefinitionType.SCOREFORMATCOLLECTION;

            GetDefinitionPublicRequest request = new GetDefinitionPublicRequest( setName, definitionType );

            GetDefinitionPublicResponse<ScoreFormatCollection> response = new GetDefinitionPublicResponse<ScoreFormatCollection>( request );

            return await GetDefinition( request, response ).ConfigureAwait( false );
        }
    }
}