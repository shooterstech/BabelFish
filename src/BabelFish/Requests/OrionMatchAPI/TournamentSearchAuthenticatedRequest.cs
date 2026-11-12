using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class TournamentSearchAuthenticatedRequest : TournamentSearchAbstractRequest {

        /// <summary>
        /// Authenticated constructor.
        /// </summary>
        public TournamentSearchAuthenticatedRequest( UserAuthentication credentials ) : base( "TournamentSearch", credentials ) {
            this.RequiresCredentials = true;
        }

        /// <inheritdoc />
        public override Request Copy() {
            var newRequest = new TournamentSearchAuthenticatedRequest( Credentials );
            newRequest.Name = Name;
            newRequest.OwnerId = OwnerId;
            newRequest.LicenseNumber = LicenseNumber;
            newRequest.Visibility = Visibility;
            newRequest.MemberPolicy = MemberPolicy;
            newRequest.MemberHasOwnerId = MemberHasOwnerId;
            newRequest.MemberHasMatchId = MemberHasMatchId;
            newRequest.MemberHasApprovalStatus = MemberHasApprovalStatus;
            newRequest.SearchPreset = SearchPreset;
            newRequest.ShowOnSearch = ShowOnSearch;
            newRequest.Token = Token;
            newRequest.Limit = Limit;

            return newRequest;
        }
    }
}
