using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShootersTech.DataModel.OrionMatch;
using ShootersTech.Requests.OrionMatchAPI;

namespace ShootersTech.Responses.OrionMatchAPI
{
    public class GetMatchSearchResponse : Response<MatchSearchWrapper>
    {

        public GetMatchSearchResponse( GetMatchSearchRequest request ) : base() {
            Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public List<Match> SearchList
        {
            get { return Value.SearchList; }
        }
    }
}
