using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI
{
    public class GetResultCOFDetailAuthenticatedResponse : Response<CourseOfFireWrapper> {

        public GetResultCOFDetailAuthenticatedResponse( GetResultCOFDetailAuthenticatedRequest request ) : base() {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public ResultCOF ResultCOF
        {
            get { return Value.ResultCOF; }
        }  
    }
}
