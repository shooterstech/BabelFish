using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.DataModel.OrionMatch {


    public class CompareParticipant : IComparer<IParticipant> {

        public enum CompareMethod { 
            /// <summary>
            /// Sort by Last name, then first name
            /// </summary>
            LASTNAME, 
            
            /// <summary>
            /// Sort by Display Name
            /// </summary>
            DISPLAYNAME 
        };

        public CompareParticipant( CompareMethod compareMethod, SortBy sortBy ) {
            this.Method = compareMethod;
            this.SortBy = sortBy;
        }

        public CompareMethod Method { get; private set; }

        public SortBy SortBy { get; private set; }

        /// <inheritdoc/>
        public int Compare( IParticipant x, IParticipant y ) {

            int compare = 0;

            switch ( Method ) {
                case CompareMethod.LASTNAME:
                    if (x.Participant is not Individual
                        || y.Participant is not Individual)
                        throw new ArgumentException( "Hey hey hey, we need individuals, not teams" );

                    Individual X = (Individual) x.Participant;
                    Individual Y = (Individual) y.Participant; 

                    compare = X.LastName.CompareTo( Y.LastName );
                    if (compare == 0)
                        compare = Y.FirstName.CompareTo( X.FirstName ); 

                    break;

                case CompareMethod.DISPLAYNAME:
                    compare = x.Participant.DisplayName.CompareTo( y.Participant.DisplayName );
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
