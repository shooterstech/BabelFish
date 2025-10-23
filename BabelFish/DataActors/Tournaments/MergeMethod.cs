using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;
using NLog;

namespace Scopos.BabelFish.DataActors.Tournaments {
    public abstract class MergeMethod {

        public TournamentMerger TournamentMerger { get; private set; }

        private static Logger _logger = LogManager.GetCurrentClassLogger();
        protected MergeConfiguration _mergeConfiguration { get; private set; }

        protected MergeMethod( TournamentMerger tournamentMerger, MergeConfiguration configuration ) {
            this.TournamentMerger = tournamentMerger;
            this._mergeConfiguration = configuration;
        }

        public static MergeMethod Factory( TournamentMerger tournamentMerger, MergedResultList mrl ) {

            MergeMethod mm;

            switch ( mrl.Method ) {
                case SumMethod.IDENTIFIER:
                    //TODO deserialize MergedResultLists.Configuration
                    return new SumMethod( tournamentMerger, new SumMethodConfiguration() );
            }

            var msg = $"Unrecognized MergeMethod '{mrl.Method}.'";
            _logger.Error( msg );

            throw new ArgumentException( msg );
        }

        /// <summary>
        /// Method to calculate the merged events for one participant in the tournament. The merged event Score will be 
        /// stored in the re.ResultCofScores dictionary. 
        /// </summary>
        /// <param name="re"></param>
        public abstract void Merge( ResultEvent re );

        /// <summary>
        /// List of non-top level EventNames the merge method adds to each participant's ResultCofScores.
        /// Examples might include "Average", "Sum", "High Score"
        /// </summary>
        public List<string> EventNames { get; protected set; } = new List<string>();

        /// <summary>
        /// The top level event name that this merge methods adds to each participant's ResultCofScores.
        /// This event name SHOULD NOTE be included in .EventNames.
        /// </summary>
        public string TopLevelEventname { get; protected set; } = string.Empty;
    }
}
