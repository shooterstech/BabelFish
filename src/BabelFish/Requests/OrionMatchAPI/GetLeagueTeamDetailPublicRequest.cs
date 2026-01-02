using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetLeagueTeamDetailPublicRequest : Request {

        public GetLeagueTeamDetailPublicRequest( MatchID leagueId, int teamId ) : base( "GetLeagueTeamDetail" ) {
            if ( leagueId is null ) { 
                throw new ArgumentNullException( "leagueId may not be empty string or null"); 
            }

			LeagueId = leagueId;
            TeamId = teamId;
        }

        public MatchID ? LeagueId { get; set; }

        public int TeamId { get; set;} = 0;

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/league/{LeagueId}/teams/{TeamId}"; }
        }
    }
}
