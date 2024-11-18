using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.DataActors.OrionMatch {
    public class CompareLeagueTeam : IComparer<LeagueTeam> {

        public enum CompareMethod {
            TEAM_NAME
        }

        public CompareLeagueTeam( CompareMethod compareMethod, SortBy sortBy ) {
            Method = compareMethod;
            SortBy = sortBy;
        }

        public CompareMethod Method { get; private set; }

        public SortBy SortBy { get; private set; }

        public int Compare( LeagueTeam x , LeagueTeam y ) {

            int compare = 0;

            switch ( Method ) {
                case CompareMethod.TEAM_NAME:
                    compare = x.TeamName.CompareTo( y.TeamName ); 
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
