using Scopos.BabelFish.Requests.ScoposData;
using Scopos.BabelFish.Responses.ScoposData;
using Scopos.BabelFish.DataModel.ScoposData;

namespace Scopos.BabelFish.APIClients {
    public class ScoposDataClient : APIClient {

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="apiKey"></param>
        public ScoposDataClient( string apiKey ) : base( apiKey ) {

            //ScoposDataClient does not support file system cache
            LocalStoreDirectory = null;
            IgnoreFileSystemCache = true;
        }

        public ScoposDataClient( string apiKey, APIStage apiStage ) : base( apiKey, apiStage ) {

            //ScoposDataClient does not support file system cache
            LocalStoreDirectory = null;
            IgnoreFileSystemCache = true;
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
        public async Task<GetVersionPublicResponse> GetVersionPublicAsync( VersionService service, VersionLevel level ) {
            GetVersionPublicRequest requestParameters = new GetVersionPublicRequest() {
                Services = new List<VersionService>() { service },
                Level = level
            };

            return await GetVersionPublicAsync( requestParameters ).ConfigureAwait( false );
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
