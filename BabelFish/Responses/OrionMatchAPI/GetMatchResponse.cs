using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {

    /// <summary>
    /// Abstract class to hide the difference between an Public and Authenticated API call.
    /// </summary>
    public abstract class GetMatchResponse : Response<MatchWrapper> {

        public GetMatchResponse( ) : base() {
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public Match Match {
            get { return Value.Match; }
        }
    }
}
