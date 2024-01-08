using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Requests;
using Scopos.BabelFish.Runtime.Authentication;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.Requests.ClubsAPI {
    public class GetClubListPublicRequest : Request, ITokenRequest {


        public GetClubListPublicRequest( ) : base( "GetClubList" ) {
            this.RequiresCredentials = false;
            this.SubDomain = APISubDomain.API;
        }

        /// <summary>
        /// If true, the returned list will be limited by clubs that are currently shooting.
        /// </summary>
        public bool CurrentlyShooting { get; set; }

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

                if (CurrentlyShooting)
                    parameterList.Add( "currently-shooting", new List<string> { "true" } );

                return parameterList;
            }
        }
    }
}
