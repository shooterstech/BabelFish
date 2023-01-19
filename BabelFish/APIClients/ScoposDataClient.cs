using Scopos.BabelFish.Requests.ScoposData;
using Scopos.BabelFish.Responses.ScoposData;
using Scopos.BabelFish.DataModel.ScoposData;

namespace Scopos.BabelFish.APIClients {
    public class ScoposDataClient : APIClient {

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="apiKey"></param>
        public ScoposDataClient( string apiKey ) : base( apiKey ) { }

        public ScoposDataClient( string apiKey, APIStage apiStage ) : base( apiKey, apiStage ) { }

        /// <summary>
        /// GetVersion API for multiple services
        /// </summary>
        /// <param name="requestParameters">GetVersionRequest object</param>
        /// <returns>List<VersionInfo> object</returns>
        public async Task<GetVersionResponse> GetVersionAsync( GetVersionRequest requestParameters ) {

            GetVersionResponse response = new GetVersionResponse( requestParameters );

            await this.CallAPI( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// GetVersion API for one service
        /// </summary>
        /// <param name="service">VersionService enum</param>
        /// <param name="level">VersionLevel enum</param>
        /// <returns>List<VersionInfo> object</returns>
        public async Task<GetVersionResponse> GetVersionAsync( VersionService service, VersionLevel level ) {
            GetVersionRequest requestParameters = new GetVersionRequest() {
                Services = new List<VersionService>() { service },
                Level = level
            };

            return await GetVersionAsync( requestParameters ).ConfigureAwait( false );
        }

        /// <summary>
        /// Calls the https://api.orionscoringsystem.com/coffee api request.
        /// </summary>
        /// <param name="requestParameters"></param>
        /// <returns></returns>
        public async Task<GetCupsOfCoffeeResponse> GetCuposOfCoffeeAsync( GetCupsOfCoffeeRequest requestParameters ) {

            GetCupsOfCoffeeResponse response = new GetCupsOfCoffeeResponse( requestParameters );

            await this.CallAPI( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Calls the https://api.orionscoringsystem.com/coffee api request
        /// </summary>
        /// <returns></returns>
        public async Task<GetCupsOfCoffeeResponse> GetCuposOfCoffeeAsync(  ) {

            GetCupsOfCoffeeRequest requestParameters = new GetCupsOfCoffeeRequest();

            GetCupsOfCoffeeResponse response = new GetCupsOfCoffeeResponse( requestParameters );

            await this.CallAPI( requestParameters, response );

            return response;
        }

    }
}
