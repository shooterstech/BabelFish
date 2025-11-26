using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.OrionMatch
{
	public  class PressReleaseGeneration
	{

        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.MatchIdConverter ) )]
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.MatchIdConverter ) )]
        public MatchID LeagueId { get; set; }


        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.MatchIdConverter ) )]
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.MatchIdConverter ) )]
        public MatchID GameId { get; set; }

        /// <summary>
        /// The name of the game.
        /// </summary>
        public string GameName { get; set; }

        /// <summary>
        /// URL of the location of the press release.
        /// <remark>Typically formated as HTML.</remark>
        /// </summary>
		public string PressReleaseUrl { get; set; }

        /// <summary>
        /// Boolean indicating if the press release was re-generated. If false, it means the press
        /// release did not previously exist.
        /// </summary>
		public bool Regenerated { get; set; }
	}
}
