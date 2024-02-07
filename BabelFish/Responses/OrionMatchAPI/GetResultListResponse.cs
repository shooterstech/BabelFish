using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {

    public abstract class GetResultListResponse : Response<ResultListWrapper> {
        /*
         * TODO: Figure out the best way to make GetResultListResponse implement the interface ITokenResponse<>
         */

        public GetResultListResponse() : base() {
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public ResultList ResultList {
            get { return Value.ResultList; }
        }
    }
}
