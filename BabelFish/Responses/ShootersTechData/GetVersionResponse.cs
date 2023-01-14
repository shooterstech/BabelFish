using Scopos.BabelFish.DataModel.ShootersTechData;
using Scopos.BabelFish.Requests.ShootersTechData;

namespace Scopos.BabelFish.Responses.ShootersTechData {
    public class GetVersionResponse : Response<VersionsList> {
        public GetVersionResponse( GetVersionRequest request ) {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public List<VersionInfo> VersionList {
            get { return Value.Versions; }
        }
    }
}