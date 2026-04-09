using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class CreateMergedResultListAuthenticatedRequest : Request {

        public CreateMergedResultListAuthenticatedRequest( UserAuthentication credentials, MatchID tournamentId ) : base( "CreateMergedResultList", credentials ) {
            HttpMethod = HttpMethod.Post;
            TournamentId = tournamentId ?? throw new ArgumentNullException( nameof( tournamentId ) );
            SubDomain = APIClients.APISubDomain.AUTHAPI;
        }

        public CreateMergedResultListAuthenticatedRequest( UserAuthentication credentials, MatchID tournamentId, MergedResultList mergedResultList ) : this( credentials, tournamentId ) {
            MergedResultList = mergedResultList ?? throw new ArgumentNullException( nameof( mergedResultList ) );
        }

        /// <summary>
        /// The Match ID of the tournament where the merged result list is created.
        /// </summary>
        public MatchID TournamentId { get; set; }

        /// <summary>
        /// The merged result list to create.
        /// </summary>
        public MergedResultList MergedResultList { get; set; } = new MergedResultList();

        /// <inheritdoc />
        public override string RelativePath {
            get {
                if (TournamentId == null) {
                    throw new ArgumentNullException( nameof( TournamentId ), "The tournament id must be set to create a merged result list." );
                }

                return $"/tournament/{TournamentId}/merged-resultlist";
            }
        }

        /// <inheritdoc />
        public override StringContent PostParameters {
            get {
                if (MergedResultList == null) {
                    throw new ArgumentNullException( nameof( MergedResultList ), "The merged result list must be set to create a merged result list." );
                }

                var jsonAsString = G_NS.JsonConvert.SerializeObject( MergedResultList, SerializerOptions.NewtonsoftJsonSerializer );
                return new StringContent( jsonAsString, Encoding.UTF8, "application/json" );
            }
        }
    }
}
