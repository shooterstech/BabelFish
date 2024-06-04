using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetLeaguePublicRequest : Request {

        public GetLeaguePublicRequest( string leagueId = "" ) : base( "GetLeagueDetail" ) {
			LeagueId = leagueId;
        }

        public string LeagueId { get; set; } = string.Empty;

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/league/{LeagueId}"; }
        }
    }
}
