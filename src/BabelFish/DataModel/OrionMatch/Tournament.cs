using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// A Tournament is a group of Matches.
    /// <para>The Matches in the Tournament are called its <see cref="TournamentMembers">members</see>.</para>
    /// <para>Scores from the member's Result Lists may be merged together in interesting ways (e.g. sum all the scores together).
    /// These are called <see cref="MergedResultList"/>.</para>
    /// <para>The <see cref="MatchID"/> for tournaments always end in ".2".</para>
    /// </summary>
    public class Tournament : MatchBase {

        /// <summary>
        /// Public constructor.
        /// <para>Usually not called directly, instead Tournaments are read from REST API using
        /// <see cref="OrionMatchAPIClient.GetTournamentPublicAsync(MatchID)"/> (or related method).</para>
        /// </summary>
        public Tournament() : base() { }

        /// <summary>
        /// Purposefully redundant. Exact same value as MatchId. But since this is a concrete 
        /// implementation of a Tournament we call it TournamentId.
        /// </summary>
        [G_NS.JsonProperty( Order = 3 )]
        public MatchID TournamentId {
            get {
                return this.MatchId;
            }
        }

        /// <summary>
        /// Purposefully redundant. Exact same value as MatchName. But since this is a concrete 
        /// implementation of a Tournament we call it TournamentName.
        /// </summary>
        [G_NS.JsonProperty( Order = 5 )]
        public string TournamentName {
            get {
                return this.MatchName;
            }
        }

        /// <summary>
        /// The list of Matches that are Members of this Tournament.
        /// </summary>
        [G_NS.JsonProperty( Order = 30 )]
        public List<TournamentMember> TournamentMembers { get; set; } = new List<TournamentMember>();

        /// <summary>
        /// 
        /// </summary>
        [G_NS.JsonProperty( Order = 35 )]
        public List<MergedResultList> MergedResultLists { get; set; } = new List<MergedResultList>();
    }
}
