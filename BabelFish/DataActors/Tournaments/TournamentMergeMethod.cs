using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.Tournaments {
    public abstract class TournamentMergeMethod {

        public TournamentMerger TournamentMerger { get; private set; }

        public TournamentMergeMethod( TournamentMerger tournamentMerger ) {
            this.TournamentMerger = tournamentMerger;
        }

        public abstract void Merge( ResultEvent re );
    }
}
