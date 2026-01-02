using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI
{
    public class GetTournamentPublicResponse : GetTournamentAbstractResponse
    {

        public GetTournamentPublicResponse( GetTournamentPublicRequest request ) : base() {
            this.Request = Request;
        }
	}
}