using Scopos.BabelFish.DataModel.ScoposData;
using Scopos.BabelFish.Requests.ScoposData;

namespace Scopos.BabelFish.Responses.ScoposData {
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