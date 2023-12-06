using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class GetMatchAuthenticatedResponse : Response<MatchWrapper> {

        public GetMatchAuthenticatedResponse( GetMatchAuthenticatedRequest request ) : base() {
            this.Request = Request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public Match Match {
            get { return Value.Match; }
        }
    }
}