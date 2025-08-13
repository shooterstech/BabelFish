using NLog.LayoutRenderers;
using Scopos.BabelFish.DataModel.Athena.ESTUnitCommands;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.Helpers.Extensions;
using System.ComponentModel;

namespace Scopos.BabelFish.DataActors.OrionMatch
{
    /*
    /// <summary>
    /// Implements the IComparer interface to sort a list of SquaddingAssignmentFiringPoint, such as one
    /// would have if they called the GetSquaddingList REST API call. 
    /// </summary>
    public class CompareSquaddingAssignmentFiringPoint : IComparer<SquaddingAssignmentFiringPoint>
    {
        public enum CompareMethod
        {

            /// <summary>
            /// Sort by Display Name
            /// </summary>
            DISPLAYNAME,

            /// <summary>
            /// Sort by Given/First Name
            /// </summary>
            GIVENNAME_FAMILYNAME,

            /// <summary>
            /// Sort by Family/Last Name
            /// </summary>
            FAMILYNAME_GIVENNAME,

            /// <summary>
            /// Sort by Relay number, then Firing point
            /// </summary>
            RELAY_FIRINGPOINT_DISPLAYNAME,

            /// <summary>
            /// Sort by relay then given/first name
            /// </summary>
            RELAY_DISPLAYNAME,

            /// <summary>
            /// Sort by firing point, then RELAY
            /// </summary>
            FIRINGPOINT_RELAY_DISPLAYNAME,

            /// <summary>
            /// Sort by firing point, then Display Name
            /// </summary>
            FIRINGPOINT_DISPLAYNAME
        };

        public CompareSquaddingAssignmentFiringPoint(CompareMethod compareMethod, SortBy sortBy)
        {
            Method = compareMethod;
            SortBy = sortBy;
        }

        public CompareMethod Method { get; private set; }

        public SortBy SortBy { get; private set; }

        /// <inheritdoc/>
        public int Compare(SquaddingAssignmentFiringPoint x, SquaddingAssignmentFiringPoint y)
        {

            int compare = 0;
            Individual X, Y;

            switch (Method)
            {

                case CompareMethod.DISPLAYNAME:

                    compare = x.Participant.DisplayName.CompareTo(y.Participant.DisplayName);

                    break;

                case CompareMethod.GIVENNAME_FAMILYNAME:
                    if (x.Participant is not Individual
                        || y.Participant is not Individual)
                        throw new ArgumentException("Those aren't people! Individuals only!");

                    X = (Individual)x.Participant;
                    Y = (Individual)y.Participant;

                    compare = X.GivenName.CompareTo(Y.GivenName);
                    if (compare == 0)
                        compare = Y.LastName.CompareTo(X.LastName);
                    break;

                case CompareMethod.FAMILYNAME_GIVENNAME:
                    if (x.Participant is not Individual
                        || y.Participant is not Individual)
                        throw new ArgumentException("Those aren't people! Individuals only!");

                    X = (Individual)x.Participant;
                    Y = (Individual)y.Participant;

                    compare = X.LastName.CompareTo(Y.LastName);
                    if (compare == 0)
                        compare = Y.GivenName.CompareTo(X.GivenName);
                    break;

                case CompareMethod.RELAY_FIRINGPOINT_DISPLAYNAME:

                    compare = x.Relay.CompareToAsIntegers(y.Relay);
                    if (compare == 0)
                    {
                        compare = x.FiringPoint.CompareToAsIntegers(y.FiringPoint);
                        if (compare == 0)
                        {
                            compare = x.Participant.DisplayName.CompareTo(y.Participant.DisplayName);
                        }
                    }
                    break;

                case CompareMethod.RELAY_DISPLAYNAME:

                    compare = x.Relay.CompareToAsIntegers(y.Relay);
                    if (compare == 0)
                    {
                        compare = x.Participant.DisplayName.CompareTo(y.Participant.DisplayName);
                    }
                    break;

                case CompareMethod.FIRINGPOINT_RELAY_DISPLAYNAME:
                    compare = x.FiringPoint.CompareToAsIntegers(y.FiringPoint);
                    if (compare == 0)
                    {
                        compare = x.Relay.CompareToAsIntegers(y.Relay);
                        if (compare == 0)
                        {
                            compare = x.Participant.DisplayName.CompareTo(y.Participant.DisplayName);
                        }
                    }
                    break;

                case CompareMethod.FIRINGPOINT_DISPLAYNAME:

                    compare = x.FiringPoint.CompareToAsIntegers(y.FiringPoint);
                    if (compare == 0)
                    {
                        compare = x.Participant.DisplayName.CompareTo(y.Participant.DisplayName);
                    }
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
    */
}
