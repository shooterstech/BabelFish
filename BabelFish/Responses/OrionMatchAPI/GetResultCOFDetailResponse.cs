using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShootersTech.BabelFish.DataModel.OrionMatch;
using ShootersTech.BabelFish.Requests.OrionMatchAPI;

namespace ShootersTech.BabelFish.Responses.OrionMatchAPI
{
    public class GetResultCOFDetailResponse : Response<CourseOfFireWrapper> {

        public GetResultCOFDetailResponse( GetResultCOFDetailRequest request ) : base() {
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
