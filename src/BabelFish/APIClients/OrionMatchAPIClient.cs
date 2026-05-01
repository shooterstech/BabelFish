using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Responses.OrionMatchAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.APIClients {
    public class OrionMatchAPIClient :
        APIClient<OrionMatchAPIClient>,
        IResultListFetcher {

        /// <summary>
        /// Default constructor.
        /// Assumes Production stage level.
        /// </summary>
        /// <param name="xapikey"></param>
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        public OrionMatchAPIClient() : base() {
            //enable in memory cache
            IgnoreInMemoryCache = false;

            //OrionMatchAPIClient does not support file system cache
            LocalStoreDirectory = null;
            IgnoreFileSystemCache = true;
        }

        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        public OrionMatchAPIClient( APIStage apiStage ) : base( apiStage ) {
            //enable in memory cache
            IgnoreInMemoryCache = false;

            //OrionMatchAPIClient does not support file system cache
            LocalStoreDirectory = null;
            IgnoreFileSystemCache = true;
        }

        #region Match API Calls
        /// <summary>
        /// Get Match Detail API
        /// </summary>
        /// <param name="requestParameters">GetMatchRequest object</param>
        /// <returns>Match Object</returns>
        /// <remarks>
        /// Visit our <see href="https://github.com/shooterstech/scopos-labs/blob/master/csharp/Command%20Line%20Examples/Match%20API%20Example/Program.cs">Scopos-labs</see>
        /// project to see an example of using BabelFish to retreive information about a match, retreiving the primary result lists from that match, and using
        /// the result list intermediate formatter to format the result list to the console.
        /// </remarks>
        public async Task<GetMatchPublicResponse> GetMatchPublicAsync( GetMatchPublicRequest requestParameters ) {

            GetMatchPublicResponse response = new GetMatchPublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Get Match Detail API
        /// </summary>
        /// <param name="matchid"></param>
        /// <returns>Match Object</returns>
        /// <remarks>
        /// Visit our <see href="https://github.com/shooterstech/scopos-labs/blob/master/csharp/Command%20Line%20Examples/Match%20API%20Example/Program.cs">Scopos-labs</see>
        /// project to see an example of using BabelFish to retreive information about a match, retreiving the primary result lists from that match, and using
        /// the result list intermediate formatter to format the result list to the console.
        /// </remarks>
        public async Task<GetMatchPublicResponse> GetMatchPublicAsync( MatchID matchid ) {
            var request = new GetMatchPublicRequest( matchid );

            return await GetMatchPublicAsync( request );
        }

        /// <summary>
        /// Get Match Detail API
        /// </summary>
        /// <param name="requestParameters">GetMatchRequest object</param>
        /// <returns>Match Object</returns>
        /// <remarks>
        /// Visit our <see href="https://github.com/shooterstech/scopos-labs/blob/master/csharp/Command%20Line%20Examples/Match%20API%20Example/Program.cs">Scopos-labs</see>
        /// project to see an example of using BabelFish to retreive information about a match, retreiving the primary result lists from that match, and using
        /// the result list intermediate formatter to format the result list to the console.
        /// </remarks>
        public async Task<GetMatchAuthenticatedResponse> GetMatchAuthenticatedAsync( GetMatchAuthenticatedRequest requestParameters ) {

            GetMatchAuthenticatedResponse response = new GetMatchAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Get Match Detail API
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="withAuthentication">default false</param>
        /// <returns>Match Object</returns>
        /// <remarks>
        /// Visit our <see href="https://github.com/shooterstech/scopos-labs/blob/master/csharp/Command%20Line%20Examples/Match%20API%20Example/Program.cs">Scopos-labs</see>
        /// project to see an example of using BabelFish to retreive information about a match, retreiving the primary result lists from that match, and using
        /// the result list intermediate formatter to format the result list to the console.
        /// </remarks>
        public async Task<GetMatchAuthenticatedResponse> GetMatchAuthenticatedAsync( MatchID matchid, UserAuthentication credentials ) {
            var request = new GetMatchAuthenticatedRequest( matchid, credentials );

            return await GetMatchAuthenticatedAsync( request );
        }

        /// <summary>
        /// Function that abstracts the Public vs Authenticated calls. If credentials is null, then a PublicAPI call is made.
        /// If credentials if not null, then an Authenticated API call is made.
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="credentials"></param>
        /// <returns></returns>
        /// <remarks>
        /// Visit our <see href="https://github.com/shooterstech/scopos-labs/blob/master/csharp/Command%20Line%20Examples/Match%20API%20Example/Program.cs">Scopos-labs</see>
        /// project to see an example of using BabelFish to retreive information about a match, retreiving the primary result lists from that match, and using
        /// the result list intermediate formatter to format the result list to the console.
        /// </remarks>
        public async Task<GetMatchAbstractResponse> GetMatchAsync( MatchID matchid, UserAuthentication? credentials = null ) {
            if (credentials == null) {
                return await GetMatchPublicAsync( matchid );
            } else {
                return await GetMatchAuthenticatedAsync( matchid, credentials );
            }
        }

        /// <remarks>
        /// Visit our <see href="https://github.com/shooterstech/scopos-labs/blob/master/csharp/Command%20Line%20Examples/Match%20API%20Example/Program.cs">Scopos-labs</see>
        /// project to see an example of using BabelFish to retreive information about a match, retreiving the primary result lists from that match, and using
        /// the result list intermediate formatter to format the result list to the console.
        /// </remarks>
        public async Task<GetMatchAbstractResponse> GetMatchAsync( GetMatchAbstractRequest requestParameters ) {
            if (requestParameters is GetMatchPublicRequest)
                return await this.GetMatchPublicAsync( (GetMatchPublicRequest)requestParameters );
            else if (requestParameters is GetMatchAuthenticatedRequest)
                return await this.GetMatchAuthenticatedAsync( (GetMatchAuthenticatedRequest)requestParameters );
            else
                //We shouldn't ever get here
                throw new ArgumentException( $"requestParameters is of unexpected type ${requestParameters.GetType()}." );
        }

        /// <summary>
        /// Create Match Child API
        /// </summary>
        /// <param name="requestParameters">CreateMatchChildAuthenticatedRequest object</param>
        /// <returns>Match Child data</returns>
        public async Task<CreateMatchChildAuthenticatedResponse> CreateMatchChildAuthenticatedAsync( CreateMatchChildAuthenticatedRequest requestParameters ) {
            CreateMatchChildAuthenticatedResponse response = new CreateMatchChildAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Create Match Child API
        /// </summary>
        /// <param name="parentMatchId"></param>
        /// <param name="ownerId"></param>
        /// <param name="name"></param>
        /// <param name="credentials"></param>
        /// <returns>Match Child data</returns>
        public async Task<CreateMatchChildAuthenticatedResponse> CreateMatchChildAuthenticatedAsync( MatchID parentMatchId, string ownerId, string name, UserAuthentication credentials ) {
            var request = new CreateMatchChildAuthenticatedRequest( credentials, parentMatchId, ownerId, name );

            return await CreateMatchChildAuthenticatedAsync( request );
        }

        /// <summary>
        /// List Parent Match Children API
        /// </summary>
        /// <param name="requestParameters">ListParentMatchChildrenPublicRequest object</param>
        /// <returns>Match Child List data</returns>
        public async Task<ListParentMatchChildrenPublicResponse> ListParentMatchChildrenPublicAsync( ListParentMatchChildrenPublicRequest requestParameters ) {
            ListParentMatchChildrenPublicResponse response = new ListParentMatchChildrenPublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// List Parent Match Children API
        /// </summary>
        /// <param name="parentMatchId"></param>
        /// <returns>Match Child List data</returns>
        public async Task<ListParentMatchChildrenPublicResponse> ListParentMatchChildrenPublicAsync( MatchID parentMatchId ) {
            var request = new ListParentMatchChildrenPublicRequest( parentMatchId );

            return await ListParentMatchChildrenPublicAsync( request );
        }

        /// <summary>
        /// List Parent Match Children API
        /// </summary>
        /// <param name="requestParameters">ListParentMatchChildrenAuthenticatedRequest object</param>
        /// <returns>Match Child List data</returns>
        public async Task<ListParentMatchChildrenAuthenticatedResponse> ListParentMatchChildrenAuthenticatedAsync( ListParentMatchChildrenAuthenticatedRequest requestParameters ) {
            ListParentMatchChildrenAuthenticatedResponse response = new ListParentMatchChildrenAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// List Parent Match Children API
        /// </summary>
        /// <param name="parentMatchId"></param>
        /// <param name="credentials"></param>
        /// <returns>Match Child List data</returns>
        public async Task<ListParentMatchChildrenAuthenticatedResponse> ListParentMatchChildrenAuthenticatedAsync( MatchID parentMatchId, UserAuthentication credentials ) {
            var request = new ListParentMatchChildrenAuthenticatedRequest( credentials, parentMatchId );

            return await ListParentMatchChildrenAuthenticatedAsync( request );
        }

        /// <summary>
        /// Function that abstracts the Public vs Authenticated calls. If credentials is null, then a PublicAPI call is made.
        /// If credentials if not null, then an Authenticated API call is made.
        /// </summary>
        /// <param name="parentMatchId"></param>
        /// <param name="credentials"></param>
        /// <returns>Match Child List data</returns>
        public async Task<ListParentMatchChildrenAbstractResponse> ListParentMatchChildrenAsync( MatchID parentMatchId, UserAuthentication? credentials = null ) {
            if (credentials == null) {
                return await ListParentMatchChildrenPublicAsync( parentMatchId );
            } else {
                return await ListParentMatchChildrenAuthenticatedAsync( parentMatchId, credentials );
            }
        }

        /// <summary>
        /// Function that abstracts the Public vs Authenticated calls.
        /// </summary>
        /// <param name="requestParameters">ListParentMatchChildrenAbstractRequest object</param>
        /// <returns>Match Child List data</returns>
        public async Task<ListParentMatchChildrenAbstractResponse> ListParentMatchChildrenAsync( ListParentMatchChildrenAbstractRequest requestParameters ) {
            if (requestParameters is ListParentMatchChildrenPublicRequest)
                return await this.ListParentMatchChildrenPublicAsync( (ListParentMatchChildrenPublicRequest)requestParameters );
            else if (requestParameters is ListParentMatchChildrenAuthenticatedRequest)
                return await this.ListParentMatchChildrenAuthenticatedAsync( (ListParentMatchChildrenAuthenticatedRequest)requestParameters );
            else
                throw new ArgumentException( $"requestParameters is of unexpected type ${requestParameters.GetType()}." );
        }

        /// <summary>
        /// Patch Match Child API
        /// </summary>
        /// <param name="requestParameters">PatchMatchChildAuthenticatedRequest object</param>
        /// <returns>Match Child data</returns>
        public async Task<PatchMatchChildAuthenticatedResponse> PatchMatchChildAuthenticatedAsync( PatchMatchChildAuthenticatedRequest requestParameters ) {
            PatchMatchChildAuthenticatedResponse response = new PatchMatchChildAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Patch Match Child API
        /// </summary>
        /// <param name="matchChild"></param>
        /// <param name="credentials"></param>
        /// <returns>Match Child data</returns>
        public async Task<PatchMatchChildAuthenticatedResponse> PatchMatchChildAuthenticatedAsync( MatchChild matchChild, UserAuthentication credentials ) {
            var request = new PatchMatchChildAuthenticatedRequest( credentials, matchChild );

            return await PatchMatchChildAuthenticatedAsync( request );
        }

        public async Task<PatchMatchChildAuthenticatedResponse> ApproveMatchChildAsync( MatchChild matchChild, UserAuthentication credentials ) {
            matchChild.ApprovalStatus = ApprovalStatus.APPROVED;
            var request = new PatchMatchChildAuthenticatedRequest( credentials, matchChild );

            return await PatchMatchChildAuthenticatedAsync( request );
        }
        #endregion

        #region Get Result List
        /// <summary>
        /// Get Result List API
        /// </summary>
        /// <param name="requestParameters">GetResultListRequest object</param>
        /// <returns>ResultList Object</returns>
        /// <remarks>
        /// Visit our <see href="https://github.com/shooterstech/scopos-labs/blob/master/csharp/Command%20Line%20Examples/Match%20API%20Example/Program.cs">Scopos-labs</see>
        /// project to see an example of using BabelFish to retreive information about a match, retreiving the primary result lists from that match, and using
        /// the result list intermediate formatter to format the result list to the console.
        /// </remarks>
        public async Task<GetResultListPublicResponse> GetResultListPublicAsync( GetResultListPublicRequest requestParameters ) {
            GetResultListPublicResponse response = new GetResultListPublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );
            await response.PostResponseProcessingAsync().ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Get Result List API
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="listname"></param>
        /// <returns>ResultList Object</returns>
        /// <remarks>
        /// Visit our <see href="https://github.com/shooterstech/scopos-labs/blob/master/csharp/Command%20Line%20Examples/Match%20API%20Example/Program.cs">Scopos-labs</see>
        /// project to see an example of using BabelFish to retreive information about a match, retreiving the primary result lists from that match, and using
        /// the result list intermediate formatter to format the result list to the console.
        /// </remarks>
        public async Task<GetResultListPublicResponse> GetResultListPublicAsync( MatchID matchid, string listname ) {
            return await GetResultListPublicAsync( new GetResultListPublicRequest( matchid, listname ) ).ConfigureAwait( false );
        }

        /// <summary>
        /// Get Result List API
        /// </summary>
        /// <param name="requestParameters">GetResultListRequest object</param>
        /// <returns>ResultList Object</returns>
        /// <remarks>
        /// Visit our <see href="https://github.com/shooterstech/scopos-labs/blob/master/csharp/Command%20Line%20Examples/Match%20API%20Example/Program.cs">Scopos-labs</see>
        /// project to see an example of using BabelFish to retreive information about a match, retreiving the primary result lists from that match, and using
        /// the result list intermediate formatter to format the result list to the console.
        /// </remarks>
        public async Task<GetResultListAuthenticatedResponse> GetResultListAuthenticatedAsync( GetResultListAuthenticatedRequest requestParameters ) {
            GetResultListAuthenticatedResponse response = new GetResultListAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );
            await response.PostResponseProcessingAsync().ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Get Result List API
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="listname"></param>
        /// <returns>ResultList Object</returns>
        /// <remarks>
        /// Visit our <see href="https://github.com/shooterstech/scopos-labs/blob/master/csharp/Command%20Line%20Examples/Match%20API%20Example/Program.cs">Scopos-labs</see>
        /// project to see an example of using BabelFish to retreive information about a match, retreiving the primary result lists from that match, and using
        /// the result list intermediate formatter to format the result list to the console.
        /// </remarks>
        public async Task<GetResultListAuthenticatedResponse> GetResultListAuthenticatedAsync( MatchID matchid, string listname, UserAuthentication credentials ) {
            return await GetResultListAuthenticatedAsync( new GetResultListAuthenticatedRequest( matchid, listname, credentials ) ).ConfigureAwait( false );
        }

        /// <summary>
        /// Function that abstracts the Public vs Authenticated calls. If credentials is null, then a PublicAPI call is made.
        /// If credentials if not null, then an Authenticated API call is made.
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="credentials"></param>
        /// <returns></returns>
        /// <remarks>
        /// Visit our <see href="https://github.com/shooterstech/scopos-labs/blob/master/csharp/Command%20Line%20Examples/Match%20API%20Example/Program.cs">Scopos-labs</see>
        /// project to see an example of using BabelFish to retreive information about a match, retreiving the primary result lists from that match, and using
        /// the result list intermediate formatter to format the result list to the console.
        /// </remarks>
        public async Task<GetResultListAbstractResponse> GetResultListAsync( MatchID matchid, string listname, UserAuthentication? credentials = null ) {
            if (credentials == null) {
                return await GetResultListPublicAsync( matchid, listname );
            } else {
                return await GetResultListAuthenticatedAsync( matchid, listname, credentials );
            }
        }

        /// <remarks>
        /// Visit our <see href="https://github.com/shooterstech/scopos-labs/blob/master/csharp/Command%20Line%20Examples/Match%20API%20Example/Program.cs">Scopos-labs</see>
        /// project to see an example of using BabelFish to retreive information about a match, retreiving the primary result lists from that match, and using
        /// the result list intermediate formatter to format the result list to the console.
        /// </remarks>
        public async Task<GetResultListAbstractResponse> GetResultListAsync( GetResultListAbstractRequest requestParameters ) {
            if (requestParameters is GetResultListPublicRequest)
                return await this.GetResultListPublicAsync( (GetResultListPublicRequest)requestParameters );
            else if (requestParameters is GetResultListAuthenticatedRequest)
                return await this.GetResultListAuthenticatedAsync( (GetResultListAuthenticatedRequest)requestParameters );
            else
                //We shouldn't ever get here
                throw new ArgumentException( $"requestParameters is of unexpected type ${requestParameters.GetType()}." );
        }

        /// <inheritdoc />
        public async Task<List<ResultList>> GetResultListsAsync( MergedResultList mergedResultList ) {
            var tasks = new List<Task<ResultList?>>();

            // Retreives each ResultList in parallel, and if there are multiple pages of results for a ResultList, retreive those pages sequentially and add them to the same ResultList object.
            foreach (var rlm in mergedResultList.ResultListMembers) {
                tasks.Add( Task.Run( async () => {
                    var getResultListRequest = new GetResultListPublicRequest( rlm.MatchId, rlm.ResultName );
                    ResultList? resultList = null;
                    GetResultListPublicResponse getResultListResponse;
                    do {
                        getResultListResponse = await this.GetResultListPublicAsync( getResultListRequest );
                        if (getResultListResponse.HasOkStatusCode) {
                            if (resultList is null) {
                                resultList = getResultListResponse.ResultList;
                            } else {
                                resultList.Items.AddRange( getResultListResponse.ResultList.Items );
                            }
                            if (getResultListResponse.HasMoreItems)
                                getResultListRequest = (GetResultListPublicRequest)getResultListResponse.GetNextRequest();
                        } else {
                            var msg = $"Could not add the Result List {rlm.ResultName} from {rlm.MatchId}. Received error '{getResultListResponse.OverallStatusCode}' and '{getResultListResponse.RestApiStatusCode}' instead.";
                            _logger.Error( msg );
                        }
                    } while (getResultListResponse.HasMoreItems);
                    return resultList;
                } ) );
            }

            var resultLists = await Task.WhenAll( tasks );
            var resultListsToReturn = new List<ResultList>();
            foreach (var resultList in resultLists) {
                if (resultList != null) {
                    resultListsToReturn.Add( resultList );
                }
            }
            return resultListsToReturn;
        }
        #endregion

        #region Get Squadding List
        /// <summary>
        /// Get Squadding List API
        /// </summary>
        /// <param name="requestParameters">GetSquaddingListRequest object</param>
        /// <returns>Squadding Object</returns>
        public async Task<GetSquaddingListPublicResponse> GetSquaddingListPublicAsync( GetSquaddingListPublicRequest requestParameters ) {

            GetSquaddingListPublicResponse response = new GetSquaddingListPublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );
            await response.PostResponseProcessingAsync().ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Get Squadding List API
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="squaddinglistname"></param>
        /// <returns>Squadding Object</returns>
        public async Task<GetSquaddingListPublicResponse> GetSquaddingListPublicAsync( MatchID matchid, string squaddinglistname ) {
            var request = new GetSquaddingListPublicRequest( matchid, squaddinglistname );

            return await GetSquaddingListPublicAsync( request );
        }

        /// <summary>
        /// Get Squadding List API, limit the response's length by the passed in relayName
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="squaddinglistname"></param>
        /// <param name="relayName"></param>
        /// <returns>Squadding Object</returns>
        public async Task<GetSquaddingListPublicResponse> GetSquaddingListPublicAsync( MatchID matchid, string squaddinglistname, string relayName ) {
            var request = new GetSquaddingListPublicRequest( matchid, squaddinglistname );
            request.RelayName = relayName;

            return await GetSquaddingListPublicAsync( request );
        }

        /// <summary>
        /// Get Squadding List API
        /// </summary>
        /// <param name="requestParameters">GetSquaddingListRequest object</param>
        /// <returns>Squadding Object</returns>
        public async Task<GetSquaddingListAuthenticatedResponse> GetSquaddingListAuthenticatedAsync( GetSquaddingListAuthenticatedRequest requestParameters ) {

            GetSquaddingListAuthenticatedResponse response = new GetSquaddingListAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );
            await response.PostResponseProcessingAsync().ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Get Squadding List API
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="squaddinglistname"></param>
        /// <returns>Squadding Object</returns>
        public async Task<GetSquaddingListAuthenticatedResponse> GetSquaddingListAuthenticatedAsync( MatchID matchid, string squaddinglistname, UserAuthentication credentials ) {
            var request = new GetSquaddingListAuthenticatedRequest( matchid, squaddinglistname, credentials );

            return await GetSquaddingListAuthenticatedAsync( request );
        }

        /// <summary>
        /// Get Squadding List API, limit the response's length by the passed in relayName
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="squaddinglistname"></param>
        /// <param name="relayName"></param>
        /// <returns>Squadding Object</returns>
        public async Task<GetSquaddingListAuthenticatedResponse> GetSquaddingListAuthenticatedAsync( MatchID matchid, string squaddinglistname, string relayName, UserAuthentication credentials ) {
            var request = new GetSquaddingListAuthenticatedRequest( matchid, squaddinglistname, credentials );
            request.RelayName = relayName;

            return await GetSquaddingListAuthenticatedAsync( request );
        }

        public async Task<GetSquaddingListAbstractResponse> GetSquaddingListAsync( MatchID matchId, string squaddinglistname, UserAuthentication credentials = null ) {
            if (credentials == null) {
                return await GetSquaddingListPublicAsync( matchId, squaddinglistname );
            } else {
                return await GetSquaddingListAuthenticatedAsync( matchId, squaddinglistname, credentials );
            }
        }

        public async Task<GetSquaddingListAbstractResponse> GetSquaddingListAsync( GetSquaddingListAbstractRequest requestParameters ) {
            if (requestParameters is GetSquaddingListPublicRequest)
                return await this.GetSquaddingListPublicAsync( (GetSquaddingListPublicRequest)requestParameters );
            else if (requestParameters is GetSquaddingListAuthenticatedRequest)
                return await this.GetSquaddingListAuthenticatedAsync( (GetSquaddingListAuthenticatedRequest)requestParameters );
            else
                //We shouldn't ever get here
                throw new ArgumentException( $"requestParameters is of unexpected type ${requestParameters.GetType()}." );
        }

        #endregion

        #region Get Result COF
        /// <summary>
        /// Get Course Of Fire Detail API
        /// </summary>
        /// <param name="requestParameters">GetResultCOFDetailRequest object</param>
        /// <returns>ResultCOF Object</returns>
        public async Task<GetResultCOFPublicResponse> GetResultCourseOfFireDetailPublicAsync( GetResultCOFPublicRequest requestParameters ) {
            GetResultCOFPublicResponse response = new GetResultCOFPublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response ).ConfigureAwait( false );
            await response.PostResponseProcessingAsync().ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Get Course Of Fire Detail API
        /// </summary>
        /// <param name="resultCOFID"></param>
        /// <returns>ResultCOF Object</returns>
        public async Task<GetResultCOFPublicResponse> GetResultCourseOfFireDetailPublicAsync( string resultCOFID ) {
            return await GetResultCourseOfFireDetailPublicAsync( new GetResultCOFPublicRequest( resultCOFID ) );
        }

        /// <summary>
        /// Get Course Of Fire Detail API
        /// </summary>
        /// <param name="requestParameters">GetResultCOFDetailRequest object</param>
        /// <returns>ResultCOF Object</returns>
        public async Task<GetResultCOFAuthenticatedResponse> GetResultCourseOfFireDetailAuthenticatedAsync( GetResultCOFAuthenticatedRequest requestParameters ) {
            GetResultCOFAuthenticatedResponse response = new GetResultCOFAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );
            await response.PostResponseProcessingAsync().ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Get Course Of Fire Detail API
        /// </summary>
        /// <param name="resultCOFID"></param>
        /// <returns>ResultCOF Object</returns>
        public async Task<GetResultCOFAuthenticatedResponse> GetResultCourseOfFireDetailAuthenticatedAsync( string resultCOFID, UserAuthentication credentials ) {
            return await GetResultCourseOfFireDetailAuthenticatedAsync( new GetResultCOFAuthenticatedRequest( resultCOFID, credentials ) );
        }

        /// <summary>
        /// Function that abstracts the Public vs Authenticated calls. If credentials is null, then a PublicAPI call is made.
        /// If credentials if not null, then an Authenticated API call is made.
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="credentials"></param>
        /// <returns></returns>
        public async Task<GetResultCOFAbstractResponse> GetResultCourseOfFireDetailAsync( string resultCOFID, UserAuthentication? credentials = null ) {
            if (credentials == null) {
                return await GetResultCourseOfFireDetailPublicAsync( resultCOFID );
            } else {
                return await GetResultCourseOfFireDetailAuthenticatedAsync( resultCOFID, credentials );
            }
        }

        public async Task<GetResultCOFAbstractResponse> GetResultCourseOfFireDetailAsync( GetResultCOFAbstractRequest requestParameters ) {
            if (requestParameters is GetResultCOFPublicRequest)
                return await this.GetResultCourseOfFireDetailPublicAsync( (GetResultCOFPublicRequest)(requestParameters) );
            else if (requestParameters is GetResultCOFAuthenticatedRequest)
                return await this.GetResultCourseOfFireDetailAuthenticatedAsync( (GetResultCOFAuthenticatedRequest)requestParameters );
            else
                //We shouldn't ever get here
                throw new ArgumentException( $"requestParameters is of unexpected type ${requestParameters.GetType()}." );
        }
        #endregion

        #region Match Search
        /// <summary>
        /// Get Match Search API
        /// </summary>
        /// <param name="requestParameters"><seealso cref="MatchSearchPublicRequest"/></param>
        /// <returns><seealso cref="MatchSearchPublicResponse"/></returns>
        /// <remarks>
        /// Visit our Scopos-Labs project to see an example of using GetMatchSearch() to retreive a list of ResultListAbbr.
        /// <seealso href="https://github.com/shooterstech/scopos-labs/blob/master/csharp/Command Line Examples/Match Search API Example/Program.cs" />
        /// </remarks>
        public async Task<MatchSearchPublicResponse> GetMatchSearchPublicAsync( MatchSearchPublicRequest requestParameters ) {
            MatchSearchPublicResponse response = new MatchSearchPublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Get Match Search API
        /// </summary>
        /// <param name="requestParameters">GetMatchSearchRequest object</param>
        /// <returns>List<Match> Object</returns>
        public async Task<MatchSearchAuthenticatedResponse> GetMatchSearchAuthenticatedAsync( MatchSearchAuthenticatedRequest requestParameters ) {
            MatchSearchAuthenticatedResponse response = new MatchSearchAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <remarks>
        /// Visit our Scopos-Labs project to see an example of using GetMatchSearch() to retreive a list of ResultListAbbr.
        /// <seealso href="https://github.com/shooterstech/scopos-labs/blob/master/csharp/Command Line Examples/Match Search API Example/Program.cs" />
        /// </remarks>
        public async Task<MatchSearchAbstractResponse> GetMatchSearchAsync( MatchSearchAbstractRequest requestParameters ) {
            if (requestParameters is MatchSearchPublicRequest)
                return await this.GetMatchSearchPublicAsync( (MatchSearchPublicRequest)requestParameters );
            else if (requestParameters is MatchSearchAuthenticatedRequest)
                return await this.GetMatchSearchAuthenticatedAsync( (MatchSearchAuthenticatedRequest)requestParameters );
            else
                //We shouldn't ever get here
                throw new ArgumentException( $"requestParameters is of unexpected type ${requestParameters.GetType()}." );
        }
        #endregion

        #region List Matches
        /// <summary>
        /// Lists public parent and child matches.
        /// </summary>
        /// <param name="requestParameters">ListMatchesPublicRequest object</param>
        /// <returns>Match list data</returns>
        public async Task<ListMatchesPublicResponse> ListMatchesPublicAsync( ListMatchesPublicRequest requestParameters ) {
            ListMatchesPublicResponse response = new ListMatchesPublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Lists public parent and child matches.
        /// </summary>
        /// <returns>Match list data</returns>
        public async Task<ListMatchesPublicResponse> ListMatchesPublicAsync() {
            var request = new ListMatchesPublicRequest();

            return await ListMatchesPublicAsync( request );
        }

        /// <summary>
        /// Lists parent and child matches visible to the authenticated caller.
        /// </summary>
        /// <param name="requestParameters">ListMatchesAuthenticatedRequest object</param>
        /// <returns>Match list data</returns>
        public async Task<ListMatchesAuthenticatedResponse> ListMatchesAuthenticatedAsync( ListMatchesAuthenticatedRequest requestParameters ) {
            ListMatchesAuthenticatedResponse response = new ListMatchesAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Lists parent and child matches visible to the authenticated caller.
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns>Match list data</returns>
        public async Task<ListMatchesAuthenticatedResponse> ListMatchesAuthenticatedAsync( UserAuthentication credentials ) {
            var request = new ListMatchesAuthenticatedRequest( credentials );

            return await ListMatchesAuthenticatedAsync( request );
        }

        /// <summary>
        /// Lists matches, selecting public or authenticated behavior based on the request type.
        /// </summary>
        /// <param name="requestParameters">ListMatchesAbstractRequest object</param>
        /// <returns>Match list data</returns>
        public async Task<ListMatchesAbstractResponse> ListMatchesAsync( ListMatchesAbstractRequest requestParameters ) {
            if (requestParameters is ListMatchesPublicRequest)
                return await this.ListMatchesPublicAsync( (ListMatchesPublicRequest)requestParameters );
            else if (requestParameters is ListMatchesAuthenticatedRequest)
                return await this.ListMatchesAuthenticatedAsync( (ListMatchesAuthenticatedRequest)requestParameters );
            else
                throw new ArgumentException( $"requestParameters is of unexpected type ${requestParameters.GetType()}." );
        }
        #endregion

        #region Match Participant List
        /// <summary>
        /// Requests a list of Match Participants for a specified match.
        /// </summary>
        /// <param name="requestParameters">GetParticipantListRequest object</param>
        /// <returns>Match Participant List Object</returns>
        public async Task<GetMatchParticipantListPublicResponse> GetMatchParticipantListPublicAsync( GetMatchParticipantListPublicRequest requestParameters ) {
            GetMatchParticipantListPublicResponse response = new GetMatchParticipantListPublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Requests a list of Match Participants for a specified match.
        /// </summary>
        /// <param name="matchid"></param>
        /// <returns>Match Participant List Object</returns>
        public async Task<GetMatchParticipantListPublicResponse> GetMatchParticipantListPublicAsync( MatchID matchid ) {
            var request = new GetMatchParticipantListPublicRequest( matchid );

            return await GetMatchParticipantListPublicAsync( request );
        }

        /// <summary>
        /// Requests a list of Match Participants for a specified match. Limited by Match Particpants with the specified role
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="role"></param>
        /// <returns>Match Participant List Object</returns>
        public async Task<GetMatchParticipantListPublicResponse> GetMatchParticipantListPublicAsync( MatchID matchid, MatchParticipantRole role ) {
            var request = new GetMatchParticipantListPublicRequest( matchid );
            request.Role = role;

            return await GetMatchParticipantListPublicAsync( request );
        }

        /// <summary>
        /// Requests a list of Match Participants for a specified match.
        /// </summary>
        /// <param name="requestParameters">GetParticipantListRequest object</param>
        /// <returns>Match Participant List Object</returns>
        public async Task<GetMatchParticipantListAuthenticatedResponse> GetMatchParticipantListAuthenticatedAsync( GetMatchParticipantListAuthenticatedRequest requestParameters ) {
            GetMatchParticipantListAuthenticatedResponse response = new GetMatchParticipantListAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Requests a list of Match Participants for a specified match.
        /// </summary>
        /// <param name="matchid"></param>
        /// <returns>Match Participant List Object</returns>
        public async Task<GetMatchParticipantListAuthenticatedResponse> GetMatchParticipantListAuthenticatedAsync( MatchID matchid, UserAuthentication credentials ) {
            var request = new GetMatchParticipantListAuthenticatedRequest( matchid, credentials );

            return await GetMatchParticipantListAuthenticatedAsync( request );
        }

        /// <summary>
        /// Requests a list of Match Participants for a specified match. Limited by Match Particpants with the specified role
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="role"></param>
        /// <returns>Match Participant List Object</returns>
        public async Task<GetMatchParticipantListAuthenticatedResponse> GetMatchParticipantListAuthenticatedAsync( MatchID matchid, MatchParticipantRole role, UserAuthentication credentials ) {
            var request = new GetMatchParticipantListAuthenticatedRequest( matchid, credentials );
            request.Role = role;

            return await GetMatchParticipantListAuthenticatedAsync( request );
        }

        public async Task<GetMatchParticipantListAbstractResponse> GetMatchParticipantListAsync( MatchID matchId, UserAuthentication credentials = null ) {
            if (credentials == null)
                return await this.GetMatchParticipantListPublicAsync( matchId );
            else
                return await this.GetMatchParticipantListAuthenticatedAsync( matchId, credentials );
        }

        public async Task<GetMatchParticipantListAbstractResponse> GetMatchParticipantListAsync( GetMatchParticipantListAbstractRequest requestParameters ) {
            if (requestParameters is GetMatchParticipantListPublicRequest)
                return await this.GetMatchParticipantListPublicAsync( (GetMatchParticipantListPublicRequest)(requestParameters) );
            else if (requestParameters is GetMatchParticipantListAuthenticatedRequest)
                return await this.GetMatchParticipantListAuthenticatedAsync( (GetMatchParticipantListAuthenticatedRequest)(requestParameters) );
            else
                //We shouldn't ever get here
                throw new ArgumentException( $"requestParameters is of unexpected type ${requestParameters.GetType()}." );
        }

        #endregion

        #region League API CAlls
        /// <summary>
        /// Get League Detail API
        /// </summary>
        /// <param name="requestParameters">GetMatchRequest object</param>
        public async Task<GetLeaguePublicResponse> GetLeagueDetailPublicAsync( GetLeaguePublicRequest requestParameters ) {

            GetLeaguePublicResponse response = new GetLeaguePublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Get League Detail API
        /// </summary>
        /// <param name="leagueId"></param>
        public async Task<GetLeaguePublicResponse> GetLeagueDetailPublicAsync( MatchID? leagueId ) {
            var request = new GetLeaguePublicRequest( leagueId );

            return await GetLeagueDetailPublicAsync( request ).ConfigureAwait( false );
        }

        /// <summary>
        /// Get League Games API
        /// </summary>
        /// <param name="requestParameters">GetMatchRequest object</param>
        public async Task<GetLeagueGamesPublicResponse> GetLeagueGamesPublicAsync( GetLeagueGamesPublicRequest requestParameters ) {

            GetLeagueGamesPublicResponse response = new GetLeagueGamesPublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Get League Games API
        /// </summary>
        /// <param name="leagueId"></param>
        public async Task<GetLeagueGamesPublicResponse> GetLeagueGamesPublicAsync( MatchID? leagueId ) {
            var request = new GetLeagueGamesPublicRequest( leagueId );

            return await GetLeagueGamesPublicAsync( request ).ConfigureAwait( false );
        }

        /// <summary>
        /// Get League Teams API
        /// </summary>
        /// <param name="requestParameters">GetLeagueTeamsPublicRequest object</param>
        public async Task<GetLeagueTeamsPublicResponse> GetLeagueTeamsPublicAsync( GetLeagueTeamsPublicRequest requestParameters ) {

            GetLeagueTeamsPublicResponse response = new GetLeagueTeamsPublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Get League Teams API
        /// </summary>
        /// <param name="leagueId"></param>
        public async Task<GetLeagueTeamsPublicResponse> GetLeagueTeamsPublicAsync( MatchID? leagueId ) {
            var request = new GetLeagueTeamsPublicRequest( leagueId );

            return await GetLeagueTeamsPublicAsync( request ).ConfigureAwait( false );
        }

        /// <summary>
        /// Get League Team Detail API
        /// </summary>
        /// <param name="requestParameters">GetLeagueTeamsPublicRequest object</param>
        public async Task<GetLeagueTeamDetailPublicResponse> GetLeagueTeamDetailPublicAsync( GetLeagueTeamDetailPublicRequest requestParameters ) {

            GetLeagueTeamDetailPublicResponse response = new GetLeagueTeamDetailPublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Get League Teams API
        /// </summary>
        /// <param name="leagueId"></param>
        /// <param name="teamId"></param>
        public async Task<GetLeagueTeamDetailPublicResponse> GetLeagueTeamDetailPublicAsync( MatchID leagueId, int teamId ) {
            var request = new GetLeagueTeamDetailPublicRequest( leagueId, teamId );

            return await GetLeagueTeamDetailPublicAsync( request ).ConfigureAwait( false );
        }

        public async Task<GetPressReleaseGenerationAuthenticatedResponse> GetPressReleaseGenerationAuthenticatedAsync( GetPressReleaseGenerationAuthenticatedRequest requestParameters ) {
            /*
             * EKA Note January 2026
             * 
             * This method should likely be worked in with ( not sure how yet ) with the Scopos.BabelFish.DataModel.OrionMatch.MatchHtmlReport calss..
             * As this method generates a press release, and that class stores where the pressrelease is at.
             */
            GetPressReleaseGenerationAuthenticatedResponse response = new GetPressReleaseGenerationAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        public async Task<PostSendPressReleaseEmailAuthenticatedResponse> PostSendPressReleaseEmailAsync( PostSendPressReleaseEmailAuthenticatedRequest requestParameters ) {
            PostSendPressReleaseEmailAuthenticatedResponse response = new PostSendPressReleaseEmailAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;

        }

        #endregion

        #region Tournament API Calls

        /// <summary>
        /// Create Tournament API
        /// </summary>
        /// <param name="requestParameters">CreateTournamentAuthenticatedRequest object</param>
        /// <returns>Tournament Object</returns>
        public async Task<CreateTournamentAuthenticatedResponse> CreateTournamentAuthenticatedAsync( CreateTournamentAuthenticatedRequest requestParameters ) {
            CreateTournamentAuthenticatedResponse response = new CreateTournamentAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }



        /// <summary>
        /// Create Tournament API
        /// </summary>
        /// <param name="tournament"></param>
        /// <param name="credentials"></param>
        /// <returns>Tournament Object</returns>
        public async Task<CreateTournamentAuthenticatedResponse> CreateTournamentAuthenticatedAsync( Tournament tournament, UserAuthentication credentials ) {
            var request = new CreateTournamentAuthenticatedRequest( credentials, tournament );

            return await CreateTournamentAuthenticatedAsync( request );
        }

        /// <summary>
        /// Patch Tournament API
        /// </summary>
        /// <param name="requestParameters">PatchTournamentAuthenticatedRequest object</param>
        /// <returns>Tournament Object</returns>
        public async Task<PatchTournamentAuthenticatedResponse> PatchTournamentAuthenticatedAsync( PatchTournamentAuthenticatedRequest requestParameters ) {
            PatchTournamentAuthenticatedResponse response = new PatchTournamentAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Patch Tournament API
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <param name="tournamentName"></param>
        /// <param name="visibility"></param>
        /// <param name="credentials"></param>
        /// <returns>Tournament Object</returns>
        public async Task<PatchTournamentAuthenticatedResponse> PatchTournamentAuthenticatedAsync(
            MatchID tournamentId,
            string? tournamentName,
            VisibilityOption? visibility,
            UserAuthentication credentials ) {
            var request = new PatchTournamentAuthenticatedRequest( credentials, tournamentId, tournamentName, visibility );

            return await PatchTournamentAuthenticatedAsync( request );
        }

        /// <summary>
        /// Delete Tournament API
        /// </summary>
        /// <param name="requestParameters">DeleteTournamentAuthenticatedRequest object</param>
        /// <returns>Delete Tournament Response data</returns>
        public async Task<DeleteTournamentAuthenticatedResponse> DeleteTournamentAuthenticatedAsync( DeleteTournamentAuthenticatedRequest requestParameters ) {
            DeleteTournamentAuthenticatedResponse response = new DeleteTournamentAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Delete Tournament API
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <param name="credentials"></param>
        /// <returns>Delete Tournament Response data</returns>
        public async Task<DeleteTournamentAuthenticatedResponse> DeleteTournamentAuthenticatedAsync( MatchID tournamentId, UserAuthentication credentials ) {
            var request = new DeleteTournamentAuthenticatedRequest( credentials, tournamentId );

            return await DeleteTournamentAuthenticatedAsync( request );
        }

        /// <summary>
        /// Create Merged Result List API
        /// </summary>
        /// <param name="requestParameters">CreateMergedResultListAuthenticatedRequest object</param>
        /// <returns>Merged Result List data</returns>
        public async Task<CreateMergedResultListAuthenticatedResponse> CreateMergedResultListAuthenticatedAsync( CreateMergedResultListAuthenticatedRequest requestParameters ) {
            CreateMergedResultListAuthenticatedResponse response = new CreateMergedResultListAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Create Merged Result List API
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <param name="mergedResultList"></param>
        /// <param name="credentials"></param>
        /// <returns>Merged Result List data</returns>
        public async Task<CreateMergedResultListAuthenticatedResponse> CreateMergedResultListAuthenticatedAsync( MatchID tournamentId, MergedResultList mergedResultList, UserAuthentication credentials ) {
            var request = new CreateMergedResultListAuthenticatedRequest( credentials, tournamentId, mergedResultList );

            return await CreateMergedResultListAuthenticatedAsync( request );
        }

        /// <summary>
        /// Delete Merged Result List API
        /// </summary>
        /// <param name="requestParameters">DeleteMergedResultListAuthenticatedRequest object</param>
        /// <returns>Delete Merged Result List response data</returns>
        public async Task<DeleteMergedResultListAuthenticatedResponse> DeleteMergedResultListAuthenticatedAsync( DeleteMergedResultListAuthenticatedRequest requestParameters ) {
            DeleteMergedResultListAuthenticatedResponse response = new DeleteMergedResultListAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Delete Merged Result List API
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <param name="mergedId"></param>
        /// <param name="credentials"></param>
        /// <returns>Delete Merged Result List response data</returns>
        public async Task<DeleteMergedResultListAuthenticatedResponse> DeleteMergedResultListAuthenticatedAsync( MatchID tournamentId, string mergedId, UserAuthentication credentials ) {
            var request = new DeleteMergedResultListAuthenticatedRequest( credentials, tournamentId, mergedId );

            return await DeleteMergedResultListAuthenticatedAsync( request );
        }

        /// <summary>
        /// Add Merged Result List Member API
        /// </summary>
        /// <param name="requestParameters">AddMergedResultListMemberAuthenticatedRequest object</param>
        /// <returns>Result List Member data</returns>
        public async Task<AddMergedResultListMemberAuthenticatedResponse> AddMergedResultListMemberAuthenticatedAsync( AddMergedResultListMemberAuthenticatedRequest requestParameters ) {
            AddMergedResultListMemberAuthenticatedResponse response = new AddMergedResultListMemberAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Add Merged Result List Member API
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <param name="mergedId"></param>
        /// <param name="resultListMember"></param>
        /// <param name="credentials"></param>
        /// <returns>Result List Member data</returns>
        public async Task<AddMergedResultListMemberAuthenticatedResponse> AddMergedResultListMemberAuthenticatedAsync( MatchID tournamentId, string mergedId, ResultListMember resultListMember, UserAuthentication credentials ) {
            var request = new AddMergedResultListMemberAuthenticatedRequest( credentials, tournamentId, mergedId, resultListMember );

            return await AddMergedResultListMemberAuthenticatedAsync( request );
        }

        /// <summary>
        /// Remove Merged Result List Member API
        /// </summary>
        /// <param name="requestParameters">RemoveMergedResultListMemberAuthenticatedRequest object</param>
        /// <returns>Result List Member data</returns>
        public async Task<RemoveMergedResultListMemberAuthenticatedResponse> RemoveMergedResultListMemberAuthenticatedAsync( RemoveMergedResultListMemberAuthenticatedRequest requestParameters ) {
            RemoveMergedResultListMemberAuthenticatedResponse response = new RemoveMergedResultListMemberAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Remove Merged Result List Member API
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <param name="mergedId"></param>
        /// <param name="resultListMember"></param>
        /// <param name="credentials"></param>
        /// <returns>Result List Member data</returns>
        public async Task<RemoveMergedResultListMemberAuthenticatedResponse> RemoveMergedResultListMemberAuthenticatedAsync( MatchID tournamentId, string mergedId, ResultListMember resultListMember, UserAuthentication credentials ) {
            var request = new RemoveMergedResultListMemberAuthenticatedRequest( credentials, tournamentId, mergedId, resultListMember );

            return await RemoveMergedResultListMemberAuthenticatedAsync( request );
        }

        /// <summary>
        /// Add Tournament Member API
        /// </summary>
        /// <param name="requestParameters">AddTournamentMemberAuthenticatedRequest object</param>
        /// <returns>Tournament Member data</returns>
        public async Task<AddTournamentMemberAuthenticatedResponse> AddTournamentMemberAuthenticatedAsync( AddTournamentMemberAuthenticatedRequest requestParameters ) {
            AddTournamentMemberAuthenticatedResponse response = new AddTournamentMemberAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Add Tournament Member API
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <param name="matchId"></param>
        /// <param name="credentials"></param>
        /// <returns>Tournament Member data</returns>
        public async Task<AddTournamentMemberAuthenticatedResponse> AddTournamentMemberAuthenticatedAsync( MatchID tournamentId, MatchID matchId, UserAuthentication credentials ) {
            var request = new AddTournamentMemberAuthenticatedRequest( credentials, tournamentId, matchId );

            return await AddTournamentMemberAuthenticatedAsync( request );
        }

        /// <summary>
        /// Patch Tournament Member API
        /// </summary>
        /// <param name="requestParameters">PatchTournamentMemberAuthenticatedRequest object</param>
        /// <returns>Tournament Member data</returns>
        public async Task<PatchTournamentMemberAuthenticatedResponse> PatchTournamentMemberAuthenticatedAsync( PatchTournamentMemberAuthenticatedRequest requestParameters ) {
            PatchTournamentMemberAuthenticatedResponse response = new PatchTournamentMemberAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }


        /// <summary>
        /// Delete Tournament Member API
        /// </summary>
        /// <param name="requestParameters">DeleteTournamentMemberAuthenticatedRequest object</param>
        /// <returns>Tournament Member data</returns>
        public async Task<DeleteTournamentMemberAuthenticatedResponse> DeleteTournamentMemberAuthenticatedAsync( DeleteTournamentMemberAuthenticatedRequest requestParameters ) {
            DeleteTournamentMemberAuthenticatedResponse response = new DeleteTournamentMemberAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Delete Tournament Member API
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <param name="matchId"></param>
        /// <param name="credentials"></param>
        /// <returns>Tournament Member data</returns>
        public async Task<DeleteTournamentMemberAuthenticatedResponse> DeleteTournamentMemberAuthenticatedAsync( MatchID tournamentId, MatchID matchId, UserAuthentication credentials ) {
            var request = new DeleteTournamentMemberAuthenticatedRequest( credentials, tournamentId, matchId );

            return await DeleteTournamentMemberAuthenticatedAsync( request );
        }

        /// <summary>
        /// Tournament Search API
        /// </summary>
        /// <param name="requestParameters">TournamentSearchPublicRequest object</param>
        /// <returns>Tournament search list data</returns>
        public async Task<TournamentSearchPublicResponse> TournamentSearchPublicAsync( TournamentSearchPublicRequest requestParameters ) {
            TournamentSearchPublicResponse response = new TournamentSearchPublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Tournament Search API
        /// </summary>
        /// <param name="requestParameters">TournamentSearchAuthenticatedRequest object</param>
        /// <returns>Tournament search list data</returns>
        public async Task<TournamentSearchAuthenticatedResponse> TournamentSearchAuthenticatedAsync( TournamentSearchAuthenticatedRequest requestParameters ) {
            TournamentSearchAuthenticatedResponse response = new TournamentSearchAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Tournament Search API
        /// </summary>
        /// <param name="requestParameters">TournamentSearchAbstractRequest object</param>
        /// <returns>Tournament search list data</returns>
        public async Task<TournamentSearchAbstractResponse> TournamentSearchAsync( TournamentSearchAbstractRequest requestParameters ) {
            if (requestParameters is TournamentSearchPublicRequest)
                return await this.TournamentSearchPublicAsync( (TournamentSearchPublicRequest)requestParameters );
            else if (requestParameters is TournamentSearchAuthenticatedRequest)
                return await this.TournamentSearchAuthenticatedAsync( (TournamentSearchAuthenticatedRequest)requestParameters );
            else
                throw new ArgumentException( $"requestParameters is of unexpected type ${requestParameters.GetType()}." );
        }

        /// <summary>
        /// Get Tournament Detail API
        /// </summary>
        /// <param name="requestParameters">GetMatchRequest object</param>
        /// <returns>Tournament Object</returns>
        public async Task<GetTournamentPublicResponse> GetTournamentPublicAsync( GetTournamentPublicRequest requestParameters ) {

            GetTournamentPublicResponse response = new GetTournamentPublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Get Tournament Detail API
        /// </summary>
        /// <param name="matchid"></param>
        /// <returns>Tournament Object</returns>
        public async Task<GetTournamentPublicResponse> GetTournamentPublicAsync( MatchID tournamentId ) {
            var request = new GetTournamentPublicRequest( tournamentId );

            return await GetTournamentPublicAsync( request );
        }

        /// <summary>
        /// Get Tournament Detail API
        /// </summary>
        /// <param name="requestParameters">GetTournamentRequest object</param>
        /// <returns>Tournament Object</returns>
        public async Task<GetTournamentAuthenticatedResponse> GetTournamentAuthenticatedAsync( GetTournamentAuthenticatedRequest requestParameters ) {

            GetTournamentAuthenticatedResponse response = new GetTournamentAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Get Tournament Detail API
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="withAuthentication">default false</param>
        /// <returns>Tournament Object</returns>
        public async Task<GetTournamentAuthenticatedResponse> GetTournamentAuthenticatedAsync( MatchID tournamentId, UserAuthentication credentials ) {
            var request = new GetTournamentAuthenticatedRequest( tournamentId, credentials );

            return await GetTournamentAuthenticatedAsync( request );
        }

        /// <summary>
        /// Function that abstracts the Public vs Authenticated calls. If credentials is null, then a PublicAPI call is made.
        /// If credentials if not null, then an Authenticated API call is made.
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <param name="credentials"></param>
        /// <returns></returns>
        public async Task<GetTournamentAbstractResponse> GetTournamentAsync( MatchID tournamentId, UserAuthentication? credentials = null ) {
            if (credentials == null) {
                return await GetTournamentPublicAsync( tournamentId );
            } else {
                return await GetTournamentAuthenticatedAsync( tournamentId, credentials );
            }
        }

        public async Task<GetTournamentAbstractResponse> GetTournamentAsync( GetTournamentAbstractRequest requestParameters ) {
            if (requestParameters is GetTournamentPublicRequest)
                return await this.GetTournamentPublicAsync( (GetTournamentPublicRequest)requestParameters );
            else if (requestParameters is GetTournamentAuthenticatedRequest)
                return await this.GetTournamentAuthenticatedAsync( (GetTournamentAuthenticatedRequest)requestParameters );
            else
                //We shouldn't ever get here
                throw new ArgumentException( $"requestParameters is of unexpected type ${requestParameters.GetType()}." );
        }
        #endregion

        #region Post a Shot API Calls

        public async Task<PostShotDataAuthenticatedResponse> PostShotDataAuthenticatedAsync( PostShotDataAuthenticatedRequest requestParameters ) {
            PostShotDataAuthenticatedResponse response = new PostShotDataAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        #endregion
    }
}
