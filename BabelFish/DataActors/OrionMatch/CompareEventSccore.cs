using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.OrionMatch {
    public class CompareEventScore : IComparer<EventScore> {

        public SetName ScoreFormatCollectionDef { get; private set; }
        public ScoreFormatCollection ScoreFormat { get; private set; }

        public string ScoreConfigName {  get; private set; }

        public SortBy SortBy { get; private set; } = SortBy.DESCENDING;

        public CompareEventScore(SetName scoreConfigDef, string scoreConfigName) { 
            ScoreFormatCollectionDef = scoreConfigDef;
            ScoreConfigName = scoreConfigName;

            if ( DefinitionCache.TryGetScoreFormatCollectionDefinition( ScoreFormatCollectionDef, out ScoreFormatCollection sc ) ) {
                this.ScoreFormat = sc;
            }
        }

        public int Compare( EventScore x, EventScore y ) {
            int compare = 0;

            //TODO: Make this method generic so it can support any ScoreFormatCollection

            switch (this.ScoreConfigName) {
                case ("Integer"):
                    compare = x.Score.I.CompareTo( y.Score.I );
                    if ( compare == 0 )
                        compare = x.Score.X.CompareTo( y.Score.X );
                    break;

                default:
                    throw new NotImplementedException();

            }

            if (SortBy == SortBy.ASCENDING)
                return compare;
            else
                return -1 * compare;
        }
    }
}
