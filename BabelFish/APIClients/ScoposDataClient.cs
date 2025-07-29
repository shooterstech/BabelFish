using Scopos.BabelFish.Requests.ScoposData;
using Scopos.BabelFish.Responses.ScoposData;
using Scopos.BabelFish.DataModel.ScoposData;

namespace Scopos.BabelFish.APIClients {
    public class ScoposDataClient : APIClient<ScoposDataClient> {

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
        /// GetVersion API for multiple services
        /// </summary>
        /// <param name="requestParameters">GetVersionRequest object</param>
        /// <returns>List<VersionInfo> object</returns>
        public async Task<GetVersionPublicResponse> GetVersionPublicAsync( GetVersionPublicRequest requestParameters ) {

            GetVersionPublicResponse response = new GetVersionPublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// GetVersion API for one service
        /// </summary>
        /// <param name="service">VersionService enum</param>
        /// <param name="level">VersionLevel enum</param>
        /// <returns>List<VersionInfo> object</returns>
        public async Task<GetVersionPublicResponse> GetVersionPublicAsync(ApplicationName service, ReleasePhase level ) {
            GetVersionPublicRequest requestParameters = new GetVersionPublicRequest() {
                Services = new List<ApplicationName>() { service },
                Level = level
            };

            return await GetVersionPublicAsync( requestParameters ).ConfigureAwait( false );
        }


        /// <summary>
        /// GetVersion API for multiple services
        /// </summary>
        /// <param name="requestParameters">GetVersionRequest object</param>
        /// <returns>List<VersionInfo> object</returns>
        public async Task<GetReleasePublicResponse> GetReleasePublicAsync(GetReleasePublicRequest requestParameters)
        {

            GetReleasePublicResponse response = new GetReleasePublicResponse(requestParameters);

            await this.CallAPIAsync(requestParameters, response).ConfigureAwait(false);

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
        public async Task<GetReleasePublicResponse> GetReleasePublicAsync( ReleasePhase releasePhase, string thingName, DataModel.Common.Version thingVersion )
        {
            GetReleasePublicRequest requestParameters = new GetReleasePublicRequest()
            {
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
        public async Task<GetReleasePublicResponse> GetReleasePublicAsync(ReleasePhase releasePhase, List<string> ApplicationItems, string thingName, DataModel.Common.Version thingVersion)
        {
            GetReleasePublicRequest requestParameters = new GetReleasePublicRequest()
            {
                ReleasePhase = releasePhase,
                ApplicationItems = ApplicationItems,
                ThingName = thingName,
                ThingVersion = thingVersion
            };

            return await GetReleasePublicAsync(requestParameters).ConfigureAwait(false);
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
        public async Task<GetReleasePublicResponse> GetReleasePublicAsync(ReleasePhase releasePhase,  string thingName, DataModel.Common.Version thingVersion, bool OrionEulaAccepted, bool AthenaEulaAccepted, string OwnerID)
        {
            GetReleasePublicRequest requestParameters = new GetReleasePublicRequest()
            {
                ReleasePhase = releasePhase,
                ThingName = thingName,
                ThingVersion = thingVersion,
                OrionEulaAccepted = OrionEulaAccepted,
                AthenaEulaAccepted = AthenaEulaAccepted,
                OwnerID = OwnerID
            };

            return await GetReleasePublicAsync(requestParameters).ConfigureAwait(false);
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
        public async Task<GetReleasePublicResponse> GetReleasePublicAsync(ReleasePhase releasePhase, List<string> ApplicationItems, string thingName, DataModel.Common.Version thingVersion, bool OrionEulaAccepted, bool AthenaEulaAccepted, string OwnerID)
        {
            GetReleasePublicRequest requestParameters = new GetReleasePublicRequest()
            {
                ReleasePhase = releasePhase,
                ApplicationItems = ApplicationItems,
                ThingName = thingName,
                ThingVersion = thingVersion,
                OrionEulaAccepted = OrionEulaAccepted,
                AthenaEulaAccepted = AthenaEulaAccepted,
                OwnerID = OwnerID
            };

            return await GetReleasePublicAsync(requestParameters).ConfigureAwait(false);
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
        public async Task<GetCupsOfCoffeePublicResponse> GetCuposOfCoffeePublicAsync(  ) {

            GetCupsOfCoffeePublicRequest requestParameters = new GetCupsOfCoffeePublicRequest();

            GetCupsOfCoffeePublicResponse response = new GetCupsOfCoffeePublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

    }
}
