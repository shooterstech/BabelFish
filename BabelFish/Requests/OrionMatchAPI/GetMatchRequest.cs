using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.Requests.OrionMatchAPI 
{
    public class GetMatchRequest : Request
    {
        public GetMatchRequest(string matchid = "")
        {
            MatchID = matchid;
        }

        public string MatchID { get; set; } = string.Empty;

        /// <inheritdoc />
        public override string RelativePath
        {
            get { return $"/match/{MatchID}"; }
        }
    }
}
