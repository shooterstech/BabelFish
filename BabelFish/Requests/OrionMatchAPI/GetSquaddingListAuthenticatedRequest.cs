using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetSquaddingListAuthenticatedRequest : Request, ITokenRequest {

        public GetSquaddingListAuthenticatedRequest( MatchID matchid, string squaddingEventName, UserAuthentication credentials ) : base( "GetSquaddingList", credentials ) {
            MatchID = matchid;
            SquaddingEventName = squaddingEventName;
        }

        /// <inheritdoc />
        public override Request Copy() {
            var newRequest = new GetSquaddingListAuthenticatedRequest( MatchID, SquaddingEventName, Credentials );
            newRequest.RelayName = this.RelayName;
            newRequest.Token = this.Token;
            newRequest.Limit = this.Limit;

            return newRequest;
        }

        public MatchID MatchID { get; set; }

        public string SquaddingEventName { get; set; }

        /// <summary>
        /// The relay query parameter limits the returned list of SquaddingAssignments to only those that have a 'Relay' name equal to this parameter's value. 
        /// An empty string or null value will return the entire list.
        /// </summary>
        public string RelayName { get; set; } = string.Empty;

        /// <inheritdoc />
        public string Token { get; set; } = string.Empty;

        /// <inheritdoc />
        public int Limit { get; set; }

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

                if (Limit > 0)
                    parameterList.Add( "limit", new List<string> { Limit.ToString() } );

                return parameterList;
            }
        }
    }
}
