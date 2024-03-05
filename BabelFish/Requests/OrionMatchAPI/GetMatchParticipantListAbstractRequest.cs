using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public abstract class GetMatchParticipantListAbstractRequest : Request, ITokenRequest {

        public GetMatchParticipantListAbstractRequest( string operationId, MatchID matchId ) : base( operationId ) {
            this.MatchID = matchId;
        }

        public GetMatchParticipantListAbstractRequest( string operationId, MatchID matchId, UserAuthentication credentials ) : base( operationId, credentials ) {
            this.MatchID = matchId;
        }
        public static GetMatchParticipantListAbstractRequest Factory( MatchID matchId, UserAuthentication credentials = null ) {
            if (credentials == null) {
                return new GetMatchParticipantListPublicRequest( matchId );
            } else {
                return new GetMatchParticipantListAuthenticatedRequest( matchId, credentials );
            }
        }

        public MatchID MatchID { get; set; }

        /// <summary>
        /// Used to limit the request to a specific type of MatchParticipantRole.
        /// </summary>
        public MatchParticipantRole Role { get; set; } = MatchParticipantRole.NONE;

        /// <inheritdoc />
        public string Token { get; set; } = string.Empty;

        /// <inheritdoc />
        public int Limit { get; set; } = 0;

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/match/{MatchID}/participant"; }
        }

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();

                if (!string.IsNullOrEmpty( Token )) {
                    parameterList.Add( "token", new List<string> { Token } );
                }

                if (Role != MatchParticipantRole.NONE) {
                    parameterList.Add( "role", new List<string> { Role.Description() } );
                }

                if (Limit > 0)
                    parameterList.Add( "limit", new List<string> { Limit.ToString() } );

                return parameterList;
            }
        }
    }
}
