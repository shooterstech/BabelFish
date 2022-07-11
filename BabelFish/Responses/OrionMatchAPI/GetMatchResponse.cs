using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShootersTech.BabelFish.DataModel.OrionMatch;
using ShootersTech.BabelFish.Requests.OrionMatchAPI;

namespace ShootersTech.BabelFish.Responses.OrionMatchAPI
{
    public class GetMatchResponse : Response<MatchWrapper>
    {

        public GetMatchResponse(GetMatchRequest request ) : base() {
            this.Request = Request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public Match Match
        {
            get { return Value.Match; }
        }
    }
}