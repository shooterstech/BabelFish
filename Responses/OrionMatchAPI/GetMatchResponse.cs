using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabelFish.DataModel.OrionMatch;

namespace BabelFish.Responses.OrionMatchAPI
{
    public class GetMatchResponse : Response<Match>
    {
        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public Match Match
        {
            get { return Value; }
        }
    }
}
