using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.OrionMatch {
    public class CompareRelayInformation : IComparer<RelayInformation>{

        public enum CompareMethod {
            /// <summary>
            /// Sort by Relay Name.
            /// <para>NOTE: Relay Name is usually designated by an interger, e.g. "1", "2"</para>
            /// </summary>
            RELAY_NAME,

            /// <summary>
            /// Sort by Start Time
            /// </summary>
            START_TIME,

            /// <summary>
            /// Sort by Call to Line Time
            /// </summary>
            CALL_TO_LINE_TIME
        }

        public CompareRelayInformation( CompareMethod compareMethod, SortBy sortBy ) {
            Method = compareMethod;
            SortBy = sortBy;
        }

        public CompareMethod Method { get; private set; }

        public SortBy SortBy { get; private set; }

        public int Compare( RelayInformation x, RelayInformation y ) {

            int xAsInt = ExtractInt( x.RelayName );
            int yAsInt = ExtractInt( y.RelayName );
            int compare = 0;

            switch (Method) {
                case CompareMethod.RELAY_NAME:
                    if (xAsInt > 0 || yAsInt > 0) {
                        compare = xAsInt.CompareTo( yAsInt );
                    }

                    if (compare == 0) {
                        compare = x.RelayName.CompareTo( y.RelayName );
                    }
                    break;

                case CompareMethod.START_TIME:
                    compare = x.StartTime.CompareTo( y.StartTime );
                    break;

                case CompareMethod.CALL_TO_LINE_TIME:
                    compare = x.CallToLineTime.CompareTo( y.CallToLineTime );
                    break;

                default:
                    //Sholdn't ever get here, but the compiler makes us have a default case.
                    compare = 0;
                    break;
            }

            if (SortBy == SortBy.ASCENDING)
                return compare;
            else
                return -1 * compare;
        }

        static int ExtractInt( string input ) {
            var match = Regex.Match( input, @"\d+" ); // find one or more digits
            if (match.Success && int.TryParse( match.Value, out int result )) {
                return result;
            }
            return 0;
        }


    }
}
