using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using Scopos.BabelFish.DataModel;
using Scopos.BabelFish.Requests;
using Scopos.BabelFish.Responses;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Requests.DefinitionAPI;
using Scopos.BabelFish.Responses.DefinitionAPI;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.APIClients {
    public class DefinitionAPIClient : APIClient<DefinitionAPIClient> {

        ResponseCache definitionCacheHelper;
        private readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="apiKey"></param>
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        public DefinitionAPIClient( ) : base( ) {
            IgnoreInMemoryCache = false;
            IgnoreFileSystemCache = false;
        }

        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        public DefinitionAPIClient( APIStage apiStage ) : base( apiStage ) {
			IgnoreInMemoryCache = false;
            IgnoreFileSystemCache = false;
        }

        /// <summary>
        /// Attempts to read the Definition file from the local file system.
        /// Because this method is async, and we can't have 'out' variables on async methods, this method instead returns a tuple.
        /// .Item1 is a boolean indicating if it was successful.
        /// .Item2 is the ResponseIntermediateObject (if successful) containing the definition file.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Parameter request must be of type GetDefinitionPublicRequest</exception>
        protected override async Task<Tuple<bool, ResponseIntermediateObject?>> TryReadFromFileSystemAsync( Request request ) {

            if (!(request is GetDefinitionPublicRequest)) {
                throw new ArgumentException( "The parameter reqeust, must be of type GetDefinitionPublicRequest." );
            }

            //Make sure the user wants us to use the file system cache
            if (IgnoreInMemoryCache || request.IgnoreFileSystemCache)
                return new Tuple<bool, ResponseIntermediateObject?>( false, null );

            var definitionRequest = (GetDefinitionPublicRequest)request;
            try {
                if (LocalStoreDirectory != null && LocalStoreDirectory.Exists) {

                    string definitionFileName = $"{definitionRequest.SetName}.json".Replace( ':', ' ' );
                    string filename = $"{LocalStoreDirectory.FullName}\\{definitionRequest.DefinitionType.Description()}\\{definitionFileName}";

                    logger.Info( $"Attempting to read definition '{definitionRequest.SetName}' from the file '{filename}'." );

                    FileInfo fileInfo = new FileInfo( filename );
                    if (fileInfo.Exists) {

                        using (StreamReader sr = File.OpenText( filename ))
                        using (JsonTextReader reader = new JsonTextReader( sr )) {
                            var jsonFromFile = await JObject.ReadFromAsync( reader );

                            var response = new ResponseIntermediateObject();
                            response.Request = request;
                            response.MessageResponse = new MessageResponse();
                            //Format the JObject response as if it was read from the REST API
                            response.Body = new JObject();
                            response.Body[definitionRequest.SetName.ToString()] = jsonFromFile;
                            response.ValidUntil = DateTime.UtcNow.AddDays( 1 ); //UMMMM, what's a good value to set here?

                            return new Tuple<bool, ResponseIntermediateObject?>( true, response );
                        }
                    } else {
                        logger.Warn( $"Can't read definition '{filename}' because the file does not exist." );
                    }

                } else {
                    if (LocalStoreDirectory == null)
                        logger.Warn( $"Can't read definition for '{definitionRequest.SetName}', because the LocalStoreDirectory has not been set." );
                    else
                        logger.Warn( $"Can't read definition for '{definitionRequest.SetName}', because the LocalStoreDirectory '{LocalStoreDirectory.FullName}' doesn't exist." );
                }
            } catch (Exception ex) {
                logger.Error( ex, $"Something unexpected happen while reading the definitions for '{definitionRequest.SetName}'." );
            }

            return new Tuple<bool, ResponseIntermediateObject?>( false, null );
        }


        public async Task<GetDefinitionPublicResponse<T>> GetDefinitionAsync<T>( GetDefinitionPublicRequest request, GetDefinitionPublicResponse<T> response ) where T : Definition {

            await this.CallAPIAsync( request, response ).ConfigureAwait( false );
            return response;

        }

        [Obsolete( "Use GetAttributeDefinitionAsync() instead." )]
        public async Task<GetDefinitionPublicResponse<Scopos.BabelFish.DataModel.Definitions.Attribute>> GetAttributeDefinition( SetName setName ) {
            return await GetAttributeDefinitionAsync(setName);
        }

        public virtual async Task<GetDefinitionPublicResponse<Scopos.BabelFish.DataModel.Definitions.Attribute>> GetAttributeDefinitionAsync( SetName setName ) {

            var definitionType = DefinitionType.ATTRIBUTE;

            GetDefinitionPublicRequest request = new GetDefinitionPublicRequest( setName, definitionType );

            GetDefinitionPublicResponse<Scopos.BabelFish.DataModel.Definitions.Attribute> response = new GetDefinitionPublicResponse<Scopos.BabelFish.DataModel.Definitions.Attribute>( request );

            return await GetDefinitionAsync( request, response ).ConfigureAwait( false );
        }

        [Obsolete( "Use GetCourseOfFireDefinitionAsync() instead." )]
        public async Task<GetDefinitionPublicResponse<CourseOfFire>> GetCourseOfFireDefinition( SetName setName ) {
            return await GetCourseOfFireDefinitionAsync(setName);
        }

        public virtual async Task<GetDefinitionPublicResponse<CourseOfFire>> GetCourseOfFireDefinitionAsync( SetName setName ) {

            var definitionType = DefinitionType.COURSEOFFIRE;

            GetDefinitionPublicRequest request = new GetDefinitionPublicRequest( setName, definitionType );

            GetDefinitionPublicResponse<CourseOfFire> response = new GetDefinitionPublicResponse<CourseOfFire>( request );

            return await GetDefinitionAsync( request, response ).ConfigureAwait( false );
        }

        [Obsolete( "Use GetEventStyleDefinitionAsync() instead." )]
        public async Task<GetDefinitionPublicResponse<EventStyle>> GetEventStyleDefinition( SetName setName ) {
            return await GetEventStyleDefinitionAsync( setName );
        }

        public virtual async Task<GetDefinitionPublicResponse<EventStyle>> GetEventStyleDefinitionAsync( SetName setName ) {

            var definitionType = DefinitionType.EVENTSTYLE;

            GetDefinitionPublicRequest request = new GetDefinitionPublicRequest( setName, definitionType );

            GetDefinitionPublicResponse<EventStyle> response = new GetDefinitionPublicResponse<EventStyle>( request );

            return await GetDefinitionAsync( request, response ).ConfigureAwait( false );
        }

        [Obsolete( "Use GetRankingRuleDefinitionAsync() instead." )]
        public async Task<GetDefinitionPublicResponse<RankingRule>> GetRankingRuleDefinition( SetName setName ) {
            return await GetRankingRuleDefinitionAsync(setName);
        }

        public virtual async Task<GetDefinitionPublicResponse<RankingRule>> GetRankingRuleDefinitionAsync( SetName setName ) {

            var definitionType = DefinitionType.RANKINGRULES;

            GetDefinitionPublicRequest request = new GetDefinitionPublicRequest( setName, definitionType );

            GetDefinitionPublicResponse<RankingRule> response = new GetDefinitionPublicResponse<RankingRule>( request );

            return await GetDefinitionAsync( request, response ).ConfigureAwait( false );
        }

        [Obsolete( "Use GetStageStyleDefinitionAsync() instead." )]
        public async Task<GetDefinitionPublicResponse<StageStyle>> GetStageStyleDefinition( SetName setName ) {
            return await GetStageStyleDefinitionAsync(setName);
        }

        public virtual async Task<GetDefinitionPublicResponse<StageStyle>> GetStageStyleDefinitionAsync( SetName setName ) {

            var definitionType = DefinitionType.STAGESTYLE;

            GetDefinitionPublicRequest request = new GetDefinitionPublicRequest( setName, definitionType );

            GetDefinitionPublicResponse<StageStyle> response = new GetDefinitionPublicResponse<StageStyle>( request );

            return await GetDefinitionAsync( request, response ).ConfigureAwait( false );
        }

        [Obsolete( "User GetTargetCollectionDefinitionAsync() instead." )]
        public async Task<GetDefinitionPublicResponse<TargetCollectionDefinition>> GetTargetCollectionDefinition( SetName setName ) {
            return await GetTargetCollectionDefinitionAsync( setName );
        }

        public virtual async Task<GetDefinitionPublicResponse<TargetCollectionDefinition>> GetTargetCollectionDefinitionAsync( SetName setName ) {

            var definitionType = DefinitionType.TARGETCOLLECTION;

            GetDefinitionPublicRequest request = new GetDefinitionPublicRequest( setName, definitionType );

            GetDefinitionPublicResponse<TargetCollectionDefinition> response = new GetDefinitionPublicResponse<TargetCollectionDefinition>( request );

            return await GetDefinitionAsync( request, response ).ConfigureAwait( false );
        }

        [Obsolete( "Use GetTargetDefinitionAsync instead." )]
        public async Task<GetDefinitionPublicResponse<Target>> GetTargetDefinition( SetName setName ) {
            return await GetTargetDefinitionAsync(setName);
        }

        public virtual async Task<GetDefinitionPublicResponse<Target>> GetTargetDefinitionAsync( SetName setName ) {

            var definitionType = DefinitionType.TARGET;

            GetDefinitionPublicRequest request = new GetDefinitionPublicRequest( setName, definitionType );

            GetDefinitionPublicResponse<Target> response = new GetDefinitionPublicResponse<Target>( request );

            return await GetDefinitionAsync( request, response ).ConfigureAwait( false );
        }

        [Obsolete( "Use GetScoreFormatCollectionDefinitionAsync instead." )]
        public async Task<GetDefinitionPublicResponse<ScoreFormatCollection>> GetScoreFormatCollectionDefinition( SetName setName ) {
            return await GetScoreFormatCollectionDefinitionAsync( setName );
        }

        public virtual async Task<GetDefinitionPublicResponse<ScoreFormatCollection>> GetScoreFormatCollectionDefinitionAsync( SetName setName ) {

            var definitionType = DefinitionType.SCOREFORMATCOLLECTION;

            GetDefinitionPublicRequest request = new GetDefinitionPublicRequest( setName, definitionType );

            GetDefinitionPublicResponse<ScoreFormatCollection> response = new GetDefinitionPublicResponse<ScoreFormatCollection>( request );

            return await GetDefinitionAsync( request, response ).ConfigureAwait( false );
        }

		public virtual async Task<GetDefinitionPublicResponse<EventAndStageStyleMapping>> GetEventAndStageStyleMappingDefinitionAsync( SetName setName ) {

			var definitionType = DefinitionType.EVENTANDSTAGESTYLEMAPPING;

			GetDefinitionPublicRequest request = new GetDefinitionPublicRequest( setName, definitionType );

			GetDefinitionPublicResponse<EventAndStageStyleMapping> response = new GetDefinitionPublicResponse<EventAndStageStyleMapping>( request );

			return await GetDefinitionAsync( request, response ).ConfigureAwait( false );
		}

		public virtual async Task<GetDefinitionPublicResponse<ResultListFormat>> GetResultListFormatDefinitionAsync( SetName setName ) {

            var definitionType = DefinitionType.RESULTLISTFORMAT;

            GetDefinitionPublicRequest request = new GetDefinitionPublicRequest( setName, definitionType );

            GetDefinitionPublicResponse<ResultListFormat> response = new GetDefinitionPublicResponse<ResultListFormat>( request );

            return await GetDefinitionAsync( request, response ).ConfigureAwait( false );
        }

        public async Task<GetDefinitionListPublicResponse> GetDefinitionListPublicAsync( GetDefinitionListPublicRequest request ) {

            GetDefinitionListPublicResponse response = new GetDefinitionListPublicResponse( request );

            await this.CallAPIAsync( request, response ).ConfigureAwait( false );

            return response;
		}

        public async Task<GetDefinitionListPublicResponse> GetDefinitionListPublicAsync( DefinitionType type ) {
			GetDefinitionListPublicRequest request = new GetDefinitionListPublicRequest( type );

            return await this.GetDefinitionListPublicAsync( request ).ConfigureAwait( false );

		}

        /// <summary>
        /// Retreives a list of SparseDefinitions in order or relavancy to the provided searchTerm. Maximum 20 items are returned.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
		public async Task<GetDefinitionListPublicResponse> GetDefinitionListPublicAsync( DefinitionType type, string searchTerm ) {
			GetDefinitionListPublicRequest request = new GetDefinitionListPublicRequest( type );
            request.Search = searchTerm;
            request.Limit = 20;

            //As the search matching is done on the server, we will be turning off client level caching.
            request.IgnoreFileSystemCache = true;
            request.IgnoreInMemoryCache = true;

			return await this.GetDefinitionListPublicAsync( request ).ConfigureAwait( false );

		}
	}
}