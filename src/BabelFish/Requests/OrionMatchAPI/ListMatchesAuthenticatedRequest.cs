using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class ListMatchesAuthenticatedRequest : Request, ITokenRequest {

        public ListMatchesAuthenticatedRequest( UserAuthentication credentials ) : base( "ListMatches", credentials ) {
        }

        /// <summary>
        /// Optional filter to only return matches owned by the specified owner id.
        /// </summary>
        public string OwnerId { get; set; } = string.Empty;

        /// <summary>
        /// Optional filter to only return matches with the specified visibility.
        /// </summary>
        public VisibilityOption? Visibility { get; set; } = null;

        /// <summary>
        /// Optional filter for whether the match is shown in search.
        /// </summary>
        public bool? ShowOnSearch { get; set; } = null;

        /// <summary>
        /// Optional filter to only return matches starting on or after this date.
        /// </summary>
        public DateTime? StartDate { get; set; } = null;

        /// <summary>
        /// Optional filter to only return matches ending on or before this date.
        /// </summary>
        public DateTime? EndDate { get; set; } = null;

        /// <summary>
        /// Optional filter for parent match member policy.
        /// </summary>
        public MemberPolicyOption? MemberPolicy { get; set; } = null;

        /// <summary>
        /// Optional filter to only return a specific parent match and/or children of that parent match.
        /// </summary>
        public MatchID? ParentMatchId { get; set; } = null;

        /// <summary>
        /// Optional filter for whether a child match has been downloaded.
        /// </summary>
        public bool? HasDownloaded { get; set; } = null;

        /// <summary>
        /// Optional filter for child match approval status.
        /// </summary>
        public ApprovalStatus? ApprovalStatus { get; set; } = null;

        /// <summary>
        /// Optional filter for which table to query. Valid values are PARENT or CHILD.
        /// </summary>
        public string MatchTypeFilter { get; set; } = string.Empty;

        /// <summary>
        /// Optional filter for a specific course of fire definition.
        /// </summary>
        public SetName? CourseOfFireDefinition { get; set; } = null;

        /// <summary>
        /// Optional filter for course of fire common name.
        /// </summary>
        public string CommonName { get; set; } = string.Empty;

        /// <summary>
        /// Optional filter for course of fire discipline.
        /// </summary>
        public DisciplineType? Discipline { get; set; } = null;

        /// <summary>
        /// Optional filter for course of fire sub-discipline.
        /// </summary>
        public string SubDiscipline { get; set; } = string.Empty;

        /// <summary>
        /// Optional filter for course of fire target collection name.
        /// </summary>
        public string TargetCollectionName { get; set; } = string.Empty;

        /// <inheritdoc />
        public string Token { get; set; } = string.Empty;

        /// <inheritdoc />
        public int Limit { get; set; } = 50;

        /// <inheritdoc />
        public override string RelativePath {
            get { return "/match/list"; }
        }

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {
                var parameterList = new Dictionary<string, List<string>>();

                if (!string.IsNullOrWhiteSpace( OwnerId )) {
                    parameterList.Add( "owner-id", new List<string> { OwnerId } );
                }

                if (Visibility.HasValue) {
                    parameterList.Add( "visibility", new List<string> { EnumHelper.MemberValue( Visibility.Value ) } );
                }

                if (ShowOnSearch.HasValue) {
                    parameterList.Add( "show-on-search", new List<string> { ShowOnSearch.Value.ToString() } );
                }

                if (StartDate.HasValue) {
                    parameterList.Add( "start-date", new List<string> { StartDate.Value.ToString( DateTimeFormats.DATE_FORMAT ) } );
                }

                if (EndDate.HasValue) {
                    parameterList.Add( "end-date", new List<string> { EndDate.Value.ToString( DateTimeFormats.DATE_FORMAT ) } );
                }

                if (MemberPolicy.HasValue) {
                    parameterList.Add( "member-policy", new List<string> { EnumHelper.MemberValue( MemberPolicy.Value ) } );
                }

                if (ParentMatchId != null) {
                    parameterList.Add( "parent-match-id", new List<string> { ParentMatchId.ToString() } );
                }

                if (HasDownloaded.HasValue) {
                    parameterList.Add( "has-downloaded", new List<string> { HasDownloaded.Value.ToString() } );
                }

                if (ApprovalStatus.HasValue) {
                    parameterList.Add( "approval-status", new List<string> { EnumHelper.MemberValue( ApprovalStatus.Value ) } );
                }

                if (!string.IsNullOrWhiteSpace( MatchTypeFilter )) {
                    parameterList.Add( "match-type", new List<string> { MatchTypeFilter.Trim().ToUpperInvariant() } );
                }

                if (CourseOfFireDefinition != null) {
                    parameterList.Add( "course-of-fire-definition", new List<string> { CourseOfFireDefinition.ToString() } );
                }

                if (!string.IsNullOrWhiteSpace( CommonName )) {
                    parameterList.Add( "common-name", new List<string> { CommonName } );
                }

                if (Discipline.HasValue) {
                    parameterList.Add( "discipline", new List<string> { EnumHelper.MemberValue( Discipline.Value ) } );
                }

                if (!string.IsNullOrWhiteSpace( SubDiscipline )) {
                    parameterList.Add( "sub-discipline", new List<string> { SubDiscipline } );
                }

                if (!string.IsNullOrWhiteSpace( TargetCollectionName )) {
                    parameterList.Add( "target-collection-name", new List<string> { TargetCollectionName } );
                }

                if (Limit > 0) {
                    parameterList.Add( "limit", new List<string> { Limit.ToString() } );
                }

                if (!string.IsNullOrWhiteSpace( Token )) {
                    parameterList.Add( "token", new List<string> { Token } );
                }

                return parameterList;
            }
        }

        /// <inheritdoc />
        public override Request Copy() {
            return new ListMatchesAuthenticatedRequest( Credentials ) {
                OwnerId = OwnerId,
                Visibility = Visibility,
                ShowOnSearch = ShowOnSearch,
                StartDate = StartDate,
                EndDate = EndDate,
                MemberPolicy = MemberPolicy,
                ParentMatchId = ParentMatchId,
                HasDownloaded = HasDownloaded,
                ApprovalStatus = ApprovalStatus,
                MatchTypeFilter = MatchTypeFilter,
                CourseOfFireDefinition = CourseOfFireDefinition,
                CommonName = CommonName,
                Discipline = Discipline,
                SubDiscipline = SubDiscipline,
                TargetCollectionName = TargetCollectionName,
                Token = Token,
                Limit = Limit
            };
        }
    }
}
