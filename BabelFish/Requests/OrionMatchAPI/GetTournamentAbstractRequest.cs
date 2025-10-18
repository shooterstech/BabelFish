using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public abstract class GetTournamentAbstractRequest : Request {

        public GetTournamentAbstractRequest( string operationId, MatchID tournamenthId ) : base( operationId ) {
            this.TournamentID = tournamenthId;
        }

        public GetTournamentAbstractRequest( string operationId, MatchID tournamenthId, UserAuthentication credentials ) : base( operationId, credentials) {
            this.TournamentID = tournamenthId;
        }

        public static GetTournamentAbstractRequest Factory( MatchID tournamenthId, UserAuthentication credentials = null ) {
            if ( credentials == null ) {
                return new GetTournamentPublicRequest( tournamenthId );
            } else {
                return new GetTournamentAuthenticatedRequest( tournamenthId, credentials );
            }
        }

        public MatchID TournamentID { get; set; }

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/tournament/{TournamentID}"; }
        }
    }
}
