using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Requests;
using Scopos.BabelFish.Runtime.Authentication;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.Requests.ClubsAPI {
    public class GetClubListPublicRequest : Request, ITokenRequest {

        public enum SearchParameterState { IGNORE, MUST_HAVE, MUST_NOT_HAVE }

        public GetClubListPublicRequest( ) : base( "GetClubList" ) {
            this.RequiresCredentials = false;
            this.SubDomain = APISubDomain.API;
        }

        /// <summary>
        /// parameter for searching the clubs DB, refer to SearchParameterState for explanation
        /// </summary>
        public SearchParameterState ShowAll { get; set; } = SearchParameterState.IGNORE;

        /// <summary>
        /// parameter for searching the clubs DB, refer to SearchParameterState for explanation
        /// </summary>
        public SearchParameterState EnabledRezults { get; set; } = SearchParameterState.IGNORE;

        /// <summary>
        /// parameter for searching the clubs DB, refer to SearchParameterState for explanation
        /// </summary>
        public SearchParameterState ActiveLicense { get; set; } = SearchParameterState.IGNORE;

        /// <summary>
        /// parameter for searching the clubs DB, refer to SearchParameterState for explanation
        /// </summary>
        public SearchParameterState OrionForClubs { get; set; } = SearchParameterState.IGNORE;

        /// <summary>
        /// parameter for searching the clubs DB, refer to SearchParameterState for explanation
        /// </summary>
        public SearchParameterState OrionAtHome { get; set; } = SearchParameterState.IGNORE;

        /// <summary>
        /// parameter for searching the clubs DB, refer to SearchParameterState for explanation
        /// </summary>
        public SearchParameterState AthenaForClubs { get; set; } = SearchParameterState.IGNORE;

        /// <summary>
        /// parameter for searching the clubs DB, refer to SearchParameterState for explanation
        /// </summary>
        public SearchParameterState CurrentlyShooting { get; set; } = SearchParameterState.IGNORE;

        /// <inheritdoc />
        public string Token { get; set; }

        /// <inheritdoc />
        public int Limit { get; set; } = 0;

        /// <inheritdoc />
        public override Request Copy() {
            var newRequest = new GetClubListPublicRequest( );
            newRequest.Token = this.Token;

            return newRequest;
        }


        /// <inheritdoc />
        public override string RelativePath {

            get { return $"/clubs"; }
        }

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();

                if (! string.IsNullOrEmpty( Token ) ) {
                    parameterList.Add( "token", new List<string> { Token } );
                }
                if (Limit > 0)
                    parameterList.Add( "limit", new List<string> { Limit.ToString() } );

                parameterList.Add("show-all", new List<string> { ShowAll.ToString() });
                parameterList.Add("enabled-rezults", new List<string> { EnabledRezults.ToString() });
                parameterList.Add("active-license", new List<string> { ActiveLicense.ToString() });
                parameterList.Add("orion-for-clubs", new List<string> { OrionForClubs.ToString() });
                parameterList.Add("orion-at-home", new List<string> { OrionAtHome.ToString() });
                parameterList.Add("athena-for-clubs", new List<string> { AthenaForClubs.ToString() });
                parameterList.Add("currently-shooting", new List<string> { CurrentlyShooting.ToString() } );

                return parameterList;
            }
        }
    }
}
