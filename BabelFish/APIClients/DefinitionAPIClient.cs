using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scopos.BabelFish.Requests;
using Scopos.BabelFish.Responses;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Requests.DefinitionAPI;
using Scopos.BabelFish.Responses.DefinitionAPI;
using Scopos.BabelFish.Helpers;
using System.Net;
using System.Text.Json;
using Scopos.BabelFish.DataModel;
using Newtonsoft.Json;

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
                if (LocalDefinitionDirectory != null && LocalDefinitionDirectory.Exists) {

                    string definitionFileName = $"{definitionRequest.SetName}.json".Replace( ':', ' ' );
                    string filename = $"{LocalDefinitionDirectory.FullName}\\{definitionRequest.DefinitionType.Description()}\\{definitionFileName}";

                    logger.Trace( $"Attempting to read definition '{definitionRequest.SetName}' from the file '{filename}'." );

                    FileInfo fileInfo = new FileInfo( filename );
                    if (fileInfo.Exists) {

                        var definitionJsonAsSring = File.ReadAllText( filename );
                        var responseAsString = $"{{ \"{definitionRequest.SetName}\" : {definitionJsonAsSring} }}";
                        var responseBody = G_STJ.JsonDocument.Parse( responseAsString );

                        var responseIntObj = new ResponseIntermediateObject();
                        responseIntObj.StatusCode = HttpStatusCode.OK;
                        responseIntObj.Body = responseBody;
                        responseIntObj.Request = request;
                        responseIntObj.ValidUntil = DateTime.UtcNow.AddDays( 1 );

                        logger.Info( $"Returning a in-memory cached Response for {request}." );


                        return new Tuple<bool, ResponseIntermediateObject?>( true, responseIntObj );

                    } else {
                        logger.Warn( $"Can't read definition '{filename}' because the file does not exist." );
                    }

                } else {
                    if (LocalDefinitionDirectory == null)
                        logger.Warn( $"Can't read definition for '{definitionRequest.SetName}', because the LocalStoreDirectory has not been set." );
                    else
                        logger.Warn( $"Can't read definition for '{definitionRequest.SetName}', because the LocalStoreDirectory '{LocalDefinitionDirectory.FullName}' doesn't exist." );
                }
            } catch (Exception ex) {
                logger.Error( ex, $"Something unexpected happen while reading the definitions for '{definitionRequest.SetName}'." );
            }

            return new Tuple<bool, ResponseIntermediateObject?>( false, null );
        }

        protected override Task TryWriteToFileSystemAsync<T>( Response<T> response ) {

            var request = response.Request;

            //No need to write to file system, if the response came from the file system or memory cache
            if (response.FileSystemCachedResponse || response.InMemoryCachedResponse)
                return Task.CompletedTask;

            var definitionRequest = (GetDefinitionPublicRequest)request;

            try {
                if (LocalDefinitionDirectory != null ) {

                    //Create the directory structure
                    string definitionTypeDirectory = $"{LocalDefinitionDirectory.FullName}\\{definitionRequest.DefinitionType.Description()}";
                    if ( ! Directory.Exists( definitionTypeDirectory ) )
                        Directory.CreateDirectory( definitionTypeDirectory );

                    string definitionFileName = $"{definitionRequest.SetName}.json".Replace( ':', ' ' );
                    string fullFilename = $"{definitionTypeDirectory}\\{definitionFileName}";

                    bool writeThefile = false;
                    if (!File.Exists( fullFilename )) {
                        //Write the file if it doesn't exist
                        writeThefile = true;
                    } else {
                        //if The file does exist, only write if its older than 5 mins
                        DateTime lastWriteTime = File.GetLastWriteTime( fullFilename );
                        if ((DateTime.Now - lastWriteTime).TotalMilliseconds > 5)
                            writeThefile = true;
                    }

                    if (writeThefile) {
                        string json = JsonConvert.SerializeObject( response.Value, SerializerOptions.NewtonsoftJsonSerializer );

                        File.WriteAllText( fullFilename, json );
                    }

                } else {
                    logger.Warn( $"Not writing {definitionRequest.DefinitionType.Description()} definition {definitionRequest.SetName} to the file system because the LocalStoreDirectory has not been set." );
                }
            } catch (Exception ex ) {
                logger.Error( ex, $"Unable to write {definitionRequest.DefinitionType.Description()} definition {definitionRequest.SetName} to the file system due to an exception {ex}." );
            }

            return Task.CompletedTask;
        }

        public static DirectoryInfo LocalDefinitionDirectory {
            get {
                if (LocalStoreDirectory == null)
                    return null;

                var definitionDirectory = $"{LocalStoreDirectory.FullName}\\DEFINITIONS";

                return new DirectoryInfo( definitionDirectory );
            }
        }


        public async Task<GetDefinitionPublicResponse<T>> GetDefinitionAsync<T>( GetDefinitionPublicRequest request, GetDefinitionPublicResponse<T> response ) where T : Definition, new() {

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
        public async Task<GetDefinitionPublicResponse<TargetCollection>> GetTargetCollectionDefinition( SetName setName ) {
            return await GetTargetCollectionDefinitionAsync( setName );
        }

        public virtual async Task<GetDefinitionPublicResponse<TargetCollection>> GetTargetCollectionDefinitionAsync( SetName setName ) {

            var definitionType = DefinitionType.TARGETCOLLECTION;

            GetDefinitionPublicRequest request = new GetDefinitionPublicRequest( setName, definitionType );

            GetDefinitionPublicResponse<TargetCollection> response = new GetDefinitionPublicResponse<TargetCollection>( request );

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

        public async Task<GetDefinitionVersionPublicResponse> GetDefinitionVersionPublicAsync(
            GetDefinitionVersionPublicRequest request ) {

            GetDefinitionVersionPublicResponse response = new GetDefinitionVersionPublicResponse( request );

            await this.CallAPIAsync( request, response ).ConfigureAwait ( false );

            return response;
        }

        public async Task<GetDefinitionVersionPublicResponse> GetDefinitionVersionPublicAsync( 
            DefinitionType type, SetName setName ) {
            GetDefinitionVersionPublicRequest request = new GetDefinitionVersionPublicRequest( setName, type );
            //as we always want to return the latest version number, we will turn off cache
            request.IgnoreFileSystemCache = true;
            request.IgnoreInMemoryCache = true;

            return await this.GetDefinitionVersionPublicAsync(request ).ConfigureAwait( false );
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