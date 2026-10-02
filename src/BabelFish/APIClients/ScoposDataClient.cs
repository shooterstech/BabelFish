using Scopos.BabelFish.DataModel.ScoposData;
using Scopos.BabelFish.Requests.ScoposData;
using Scopos.BabelFish.Responses.ScoposData;
using Version = Scopos.BabelFish.DataModel.Common.Version;

namespace Scopos.BabelFish.APIClients {
    public class ScoposDataClient : APIClient<ScoposDataClient> {

        //
        private static Cache<ApplicationName, Version> _productionVersionCache = new Cache<ApplicationName, Version>( TimeSpan.FromHours( 1 ) );

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="apiKey"></param>
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        public ScoposDataClient() : base() {

            //ScoposDataClient does not support file system cache
            LocalStoreDirectory = null;
            IgnoreFileSystemCache = true;

            //We do want in memory cache
            IgnoreInMemoryCache = false;
        }

        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        public ScoposDataClient( APIStage apiStage ) : base( apiStage ) {

            //ScoposDataClient does not support file system cache
            LocalStoreDirectory = null;
            IgnoreFileSystemCache = true;

            //We do want in memory cache
            IgnoreInMemoryCache = false;
        }


        /// <summary>
        /// GetRelease API for multiple services
        /// </summary>
        /// <param name="requestParameters">GetReleasePublicRequest object</param>
        /// <returns>GetReleasePublicResponse object</returns>
        public async Task<GetReleasePublicResponse> GetReleasePublicAsync( GetReleasePublicRequest requestParameters ) {

            GetReleasePublicResponse response = new GetReleasePublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// GetRelease API for one service
        /// </summary>
        /// <param name="service">VersionService enum</param>
        /// <param name="level">VersionLevel enum</param>
        /// <returns>List<VersionInfo> object</returns>
        public async Task<GetReleasePublicResponse> GetReleasePublicAsync( ReleasePhase releasePhase ) {
            GetReleasePublicRequest requestParameters = new GetReleasePublicRequest() {
                ReleasePhase = releasePhase
            };

            return await GetReleasePublicAsync( requestParameters ).ConfigureAwait( false );
        }

        /// <summary>
        /// GetRelease API for one service
        /// </summary>
        /// <param name="service">VersionService enum</param>
        /// <param name="level">VersionLevel enum</param>
        /// <returns>List<VersionInfo> object</returns>
        public async Task<GetReleasePublicResponse> GetReleasePublicAsync( ReleasePhase releasePhase, string thingName, DataModel.Common.Version thingVersion ) {
            GetReleasePublicRequest requestParameters = new GetReleasePublicRequest() {
                ReleasePhase = releasePhase,
                ThingName = thingName,
                ThingVersion = thingVersion
            };

            return await GetReleasePublicAsync( requestParameters ).ConfigureAwait( false );
        }

        /// <summary>
        /// GetReleaseAPI for if you want a specific application and are not incl. EULAs
        /// </summary>
        /// <param name="releasePhase"></param>
        /// <param name="ApplicationItems"></param>
        /// <param name="thingName"></param>
        /// <param name="thingVersion"></param>
        /// <returns></returns>
        public async Task<GetReleasePublicResponse> GetReleasePublicAsync( ReleasePhase releasePhase, List<string> ApplicationItems, string thingName, DataModel.Common.Version thingVersion ) {
            GetReleasePublicRequest requestParameters = new GetReleasePublicRequest() {
                ReleasePhase = releasePhase,
                ApplicationItems = ApplicationItems,
                ThingName = thingName,
                ThingVersion = thingVersion
            };

            return await GetReleasePublicAsync( requestParameters ).ConfigureAwait( false );
        }

        /// <summary>
        /// GetRelease API call for Orion and Athena by default with EULA accept
        /// </summary>
        /// <param name="releasePhase"></param>
        /// <param name="thingName"></param>
        /// <param name="thingVersion"></param>
        /// <param name="OrionEulaAccepted"></param>
        /// <param name="AthenaEulaAccepted"></param>
        /// <param name="OwnerID"></param>
        /// <returns></returns>
        public async Task<GetReleasePublicResponse> GetReleasePublicAsync( ReleasePhase releasePhase, string thingName, DataModel.Common.Version thingVersion, bool OrionEulaAccepted, bool AthenaEulaAccepted, string OwnerID ) {
            GetReleasePublicRequest requestParameters = new GetReleasePublicRequest() {
                ReleasePhase = releasePhase,
                ThingName = thingName,
                ThingVersion = thingVersion,
                OrionEulaAccepted = OrionEulaAccepted,
                AthenaEulaAccepted = AthenaEulaAccepted,
                OwnerID = OwnerID
            };

            return await GetReleasePublicAsync( requestParameters ).ConfigureAwait( false );
        }

        /// <summary>
        /// GetRelease API call for specific Application with EULA accept
        /// </summary>
        /// <param name="releasePhase"></param>
        /// <param name="ApplicationItems"></param>
        /// <param name="thingName"></param>
        /// <param name="thingVersion"></param>
        /// <param name="OrionEulaAccepted"></param>
        /// <param name="AthenaEulaAccepted"></param>
        /// <param name="OwnerID"></param>
        /// <returns></returns>
        public async Task<GetReleasePublicResponse> GetReleasePublicAsync( ReleasePhase releasePhase, List<string> ApplicationItems, string thingName, DataModel.Common.Version thingVersion, bool OrionEulaAccepted, bool AthenaEulaAccepted, string OwnerID ) {
            GetReleasePublicRequest requestParameters = new GetReleasePublicRequest() {
                ReleasePhase = releasePhase,
                ApplicationItems = ApplicationItems,
                ThingName = thingName,
                ThingVersion = thingVersion,
                OrionEulaAccepted = OrionEulaAccepted,
                AthenaEulaAccepted = AthenaEulaAccepted,
                OwnerID = OwnerID
            };

            return await GetReleasePublicAsync( requestParameters ).ConfigureAwait( false );
        }

        /// <summary>
        /// Helper method to get the current production version of an application. Common application names are Orion and Athena.
        /// <para>Version value is cached for 1 hour.</para>
        /// <para>If the application version cannot be found, this method will log an error and return a default version of 1.0.0 to avoid blocking any functionality that depends on this.</para>
        /// </summary>
        /// <param name="applicationName"></param>
        /// <returns></returns>
        public async Task<Version> GetProductionVersionAsync( ApplicationName applicationName ) {

            if (_productionVersionCache.TryGetValue( applicationName, out Version? cachedVersion )) {
                return cachedVersion;
            }

            try {
                ScoposDataClient client = new ScoposDataClient();
                var response = await client.GetReleasePublicAsync( ReleasePhase.PRODUCTION );
                if (response.HasOkStatusCode) {
                    var applicationReleaseList = response.ApplicationRelease;
                    // GEtReleasePublicAsync returns a list of all applications in the specified release phase, so we will loop through the list and add all applications and their versions to the cache. This way, if we need to get the production version for another application in the future, it will already be in the cache and we can avoid making another API call.
                    foreach (var app in applicationReleaseList.Items) {
                        if (app.ReleasePhase == ReleasePhase.PRODUCTION) {
                            _productionVersionCache.AddValue( app.Application, app.Version );
                        }
                    }
                } else {
                    _logger.Error( $"Received non-success status code {response.OverallStatusCode} when calling GetReleasePublicAsync to get production version for {applicationName}" );
                }

            } catch (Exception ex) {
                _logger.Error( $"Error while calling GetReleasePublicAsync to get production version for {applicationName}: {ex.Message}" );
            }

            if (_productionVersionCache.TryGetValue( applicationName, out cachedVersion )) {
                return cachedVersion;
            }

            //If we get here, it means we did not find the application in the production release list. If this happens, there is likely something wrong with the API response or the application name, so we will log an error and return a default version of 1.0.0 to avoid blocking any functionality that depends on this.
            _logger.Error( $"Did not find {applicationName} in production release list returned by GetReleasePublicAsync" );
            return Version.Parse( "1.0.0" );
        }

        /// <summary>
        /// Calls the https://api.orionscoringsystem.com/coffee api request.
        /// </summary>
        /// <param name="requestParameters"></param>
        /// <returns></returns>
        public async Task<GetCupsOfCoffeePublicResponse> GetCuposOfCoffeePublicAsync( GetCupsOfCoffeePublicRequest requestParameters ) {

            GetCupsOfCoffeePublicResponse response = new GetCupsOfCoffeePublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Calls the https://api.orionscoringsystem.com/coffee api request
        /// </summary>
        /// <returns></returns>
        public async Task<GetCupsOfCoffeePublicResponse> GetCuposOfCoffeePublicAsync() {

            GetCupsOfCoffeePublicRequest requestParameters = new GetCupsOfCoffeePublicRequest();

            GetCupsOfCoffeePublicResponse response = new GetCupsOfCoffeePublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Calls the internal image moderation API request.
        /// </summary>
        /// <param name="requestParameters"></param>
        /// <returns></returns>
        public async Task<PatchModerateImageAuthenticatedResponse> PatchModerateImageAuthenticatedAsync( PatchModerateImageAuthenticatedRequest requestParameters ) {

            PatchModerateImageAuthenticatedResponse response = new PatchModerateImageAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

    }
}
