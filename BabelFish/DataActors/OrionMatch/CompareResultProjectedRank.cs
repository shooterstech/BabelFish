using Scopos.BabelFish.DataModel.OrionMatch;
using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.DataActors.OrionMatch {

    /// <summary>
    /// Compares (thus allowing to sort), two ResultEvents by their existing ProjectedRank.
    /// This class does not calculate their Projected Score or tries to sort them based on their
    /// Projected Score, rather, or only sorts the existing values.
    /// </summary>
    public class CompareResultProjectedRank : IComparer<ResultEvent> {
        public enum CompareMethod {
            /// <summary>
            /// Sort by PROJECTED RANK
            /// </summary>
            PROJECTED_RANK
        };

        public CompareResultProjectedRank( CompareMethod method = CompareMethod.PROJECTED_RANK, SortBy sortBy = SortBy.DESCENDING ) {
            Method = method;
            SortBy = sortBy;
        }

        public CompareMethod Method { get; private set; }

        public SortBy SortBy { get; private set; }

        public int Compare( ResultEvent x, ResultEvent y ) {

            int compare = 0;

            switch (Method) {
                case CompareMethod.PROJECTED_RANK:
                    //not sure if that is the safe way I should do it, but it is technically effective.
                    if (x.ProjectedRankOrder <= 0) x.ProjectedRankOrder = x.RankOrder;
                    if (y.ProjectedRankOrder <= 0) y.ProjectedRankOrder = y.RankOrder;
                    compare = x.ProjectedRankOrder.CompareTo( y.ProjectedRankOrder );

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
    }
}
