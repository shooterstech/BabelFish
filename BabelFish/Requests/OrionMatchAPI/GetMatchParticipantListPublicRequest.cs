using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetMatchParticipantListPublicRequest : Request, ITokenRequest {

        public GetMatchParticipantListPublicRequest( string matchid = "" ) : base( "GetMatchParticipantList" ) {
            MatchID = matchid;
        }

        public string MatchID { get; set; } = string.Empty;

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

        public override Request Copy() {
            GetMatchParticipantListPublicRequest newRequest = new GetMatchParticipantListPublicRequest( MatchID );
            newRequest.Role = this.Role; 
            newRequest.Token = this.Token;
            return newRequest;
        }
    }
}
