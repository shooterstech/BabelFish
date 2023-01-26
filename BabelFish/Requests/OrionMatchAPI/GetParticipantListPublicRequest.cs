using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetParticipantListPublicRequest : Request {
        public GetParticipantListPublicRequest( string matchid = "" ) : base( "GetParticipantList" ) {
            MatchID = matchid;
        }

        public string MatchID { get; set; } = string.Empty;

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/match/{MatchID}/participant"; }
        }
    }
}
