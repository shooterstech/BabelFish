using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class AddMergedResultListMemberAuthenticatedRequest : Request {

        public AddMergedResultListMemberAuthenticatedRequest( UserAuthentication credentials, MatchID tournamentId, string mergedId ) : base( "AddMergedResultListMember", credentials ) {
            HttpMethod = HttpMethod.Post;
            TournamentId = tournamentId ?? throw new ArgumentNullException( nameof( tournamentId ) );
            MergedId = mergedId ?? throw new ArgumentNullException( nameof( mergedId ) );
            SubDomain = APIClients.APISubDomain.AUTHAPI;
        }

        public AddMergedResultListMemberAuthenticatedRequest( UserAuthentication credentials, MatchID tournamentId, string mergedId, ResultListMember resultListMember ) : this( credentials, tournamentId, mergedId ) {
            ResultListMember = resultListMember ?? throw new ArgumentNullException( nameof( resultListMember ) );
        }

        /// <summary>
        /// The Match ID of the tournament containing the merged result list.
        /// </summary>
        public MatchID TournamentId { get; set; }

        /// <summary>
        /// The identifier of the merged result list receiving the member.
        /// </summary>
        public string MergedId { get; set; } = string.Empty;

        /// <summary>
        /// The member to add to the merged result list.
        /// </summary>
        public ResultListMember ResultListMember { get; set; } = new ResultListMember();

        /// <inheritdoc />
        public override string RelativePath {
            get {
                if (TournamentId == null) {
                    throw new ArgumentNullException( nameof( TournamentId ), "The tournament id must be set to add a merged result list member." );
                }

                if (string.IsNullOrWhiteSpace( MergedId )) {
                    throw new ArgumentNullException( nameof( MergedId ), "The merged id must be set to add a merged result list member." );
                }

                return $"/tournament/{TournamentId}/merged-resultlist/{MergedId}";
            }
        }

        /// <inheritdoc />
        public override StringContent PostParameters {
            get {
                if (ResultListMember == null) {
                    throw new ArgumentNullException( nameof( ResultListMember ), "The result list member must be set to add a merged result list member." );
                }

                var jsonAsString = G_NS.JsonConvert.SerializeObject( ResultListMember, SerializerOptions.NewtonsoftJsonSerializer );
                return new StringContent( jsonAsString, Encoding.UTF8, "application/json" );
            }
        }
    }
}
