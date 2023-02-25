using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetSquaddingListPublicRequest : Request, ITokenRequest {

        public GetSquaddingListPublicRequest( string matchid = "", string squaddingEventName = "" ) : base( "GetSquaddingList" ) {
            MatchID = matchid;
            SquaddingEventName = squaddingEventName;
        }

        /// <inheritdoc />
        public override Request Copy() {
            var newRequest = new GetSquaddingListPublicRequest( MatchID, SquaddingEventName );
            newRequest.RelayName = this.RelayName;
            newRequest.Token = this.Token;

            return newRequest;
        }

        public string MatchID { get; set; } = string.Empty;

        public string SquaddingEventName { get; set; } = string.Empty;

        /// <summary>
        /// The relay query parameter limits the returned list of SquaddingAssignments to only those that have a 'Relay' name equal to this parameter's value. 
        /// An empty string or null value will return the entire list.
        /// </summary>
        public string RelayName { get; set; } = string.Empty;

        /// <inheritdoc />
        public string Token { get; set; } = string.Empty;

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/match/{MatchID}/squadding-list/{SquaddingEventName}"; }
        }

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();

                if (!string.IsNullOrEmpty( Token )) {
                    parameterList.Add( "token", new List<string> { Token } );
                }

                if (!string.IsNullOrEmpty( RelayName )) {
                    parameterList.Add( "relay", new List<string> { RelayName } );
                }

                return parameterList;
            }
        }
    }
}
