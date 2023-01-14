using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Requests;

namespace Scopos.BabelFish.Requests.ClubsAPI {
    public class GetClubListRequest : Request, IToken {

        public GetClubListRequest() { }

        /// <inheritdoc />
        public string Token { get; set; }


        /// <inheritdoc />
        public override string RelativePath {

            get { return $"/clubs"; }
        }

        public override Dictionary<string, List<string>> QueryParameters {
            get {

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();

                if (! string.IsNullOrEmpty( Token ) ) {
                    parameterList.Add( "token", new List<string> { Token } );
                }

                return parameterList;
            }
        }
    }
}
