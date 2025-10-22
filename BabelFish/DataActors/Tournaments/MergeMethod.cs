using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.Tournaments {
    public abstract class MergeMethod {

        public TournamentMerger TournamentMerger { get; private set; }

        protected MergeConfiguration _mergeConfiguration { get; private set; }

        public MergeMethod( TournamentMerger tournamentMerger, MergeConfiguration configuration ) {
            this.TournamentMerger = tournamentMerger;
            this._mergeConfiguration = configuration;
        }

        public abstract void Merge( ResultEvent re );

        /// <summary>
        /// List of EventNames the merge method adds to each participant's ResultCofScores.
        /// Examples might include "Average", "Sum", "High Score"
        /// </summary>
        public List<string> EventNames { get; protected set; } = new List<string>();

        /// <summary>
        /// The top level event name that this merge methods adds to each participant's ResultCofScores.
        /// This event name SHOULD also be included in .EventNames.
        /// </summary>
        public string TopLevelEventname { get; protected set; } = string.Empty;
    }
}
