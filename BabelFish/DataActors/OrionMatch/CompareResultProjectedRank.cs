using Scopos.BabelFish.DataModel.OrionMatch;
using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.DataActors.OrionMatch
{
    public class CompareResultProjectedRank : IComparer<ResultEvent>
    {
        public enum CompareMethod
        {
            /// <summary>
            /// Sort by PROJECTED RANK
            /// </summary>
            PROJECTED_RANK
        };
        public CompareResultProjectedRank(CompareMethod method, SortBy sortBy)
        {
            Method = method;
            SortBy = sortBy;
        }
        
        public CompareMethod Method { get; private set; }

        public SortBy SortBy { get; private set; }

        public int Compare(ResultEvent x, ResultEvent y)
        {

            int compare = 0;

            switch (Method)
            {
                case CompareMethod.PROJECTED_RANK:
                    //not sure if that is the safe way I should do it, but it is technically effective.
                    if (x.ProjectedRank <= 0) x.ProjectedRank = x.Rank;
                    if (y.ProjectedRank <= 0) y.ProjectedRank = y.Rank;
                    compare = x.ProjectedRank.CompareTo(y.ProjectedRank);
                    Console.WriteLine($"{compare} = {x.ProjectedRank} compareTo {y.ProjectedRank}");

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
