using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class ListMatchesAuthenticatedRequest : ListMatchesAbstractRequest {

        public ListMatchesAuthenticatedRequest( UserAuthentication credentials ) : base( "ListMatches", credentials ) {
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
