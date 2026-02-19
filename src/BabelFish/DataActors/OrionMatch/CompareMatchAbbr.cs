using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;
using System.Text.RegularExpressions;


namespace Scopos.BabelFish.DataActors.OrionMatch {
    /// <summary>
    /// Compares (thus allowing to sort), two MatchAbbr objects by chosen value.
    /// default sorting is Distance, though that is only known if the caller gave their location.
    /// </summary>
    public class CompareMatchAbbr : IComparer<MatchAbbr>{
        public enum CompareMethod {
            /// <summary>
            /// DateTime StartDate of match
            /// </summary>
            START_DATE,

            /// <summary>
            /// DateTime EndDate of match
            /// </summary>
            END_DATE,

            /// <summary>
            /// string OwnerId creator of the match
            /// </summary>
            OWNER_ID,

            /// <summary>
            /// MatchID MatchID of match, unique per match
            /// </summary>
            MATCH_ID,

            /// <summary>
            /// string MatchName, Human readable name of the match
            /// </summary>
            MATCH_NAME,

            /// <summary>
            /// double (nullable) Location.Distance, matches kilometers to caller, if known
            /// </summary>
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
                    // how happy is this for Orion & AtHomeAccount?
                    compare = ParseOwnerIdForAccountNumber(x.OwnerId).CompareTo(ParseOwnerIdForAccountNumber(y.OwnerId));
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

        private int ParseOwnerIdForAccountNumber(string ownerId) {
            var match = Regex.Match(ownerId, @"\d+");
            int number = int.Parse(match.Value);

            return number;
        }
    }
}
