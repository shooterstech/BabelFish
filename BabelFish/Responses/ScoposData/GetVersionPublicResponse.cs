using Scopos.BabelFish.DataModel.ScoposData;
using Scopos.BabelFish.Requests.ScoposData;

namespace Scopos.BabelFish.Responses.ScoposData {

    /// <summary>
    /// The GetVersion API call is deprecated. Use GetRelease instead.
    /// </summary>
    [Obsolete( "GetVersion is deprecated. Use GetRelease instead." )]
    public class GetVersionPublicResponse : Response<VersionsList> {
        public GetVersionPublicResponse( GetVersionPublicRequest request ) {
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