using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabelFish.DataModel.OrionMatch;

namespace BabelFish.Responses.OrionMatchAPI
{
    public class GetMatchLocationsResponse : Response<MatchLocationWrapper>
    {
        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public List<MatchLocation> MatchLocations
        {
            get { return Value.MatchLocations; }
        }
    }
}
