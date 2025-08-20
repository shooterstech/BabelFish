using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.DataActors.OrionMatch
{

    /// <summary>
    /// Implements the IComparer interface to sort a list of Participants, such as one
    /// would have if they called the GetSquaddingList REST API call, or the GetMatchParticipant
    /// REST API call.
    /// </summary>

    public class CompareParticipant : IComparer<IRLIFItem>
    {

        public enum CompareMethod
        {

            /// <summary>
            /// Sort by Given/First Name
            /// </summary>
            GIVENNAME_FAMILYNAME,

            /// <summary>
            /// Sort by Last name, then first name
            /// </summary>
            FAMILYNAME_GIVENNAME,

            /// <summary>
            /// Sort by Display Name
            /// </summary>
            DISPLAYNAME
        };

        public CompareParticipant(CompareMethod compareMethod, SortBy sortBy)
        {
            Method = compareMethod;
            SortBy = sortBy;
        }

        public CompareMethod Method { get; private set; }

        public SortBy SortBy { get; private set; }

        /// <inheritdoc/>
        public int Compare( IRLIFItem x, IRLIFItem y )
        {

            int compare = 0;

            Individual X, Y;

            switch (Method)
            {
                case CompareMethod.FAMILYNAME_GIVENNAME:
                    if (x.Participant is not Individual
                        || y.Participant is not Individual)
                        throw new ArgumentException("Hey hey hey, we need individuals, not teams");

                    X = (Individual)x.Participant;
                    Y = (Individual)y.Participant;

                    compare = X.LastName.CompareTo(Y.LastName);
                    if (compare == 0)
                        compare = X.FirstName.CompareTo(Y.FirstName);

                    break;

                case CompareMethod.GIVENNAME_FAMILYNAME:
                    if (x.Participant is not Individual
                        || y.Participant is not Individual)
                        throw new ArgumentException("Hey hey hey, we need individuals, not teams");

                    X = (Individual)x.Participant;
                    Y = (Individual)y.Participant;

                    compare = X.FirstName.CompareTo(Y.FirstName);
                    if (compare == 0)
                        compare = X.LastName.CompareTo(Y.LastName);

                    break;

                case CompareMethod.DISPLAYNAME:
                    compare = x.Participant.DisplayName.CompareTo(y.Participant.DisplayName);
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
