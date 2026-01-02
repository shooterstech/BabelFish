using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetLeaguePublicRequest : Request {

        public GetLeaguePublicRequest( MatchID ? leagueId ) : base( "GetLeagueDetail" ) {
			LeagueId = leagueId;
        }

        public MatchID ? LeagueId { get; set; }

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/league/{LeagueId}"; }
        }
    }
}
