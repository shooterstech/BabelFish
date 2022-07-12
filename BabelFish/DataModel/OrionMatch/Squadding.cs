using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.BabelFish.DataModel.OrionMatch {
    [Serializable]
    public class Squadding {

        public Squadding() { }

        /// <summary>
        /// GUID unique identifier of the squadding list
        /// </summary>
        public string SquaddingListID { get; set; }

        /// <summary>
        /// Unique MatchID for the competition to get squadding for. Will match exactly (assuming no errors) of the MatchID in the GetSquaddingListRequest
        /// 
        /// This is a required field.
        /// </summary>
        public string MatchID { get; set; } = string.Empty;

        public string ParentID { get; set; } = string.Empty;

        /// <summary>
        /// The EventName of the SquaddingList returned. Will match exactly (assuming no errors) of the EventName in the GetSquaddingListRequest
        /// </summary>
        public string EventName { get; set; } = string.Empty;

        /// <summary>
        /// The SquaddingList.
        /// </summary>
        public List<SquaddingAssignment> SquaddingList { get; set; } = new List<SquaddingAssignment>();

        public override string ToString() {
            StringBuilder foo = new StringBuilder();
            foo.Append("SquaddingList for ");
            foo.Append(EventName);
            return foo.ToString();
        }
    }
}
