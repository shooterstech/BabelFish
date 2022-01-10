using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabelFish.DataModel.Match;

namespace BabelFish.Requests {
    public class GetMatchResponse {

        public GetMatchResponse() {
            this.Match = new Match();
        }

        public Match Match { get; set; }  
    }
}
