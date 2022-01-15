using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabelFish.DataModel.OrionMatch {
    [Serializable]
    public class Squadding {

        /// <summary>
        /// GUID unique identifier of the squadding list
        /// </summary>
        public string SquaddingListID { get; set; }

        public Squadding() {
            MatchID = "";
            EventName = "";
            this.SquaddingList = new List<SquaddingAssignment>();
        }

        /// <summary>
        /// Unique MatchID for the competition to get squadding for. Will match exactly (assuming no errors) of the MatchID in the GetSquaddingListRequest
        /// 
        /// This is a required field.
        /// </summary>
        public string MatchID { get; set; }

        public string ParentID { get; set; }


        /// <summary>
        /// The EventName of the SquaddingList returned. Will match exactly (assuming no errors) of the EventName in the GetSquaddingListRequest
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// The SquaddingList.
        /// </summary>
        public List<SquaddingAssignment> SquaddingList { get; set; }

        public override string ToString() {
            StringBuilder foo = new StringBuilder();
            foo.Append("SquaddingList for ");
            foo.Append(EventName);
            return foo.ToString();
        }
    }
}
