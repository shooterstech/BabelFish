using System;
using System.Collections.Generic;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.Helpers.Extensions;
using System.Text;
using Scopos.BabelFish.DataModel.Clubs;

namespace Scopos.BabelFish.DataActors.OrionMatch
{
    public class CompareClubAbbr : IComparer<ClubAbbr>
    {
        public enum CompareMethod
        {

            /// <summary>
            /// Sort by AccountNumber
            /// </summary>
            ACCOUNT_NUMBER,

            /// <summary>
            /// Sort by Name of Club
            /// </summary>
            NAME,

            /// <summary>
            /// Sort by if the club is shooting/Has shot in 15 mins
            /// </summary>
            IS_SHOOTING
        };

        public CompareClubAbbr(CompareMethod compareMethod, SortBy sortBy)
        {
            Method = compareMethod;
            SortBy = sortBy;
        }

        public CompareMethod Method { get; private set; }

        public SortBy SortBy { get; private set; }

        /// <inheritdoc/>
        public int Compare(ClubAbbr x, ClubAbbr y)
        {

            int compare = 0;
            Individual X, Y;

            switch (Method)
            {

                case CompareMethod.ACCOUNT_NUMBER:

                    compare = x.AccountNumber.CompareTo(y.AccountNumber);

                    break;

                case CompareMethod.NAME:

                    compare = x.Name.CompareTo(y.Name);

                    break;

                case CompareMethod.IS_SHOOTING:

                    compare = x.IsCurrentlyShooting.CompareTo(y.IsCurrentlyShooting);

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
