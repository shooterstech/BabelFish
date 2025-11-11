using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    public class Tournament : MatchBase {

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
