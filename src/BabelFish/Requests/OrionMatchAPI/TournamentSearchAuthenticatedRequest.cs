using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class TournamentSearchAuthenticatedRequest : Request, ITokenRequest {

        /// <summary>
        /// Authenticated constructor.
        /// </summary>
        public TournamentSearchAuthenticatedRequest( UserAuthentication credentials ) : base( "TournamentSearch", credentials ) {
            this.RequiresCredentials = true;
            this.SubDomain = APISubDomain.AUTHAPI;
        }

        /// <summary>
        /// Optional filter by tournament name. Matches partial names.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Optional filter by owner id (e.g. OrionAcct000001).
        /// </summary>
        public string OwnerId { get; set; } = string.Empty;

        /// <summary>
        /// Optional filter by organizer license number.
        /// </summary>
        public int? LicenseNumber { get; set; } = null;

        /// <summary>
        /// Optional tournament visibility filter.
        /// </summary>
        public VisibilityOption? Visibility { get; set; } = VisibilityOption.PUBLIC;

        /// <summary>
        /// Optional tournament member policy filter.
        /// </summary>
        public MemberPolicyOption? MemberPolicy { get; set; } = null;

        /// <summary>
        /// Optional filter for tournaments containing a member match owned by this owner id.
        /// </summary>
        public string MemberHasOwnerId { get; set; } = string.Empty;

        /// <summary>
        /// Optional filter for tournaments containing this match id as a member.
        /// </summary>
        public MatchID? MemberHasMatchId { get; set; } = null;

        /// <summary>
        /// Optional filter for tournaments with members in this approval status.
        /// </summary>
        public ApprovalStatus? MemberHasApprovalStatus { get; set; } = null;

        /// <summary>
        /// Optional preset filter, e.g. incoming-invites or outgoing-requests.
        /// </summary>
        public TournamentSearchPresetOption? SearchPreset { get; set; } = null;

        /// <summary>
        /// Optional filter for whether tournament is shown in search results.
        /// </summary>
        public bool? ShowOnSearch { get; set; } = null;

        /// <inheritdoc />
        public string Token { get; set; } = string.Empty;

        /// <inheritdoc />
        public int Limit { get; set; } = 10;

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

        /// <inheritdoc />
        public override string RelativePath {
            get { return "/tournament"; }
        }

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {
                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();

                if (!string.IsNullOrWhiteSpace( Name )) {
                    parameterList.Add( "name", new List<string>() { Name } );
                }

                if (!string.IsNullOrWhiteSpace( OwnerId )) {
                    parameterList.Add( "owner-id", new List<string>() { OwnerId } );
                }

                if (LicenseNumber.HasValue) {
                    parameterList.Add( "license-number", new List<string>() { LicenseNumber.Value.ToString() } );
                }

                if (Visibility.HasValue) {
                    parameterList.Add( "visibility", new List<string>() { EnumHelper.MemberValue( Visibility.Value ) } );
                }

                if (MemberPolicy.HasValue) {
                    parameterList.Add( "member-policy", new List<string>() { EnumHelper.MemberValue( MemberPolicy.Value ) } );
                }

                if (!string.IsNullOrWhiteSpace( MemberHasOwnerId )) {
                    parameterList.Add( "member-has-owner-id", new List<string>() { MemberHasOwnerId } );
                }

                if (MemberHasMatchId != null) {
                    parameterList.Add( "member-has-match-id", new List<string>() { MemberHasMatchId.ToString() } );
                }

                if (MemberHasApprovalStatus.HasValue) {
                    parameterList.Add( "member-has-approval-status", new List<string>() { EnumHelper.MemberValue( MemberHasApprovalStatus.Value ) } );
                }

                if (SearchPreset.HasValue) {
                    parameterList.Add( "search-preset", new List<string>() { EnumHelper.MemberValue( SearchPreset.Value ) } );
                }

                if (ShowOnSearch.HasValue) {
                    parameterList.Add( "show-on-search", new List<string>() { ShowOnSearch.Value.ToString() } );
                }

                if (Limit > 0) {
                    parameterList.Add( "limit", new List<string>() { Limit.ToString() } );
                }

                if (!string.IsNullOrWhiteSpace( Token )) {
                    parameterList.Add( "token", new List<string>() { Token } );
                }

                return parameterList;
            }
        }
    }
}
