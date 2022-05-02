using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.DataModel.Athena.Match
{
    public class SquaddingListRequest
    {

        public string MatchID { get; set; }
        public string SquaddingListID { get; set; }
        public string SquaddingListName { get; set; }
    }
}