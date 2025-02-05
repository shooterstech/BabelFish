using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.ClubsAPI {
    public class DeleteCoachAssignmentAuthenticatedRequest : CoachAssignmentCRUDBaseRequest {
        public DeleteCoachAssignmentAuthenticatedRequest( UserAuthentication credentials ) : base( "CoachAssignmentD", credentials ) {
            HttpMethod = HttpMethod.Delete;

        }
    }
}

