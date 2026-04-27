using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.ResultListMerger {

    /// <summary>
    /// A MergeMethod class contains the processing methods to combine (or merge) result lists together. It is the
    /// actor (the class that does the work) to merge scores together.
    /// <para>To create an instance of MergeMethod use the <see cref="FactoryAsync(ResultListMergerEngine, MergedResultList)"/> method.</para>
    /// </summary>
    public abstract class MergeMethod {

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets the <see cref="ResultListMerger.ResultListMergerEngine"/> instance in use.
        /// </summary>
        public ResultListMergerEngine ResultListMergerEngine { get; private set; }

        /// <summary>
        /// Gets the <see cref="MergeConfiguration"/> instance in use.
        /// </summary>
        protected MergeConfiguration _mergeConfiguration { get; private set; }

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
    }
}
