using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class DeleteMergedResultListAuthenticatedRequest : Request {

        public DeleteMergedResultListAuthenticatedRequest( UserAuthentication credentials, MatchID tournamentId, string mergedId ) : base( "DeleteMergedResultList", credentials ) {
            HttpMethod = HttpMethod.Delete;
            TournamentId = tournamentId ?? throw new ArgumentNullException( nameof( tournamentId ) );
            MergedId = mergedId ?? throw new ArgumentNullException( nameof( mergedId ) );
            SubDomain = APIClients.APISubDomain.AUTHAPI;
        }

        /// <summary>
        /// The Match ID of the tournament where the merged result list exists.
        /// </summary>
        public MatchID TournamentId { get; set; }

        /// <summary>
        /// The identifier of the merged result list to delete.
        /// </summary>
        public string MergedId { get; set; } = string.Empty;

        /// <inheritdoc />
        public override string RelativePath {
            get {
                if (TournamentId == null) {
                    throw new ArgumentNullException( nameof( TournamentId ), "The tournament id must be set to delete a merged result list." );
                }

                return $"/tournament/{TournamentId}/merged-resultlist";
            }
        }

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {
                if (string.IsNullOrWhiteSpace( MergedId )) {
                    throw new ArgumentNullException( nameof( MergedId ), "The merged id must be set to delete a merged result list." );
                }

                return new Dictionary<string, List<string>> {
                    { "merged-id", new List<string> { MergedId } }
                };
            }
        }
    }
}
