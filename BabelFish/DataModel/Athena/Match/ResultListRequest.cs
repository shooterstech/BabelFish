using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.DataModel.Athena.Match
{
    /// <summary>
    /// Represents a request, probable from a EST Display module, asking for one or more
    /// Result Lists be published on /{OrionAcct}/{MatchID}/resultList
    /// </summary>
    public class ResultListRequest
    {

        /// <summary>
        /// The Result List's Match ID.
        /// </summary>
        public string MatchID { get; set; }

        /// <summary>
        /// A list of strings representing the Result List Names in the request. 
        /// </summary>
        public List<string> ResultName { get; set; }
    }
}