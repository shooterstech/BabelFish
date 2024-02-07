using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public abstract class GetResultCOFDetailResponse : Response<CourseOfFireWrapper> {


        public GetResultCOFDetailResponse() : base() {
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public ResultCOF ResultCOF {
            get { return Value.ResultCOF; }
        }
    }
}
