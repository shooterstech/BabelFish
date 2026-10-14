namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class ListMatchesPublicRequest : ListMatchesAbstractRequest {

        public ListMatchesPublicRequest() : base( "ListMatches" ) {
        }

        /// <inheritdoc />
        public override Request Copy() {
            return new ListMatchesPublicRequest() {
                OwnerId = OwnerId,
                IncludeAwayMatches = IncludeAwayMatches,
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
