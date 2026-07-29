using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    [Obsolete( "Use one of the ListMatch() methods instead. Deprecated July 2026 with the BabelFish 2.0 release. This method will be removed in a future release." )]
    public class MatchSearchPublicResponse : MatchSearchAbstractResponse {

        public MatchSearchPublicResponse( MatchSearchPublicRequest request ) : base() {
            Request = request;
        }
    }
}
