using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public abstract class GetSquaddingListAbstractRequest : Request, ITokenRequest {

        public GetSquaddingListAbstractRequest( string operationId, MatchID matchId, string squaddingEventName ) : base( operationId ) {
            this.MatchID = matchId;
            this.SquaddingEventName = squaddingEventName;
        }

        public GetSquaddingListAbstractRequest( string operationId, MatchID matchId, string squaddingEventName, UserAuthentication credentials ) : base( operationId, credentials ) {
            this.MatchID = matchId;
            this.SquaddingEventName = squaddingEventName;
        }

        public static GetSquaddingListAbstractRequest Factory( MatchID matchId, string squaddingEventName, UserAuthentication credentials = null ) {
            if (credentials == null) {
                return new GetSquaddingListPublicRequest( matchId, squaddingEventName );
            } else {
                return new GetSquaddingListAuthenticatedRequest( matchId, squaddingEventName, credentials );
            }
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
