using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.BabelFish.Requests.OrionMatchAPI 
{
    public class GetParticipantListRequest : Request
    {
        public GetParticipantListRequest(string matchid = "")
        {
            MatchID = matchid;
        }
        public string MatchID { get; set; } = string.Empty;

        /// <inheritdoc />
        public override string RelativePath
        {
            get { return $"/match/{MatchID}/participant"; }
        }
    }
}
