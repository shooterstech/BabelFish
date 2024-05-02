using NLog.LayoutRenderers;
using Scopos.BabelFish.DataModel.Athena.ESTUnitCommands;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.OrionMatch
{
    public class CompareSquadding : IComparer<SquaddingAssignmentFiringPoint>
    {
        public enum CompareMethod
        {

            /// <summary>
            /// Sort by Relay number
            /// </summary>
            RELAY,

            /// <summary>
            /// Sort by Firing point
            /// </summary>
            FIRINGPOINT,

            /// <summary>
            /// Sort by Given/First Name
            /// </summary>
            FIRSTLAST,

            /// <summary>
            /// Sort by Family/Last Name
            /// </summary>
            LASTFIRST,

            /// <summary>
            /// Sort by Relay number, then Firing point
            /// </summary>
            RELAYFIRINGPOINT,

            /// <summary>
            /// Sort by firing point, then RELAY
            /// </summary>
            FIRINGPOINTRELAY,

            /// <summary>
            /// Sort by relay then given/first name
            /// </summary>
            RELAYFIRSTLAST,

            /// <summary>
            /// Sort by relay then family/last name
            /// </summary>
            RELAYLASTFIRST
        };

        public CompareSquadding(CompareMethod compareMethod, SortBy sortBy)
        {
            this.Method = compareMethod;
            this.SortBy = sortBy;
        }

        public CompareMethod Method { get; private set; }

        public SortBy SortBy { get; private set; }

        /// <inheritdoc/>
        public int Compare(SquaddingAssignmentFiringPoint x, SquaddingAssignmentFiringPoint y)
        {

            int compare = 0;

            switch (Method)
            {
                case CompareMethod.RELAY:
                    if (x.Participant is not Individual
                        || y.Participant is not Individual)
                        throw new ArgumentException("Those aren't people! Individuals only!");
                    compare = CompareStringInt(x.Relay, y.Relay);
                    break;

                case CompareMethod.FIRINGPOINT:
                    if (x.Participant is not Individual
                        || y.Participant is not Individual)
                        throw new ArgumentException("Those aren't people! Individuals only!");
                    compare = CompareStringInt(x.FiringPoint, y.FiringPoint);
                    break;

                case CompareMethod.FIRSTLAST:
                    if (x.Participant is not Individual
                        || y.Participant is not Individual)
                        throw new ArgumentException("Those aren't people! Individuals only!");

                    Individual X = (Individual)x.Participant;
                    Individual Y = (Individual)y.Participant;

                    compare = X.GivenName.CompareTo(Y.GivenName);
                    if (compare == 0)
                        compare = Y.LastName.CompareTo(X.LastName);
                    break;

                case CompareMethod.LASTFIRST:
                    if (x.Participant is not Individual
                        || y.Participant is not Individual)
                        throw new ArgumentException("Those aren't people! Individuals only!");

                    Individual Xind = (Individual)x.Participant;
                    Individual Yind = (Individual)y.Participant;

                    compare = Xind.LastName.CompareTo(Yind.LastName);
                    if (compare == 0)
                        compare = Yind.GivenName.CompareTo(Xind.GivenName);
                    break;

                case CompareMethod.RELAYFIRINGPOINT:
                    if (x.Participant is not Individual
                        || y.Participant is not Individual)
                        throw new ArgumentException("Those aren't people! Individuals only!");
                    compare = CompareStringInt(x.Relay, y.Relay);
                    if (compare == 0)
                        compare = CompareStringInt(x.FiringPoint, y.FiringPoint);
                    break;

                case CompareMethod.FIRINGPOINTRELAY:
                    if (x.Participant is not Individual
                        || y.Participant is not Individual)
                        throw new ArgumentException("Those aren't people! Individuals only!");
                    compare = CompareStringInt(x.FiringPoint, y.FiringPoint);
                    if (compare == 0)
                        compare = CompareStringInt(x.Relay, y.Relay);
                    break;

                case CompareMethod.RELAYFIRSTLAST:
                    if (x.Participant is not Individual
                        || y.Participant is not Individual)
                        throw new ArgumentException("Those aren't people! Individuals only!");
                    compare = CompareStringInt(x.Relay, y.Relay);
                    if (compare == 0)
                    {
                        Individual Xindi = (Individual)x.Participant;
                        Individual Yindi = (Individual)y.Participant;

                        compare = Xindi.GivenName.CompareTo(Yindi.GivenName);
                        if (compare == 0)
                            compare = Yindi.LastName.CompareTo(Xindi.LastName);
                    }
                    break;

                case CompareMethod.RELAYLASTFIRST:
                    if (x.Participant is not Individual
                        || y.Participant is not Individual)
                        throw new ArgumentException("Those aren't people! Individuals only!");
                    compare = CompareStringInt(x.Relay, y.Relay);
                    if (compare == 0)
                    {
                        Individual Xindiv = (Individual)x.Participant;
                        Individual Yindiv = (Individual)y.Participant;

                        compare = Xindiv.LastName.CompareTo(Yindiv.LastName);
                        if (compare == 0)
                            compare = Yindiv.GivenName.CompareTo(Xindiv.GivenName);
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

        private int CompareStringInt(string x, string y)
        {
            if (int.TryParse(x, out int Xint))
            {
                if (int.TryParse(y, out int Yint))
                {//both are ints
                    return Xint.CompareTo(Yint);
                }
                else
                {//x is int, y is not
                    return 1;
                }
            }
            else
            {//x is not an int
                if (int.TryParse(y, out int Yint))
                {//x not int, y is int
                    return -1;
                }
                else
                {//x and y strings
                    return x.CompareTo(y);
                }
            }
        }
    }
}
