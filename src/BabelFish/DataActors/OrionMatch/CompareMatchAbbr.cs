using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.OrionMatch {
    public class CompareMatchAbbr : IComparer<MatchAbbr>{
        public enum CompareMethod {
            START_DATE,
            END_DATE,
            OWNER_ID,
            MATCH_ID,
            MATCH_NAME,
            DISTANCE
        };


        public CompareMatchAbbr(CompareMethod method = CompareMethod.DISTANCE, SortBy sortBy = SortBy.DESCENDING) {
            Method = method;
            SortBy = sortBy;
        }

        public CompareMethod Method { get; private set; }

        public SortBy SortBy { get; private set; }

        public int Compare(MatchAbbr x, MatchAbbr y) {
            int compare = 0;

            switch (Method) {
                case CompareMethod.START_DATE:
                    compare = x.StartDate.CompareTo(y.StartDate);
                    break;

                case CompareMethod.END_DATE:
                    compare = x.EndDate.CompareTo(y.EndDate);
                    break;

                case CompareMethod.OWNER_ID:
                    //maybe should do breaking into account ID and compare ints?
                    compare = x.OwnerId.CompareTo(y.OwnerId);
                    break;

                case CompareMethod.MATCH_ID:
                    compare = x.MatchID.ToString().CompareTo(y.MatchID.ToString());
                    break;

                case CompareMethod.MATCH_NAME:
                    compare = x.MatchName.CompareTo(y.MatchName);
                    break;

                case CompareMethod.DISTANCE:
                    if(x.Location.Distance != null && y.Location.Distance!= null) {

                        compare = x.Location.Distance.Value.CompareTo(y.Location.Distance.Value);
                    }
                    else {
                        compare = 0;
                    }
                    break;

                default:
                    compare = 0;
                    break;
            }

            if (SortBy == SortBy.ASCENDING)
                return compare;
            else
                return -1 * compare;
        }
    }
}
