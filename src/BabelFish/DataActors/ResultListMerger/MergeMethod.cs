using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.ResultListMerger {

    /// <summary>
    /// A MergeMethod class contains the processing methods to combine (or merge) result lists together. It is the
    /// actor (the class that does the work) to merge scores together.
    /// <para>To create an instance of MergeMethod use the <see cref="FactoryAsync(ResultListMergerEngine, MergedResultList)"/> method.</para>
    /// </summary>
    public abstract class MergeMethod {

        #region Private and Protected Fields
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets the <see cref="MergeConfiguration"/> instance in use.
        /// </summary>
        protected MergeConfiguration _mergeConfiguration { get; private set; }

        #endregion

        #region Constructors, factory methods, and initializations

        /// <summary>
        /// Constructor. Protected so end users can not call it directly. Instead, use the <see cref="FactoryAsync(ResultListMergerEngine, MergedResultList)"/> method.
        /// </summary>
        /// <param name="resultListMergerEngine">The <see cref="ResultListMergerEngine"/> instance in use.</param>
        /// <param name="configuration">The <see cref="MergeConfiguration"/> instance in use.</param>
        protected MergeMethod( ResultListMergerEngine resultListMergerEngine, MergeConfiguration configuration ) {
            this.ResultListMergerEngine = resultListMergerEngine;
            this._mergeConfiguration = configuration;
        }

        /// <summary>
        /// Asynchronous portion of the constructor. Concrete classes do not have to immplemnet this
        /// method if they do not have any asynchronous calls.
        /// </summary>
        /// <returns></returns>
        public virtual async Task InitializeAsync() { }

        #endregion

        #region Data Model Properties
        // Should be empty as this is a Data Actor
        #endregion

        #region Helper Properties
        /// <summary>
        /// Gets the <see cref="ResultListMerger.ResultListMergerEngine"/> instance in use.
        /// </summary>
        public ResultListMergerEngine ResultListMergerEngine { get; private set; }

        /// <summary>
        /// List of non-top level EventNames the merge method adds to each participant's ResultCofScores.
        /// Examples might include "Average", "Sum", "High Score"
        /// </summary>
        public List<string> EventNames { get; protected set; } = new List<string>();

        /// <summary>
        /// The top level event name that this merge methods adds to each participant's ResultCofScores. The format will be similiar to
        /// {MatchId}_{HumanReadableTopLevelEventName}.
        /// <para>This event name SHOULD NOT be included in .EventNames.</para>
        /// </summary>
        public string TopLevelEventname { get; protected set; } = string.Empty;

        /// <summary>
        /// When <see cref="ResultListMergerEngine.AutoGenerateResultListFormat"/> method runs, it asks the MergeMethod for any additional
        /// display columns that should be added to the ResultListFormat. This method returns those additional display columns (if any).
        /// </summary>
        public virtual List<ResultListDisplayColumn> AdditionalDisplayColumns {
            get {
                return new List<ResultListDisplayColumn>();
            }
        }

        /// <summary>
        /// When <see cref="ResultListMergerEngine.AutoGenerateResultListFormat"/> method runs, it asks the MergeMethod for any additional
        /// ResultListFields that should be added to the ResultListFormat. This method returns those additional fields (if any).
        /// </summary>
        public virtual List<ResultListField> AdditionalFields {
            get {
                return new List<ResultListField>();
            }
        }

        /// <summary>
        /// When <see cref="ResultListMergerEngine.AutoGenerateResultListFormat"/> method runs, it asks the MergeMethod for the text
        /// to use for the top level event score. This is that text.
        /// </summary>
        public virtual string TopLevelHeaderText {
            get {
                return "Aggregate";
            }
        }

        /// <summary>
        /// Returns a boolean indicating if this MergeConfiguration can be used to generate a <see cref="RankingRule"/> for ranking the Participants in the Merged
        /// Result List. If true, then <see cref="ResultListMergerEngine.AutoGenerateRankingRule"/>
        /// will ask <see cref="GenerateRankingRule"/> for the RankingRule to use. If false then
        /// <see cref="ResultListMergerEngine.AutoGenerateRankingRule"/> will use its own default logic to generate a RankingRule based on the MergeConfiguration's properties. 
        /// <para>It is expected, in a concrete class implementation, if CanGererateRankingRule is true, then GenerateRankingRuleAsync is implemented
        /// and will not return null.</para>
        /// </summary>
        public bool CanGenerateRankingRule { get; protected set; } = false;

        /// <summary>
        /// Returns a boolean indicating if this MergeConfiguration can be used to generate a <see cref="ResultListFormat"/> for displaying the Participants in the Merged
        /// Result List. If true, then <see cref="ResultListMergerEngine.AutoGenerateResultListFormat"/>
        /// will ask <see cref="GenerateResultListFormat"/> for the ResultListFormat to use. If false then
        /// <see cref="ResultListMergerEngine.AutoGenerateResultListFormat"/> will use its own default logic to generate a ResultListFormat based on the MergeConfiguration's properties. 
        /// <para>It is expected, in a concrete class implementation, if CanGenerateResultListFormat is true, then GenerateResultListFormat is implemented
        /// and will not return null.</para>
        /// </summary>
        public bool CanGenerateResultListFormat { get; protected set; } = false;
        #endregion

        #region Methods

        /// <summary>
        /// Factory method to construct a concrete instance of a MergeMethod. It constructs the correct
        /// concrete class based on the passed in <see cref="MergedResultList"/>
        /// </summary>
        /// <param name="tournamentMerger"></param>
        /// <param name="mrl"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static async Task<MergeMethod> CreateAsync( ResultListMergerEngine resultListMerger ) {

            MergeMethod mm;
            var mrl = resultListMerger.MergedResultList;

            switch (mrl.Method) {
                case MergeMethodType.SUM:
                    mm = new SumMethod( resultListMerger, (SumMethodConfiguration)mrl.Configuration );
                    break;

                case MergeMethodType.AVERAGE:
                    mm = new AverageMethod( resultListMerger, (AverageMethodConfiguration)mrl.Configuration );
                    break;

                case MergeMethodType.REENTRY:
                    mm = new ReentryMethod( resultListMerger, (ReentryMethodConfiguration)mrl.Configuration );
                    break;

                case MergeMethodType.EVENT_COUNT:
                    mm = new ParticipationCountMethod( resultListMerger, (ParticipationCountMethodConfiguration)mrl.Configuration );
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
        public abstract bool Merge( ResultEvent re );

        /// <summary>
        /// Implements a customer RankingRule specific to the concrete MergeMethod. This method is only called if
        /// <see cref="CanGenerateRankingRule"/> is true. If <see cref="CanGenerateRankingRule"/> is false, then the
        /// <see cref="ResultListMergerEngine.AutoGenerateRankingRule"/> method will use its own default logic to generate a
        /// RankingRule based on the MergeConfiguration's properties.
        /// </summary>
        /// <returns></returns>
        public virtual RankingRule GenerateRankingRule() {
            return null;
        }

        /// <summary>
        /// Implements a customer ResultListFormat specific to the concrete MergeMethod. This method is only called if
        /// <see cref="CanGenerateResultListFormat"/> is true. If <see cref="CanGenerateResultListFormat"/> is false, then the
        /// <see cref="ResultListMergerEngine.AutoGenerateResultListFormat"/> method will use its own default logic to generate a
        /// ResultListFormat based on the MergeConfiguration's properties.
        /// </summary>
        /// <returns></returns>
        public virtual ResultListFormat GenerateResultListFormat() {
            return null;
        }

        #endregion
    }
}
