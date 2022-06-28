using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShootersTech.DataModel.OrionMatch;
using ShootersTech.Requests.OrionMatchAPI;

namespace ShootersTech.Responses.OrionMatchAPI
{
    public class GetResultListResponse : Response<ResultListWrapper>
    {

        public GetResultListResponse( GetResultListRequest request ) : base() {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public ResultList ResultList
        {
            get { return Value.ResultList; }
        }
    }
}
