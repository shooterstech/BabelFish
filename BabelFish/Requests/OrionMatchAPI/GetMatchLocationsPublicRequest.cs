using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetMatchLocationsPublicRequest : Request {
        public GetMatchLocationsPublicRequest() : base( "GetMatchLocations" ) { }

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/match/locations"; }
        }
    }
}
