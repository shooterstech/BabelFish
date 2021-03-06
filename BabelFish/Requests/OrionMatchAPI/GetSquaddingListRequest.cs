using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.BabelFish.Requests.OrionMatchAPI 
{
    public class GetSquaddingListRequest : Request
    {
        public GetSquaddingListRequest(string matchid = "", string squaddinglistname = "") : base()
        {
            MatchID = matchid;
            SquaddingListName = squaddinglistname;
        }
        public string MatchID { get; set; } = string.Empty;

        public string SquaddingListName { get; set; } = string.Empty;

        /// <inheritdoc />
        public override string RelativePath
        {
            get { return $"/match/{MatchID}/squadding-list/{SquaddingListName}"; }
        }
    }
}
