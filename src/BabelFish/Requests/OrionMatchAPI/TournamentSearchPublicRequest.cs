namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class TournamentSearchPublicRequest : TournamentSearchAbstractRequest {

        public TournamentSearchPublicRequest() : base( "TournamentSearch" ) {
        }

        /// <inheritdoc />
        public override Request Copy() {
            var newRequest = new TournamentSearchPublicRequest();
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
