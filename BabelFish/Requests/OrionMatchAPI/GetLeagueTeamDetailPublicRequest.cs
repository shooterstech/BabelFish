using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetLeagueTeamDetailPublicRequest : Request {

        public GetLeagueTeamDetailPublicRequest( string leagueId, int teamId ) : base( "GetLeagueTeamDetail" ) {
            if (string.IsNullOrEmpty( leagueId ) ) { throw new ArgumentNullException( "leagueId may not be empty string or null"); }

			LeagueId = leagueId;
            TeamId = teamId;
        }

        public string LeagueId { get; set; } = string.Empty;

        public int TeamId { get; set;} = 0;

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/league/{LeagueId}/teams/{TeamId}"; }
        }
    }
}
