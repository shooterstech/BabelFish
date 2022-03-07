using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabelFish.DataModel.OrionMatch;

namespace BabelFish.Responses.OrionMatchAPI
{
    public class GetSquaddingListResponse : Response<Squadding>
    {
        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public Squadding Squadding {
            get { return Value; }
        }

        /*
        protected override void ConvertBodyToValue() {

            Squadding squadding = new Squadding() {
                SquaddingListID = (string)Body["SquaddingListID"],
                MatchID = (string)Body["MatchID"],
                ParentID = (string)Body["ParentID"],
                EventName = (string)Body["EventName"]
            };

            foreach (var jToken in Body["SquaddingList"]) {
                if (jToken != null) {
                    var sa = jToken.ToObject<SquaddingAssignmentFiringPoint>();
                    squadding.SquaddingList.Add(sa);
                } else {
                    ; //Why is jToken null?
                }
            }

            Value = squadding;
        }
        */
    }
}
