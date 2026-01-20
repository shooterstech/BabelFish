using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.Tournaments {
    public abstract class MergeMethod {

        public TournamentMerger TournamentMerger { get; private set; }

        private static Logger _logger = LogManager.GetCurrentClassLogger();
        protected MergeConfiguration _mergeConfiguration { get; private set; }

        protected MergeMethod( TournamentMerger tournamentMerger, MergeConfiguration configuration ) {
            this.TournamentMerger = tournamentMerger;
            this._mergeConfiguration = configuration;
        }

        /// <summary>
        /// Asynchronous portion of the constructor. Concrete classes do not have to immplemnet this
        /// method if they do not have any asynchronous calls.
        /// </summary>
        /// <returns></returns>
        public virtual async Task InitializeAsync() { }

        public static async Task<MergeMethod> FactoryAsync( TournamentMerger tournamentMerger, MergedResultList mrl ) {

            MergeMethod mm;

            switch (mrl.Method) {
                case SumMethod.IDENTIFIER:
                    mm = new SumMethod( tournamentMerger, (SumMethodConfiguration)mrl.Configuration );
                    break;

                case AverageMethod.IDENTIFIER:
                    mm = new AverageMethod( tournamentMerger, (AverageMethodConfiguration)mrl.Configuration );
                    break;

                case ReentryMethod.IDENTIFIER:
                    mm = new ReentryMethod( tournamentMerger, (ReentryMethodConfiguration)mrl.Configuration );
                    break;

                default:
                    var msg = $"Unrecognized MergeMethod '{mrl.Method}.'";
                    _logger.Error( msg );

                    throw new ArgumentException( msg );
            }

            await mm.InitializeAsync();
            return mm;
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
