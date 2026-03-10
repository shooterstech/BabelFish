namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// An AttributeFilter describes how a ResultList should be filtered. That is to say, of the
    /// participants who shot a Course of Fire, which of those should be included in a ResultList.
    /// <para>For example, a Result List could show all the Sporter Air Rifle marksmen (excluding
    /// the Precision Air Rifle marksmen).</para>
    /// </summary>
    [Serializable]
    [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.AttributeFilterConverter ) )]
    [G_NS.JsonConverter( typeof( G_BF_NS_CONV.AttributeFilterConverter ) )]
    public abstract class AttributeFilter {

        public static readonly AttributeFilter DEFAULT = new AttributeFilterNone();

        /// <summary>
        /// Concret class identifier. 
        /// </summary>
        /// <remarks>Concrete class implementations should set this value in their constructors.</remarks>
        [G_NS.JsonProperty( Order = 1, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public AttributeFilterOperation Operation { get; protected set; } = AttributeFilterOperation.ATTRIBUTE_VALUE;

        /// <summary>
        /// Helper method used by <see cref="CourseOfFireStructure.AddResultList(ResultListAbbr)"/>, to update the CourseOfFireId of this AttributeFilter and all of its arguments.
        /// </summary>
        /// <param name="courseOfFireId"></param>
        public abstract void UpdateCourseOfFireId( int courseOfFireId );

        /// <summary>
        /// Returns true if this AttributeFilter is the default filter (i.e. it has no arguments and does not filter out any participants).
        /// </summary>
        /// <returns></returns>
        public virtual bool IsDefault() {
            return Operation == AttributeFilterOperation.NONE;
        }

        /// <summary>
        /// Returns the number of AttributeFilterAtributeValue arguments in total.
        /// <para>Largely a helper property used in unit tests, purposefully not serialized.</para>
        /// </summary>
        [G_NS.JsonIgnore]
        public abstract int Count { get; }

    }

    public class AttributeFilterNone : AttributeFilter {
        public AttributeFilterNone() {
            Operation = AttributeFilterOperation.NONE;
        }

        public override void UpdateCourseOfFireId( int courseOfFireId ) {
            // Do nothing, since this filter has no arguments.
        }

        /// <inheritdoc/>
        [G_NS.JsonIgnore]
        public override int Count => 0;
    }
}
