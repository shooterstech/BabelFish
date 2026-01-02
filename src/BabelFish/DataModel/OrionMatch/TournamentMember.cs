using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    public class TournamentMember {

        /// <summary>
        /// The Unique Match ID that is a Member of the Tournament.
        /// </summary>
        [G_NS.JsonProperty( Order = 1 )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.MatchIdConverter ) )]
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.MatchIdConverter ) )]
        public MatchID MatchId { get; set; }

        /// <summary>
        /// The name of the Match that is a Member of the Tournament.
        /// </summary>
        [G_NS.JsonProperty( Order = 2 )]
        public string MatchName { get; set; }
    }
}
