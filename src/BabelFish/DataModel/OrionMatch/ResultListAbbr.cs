using System.ComponentModel;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Definitions;


namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// Describes the configuration of a standard ResultList, but does not include the participant scores. This is the abbreviated version of a ResultList,
    /// and is used when retreiving a list of ResultLists for a match, such as with Get Match API call. The full ResultList, with participant scores, can be retreived with the Get Result List API calls.
    /// <para>ResultListAbbr is used only to describe Result Lists from a single Course of Fire. It is not used to describe a Merged Result List.</para>
    /// </summary>
    /// <remarks>
    /// Visit our Scopos-Labs project to see an example of using GetMatchSearch() to retreive a list of ResultListAbbr.
    /// <seealso href="https://github.com/shooterstech/scopos-labs/blob/master/csharp/Command Line Examples/Match Search API Example/Program.cs" />
    /// </remarks>
    public class ResultListAbbr : IEquatable<ResultListAbbr>, IEqualityComparer<ResultListAbbr>, IFinishInitializationAsync {

        /// <summary>
        /// Default public constructor
        /// </summary>
        public ResultListAbbr() {

        }

        /// <summary>
        /// The name of the Result List. Will be unique within a Course of fire within a match. 
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 1 )]
        [G_NS.JsonProperty( Order = 1 )]
        public string ResultName { get; set; } = string.Empty;

        /// <summary>
        /// Each Match may contain multiple Courses of Fire, and each Course of Fire may have multiple Result Lists. This is the unique idtifier to the Course of Fire that this Result List is associated with.
        /// <para>Most Matches only contain one Course of Fire (prior to Orion 3.0 (and BabelFish 2.0) Orion only supported 1 coruse of fire in a match). 1 is the starting index. </para>
        /// </summary>
        /// <remarks>A value of 0 would indicate that this is a Merged Result List. But since ResultListAbbr should not be used to described a Merged Result List, the value should always be >= 1. </remarks>
        [G_STJ_SER.JsonPropertyOrder( 2 )]
        [G_NS.JsonProperty( Order = 2 )]
        public int CourseOfFireId { get; set; } = 1;

        /// <summary>
        /// The Event Name, as defined in the COURSE OF FIRE definiton, that's the top level event for this ResultList.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 3 )]
        [G_NS.JsonProperty( Order = 3 )]
        public string EventName { get; set; }

        /// <summary>
        /// Boolean, indicating if this Result List is considered one of the primary (or featured) competition results in the match.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 5 )]
        [G_NS.JsonProperty( Order = 5 )]
        public bool Primary { get; set; }

        /// <summary>
        /// Boolean, indicating if this Result List is for a Team competition.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 6 )]
        [G_NS.JsonProperty( Order = 6 )]
        public bool Team { get; set; }

        /// <summary>
        /// Indicates the completion status of this Result List.
        /// <para>The Status is not part of a Result List's configuration. It is included for informational purposes only.</para>
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 5 )]
        [G_NS.JsonProperty( Order = 5, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        [DefaultValue( ResultStatus.FUTURE )]
        public ResultStatus Status { get; set; } = ResultStatus.FUTURE;

        /// <summary>
        /// The RESULT LIST FORMAT definition used for this Result List.
        /// </summary>
        [G_NS.JsonProperty( Order = 10 )]
        public SetName ResultListFormatDef { get; set; } = SetName.Parse( "v1.0:orion:Default Qualification", true );

        /// <summary>
        /// The RANKING RULE definition used for this Result List.
        /// </summary>
        [G_NS.JsonProperty( Order = 11 )]
        public SetName RankingRuleDef { get; set; } = SetName.Parse( "v1.0:orion:Generic Qualification", true );

        /// <summary>
        /// The name of the ScoreConfig, from the SCORE FORMAT COLLECTION used as the default format option.
        /// </summary>
        [G_NS.JsonProperty( Order = 12 )]
        public string ScoreConfigName { get; set; } = string.Empty;

        /// <summary>
        /// An AttributeFilter describes how this ResultList will be filtered from the list of all participants. That is to say, of the
        /// participants who shot the Evente, which of those should be included in this ResultList.
        /// <para>For example, a Result List could show all the Sporter Air Rifle marksmen (excluding
        /// the Precision Air Rifle marksmen).</para>
        /// </summary>
        [G_NS.JsonProperty( Order = 14 )]
        public AttributeFilter AttributeFilter { get; set; } = AttributeFilter.DEFAULT;

        /// <summary>
        /// Newtonsoft.json helper method, to determine if AttributeFilters should be serialized.
        /// </summary>
        public bool ShouldSerializeAttributeFilter() {
            //Only serialize if AttributeFilters is not null and is not the default (which is the same as no filter)
            return (AttributeFilter is not null) && !AttributeFilter.IsDefault();
        }

        /// <summary>
        /// On RESULT LIST FORMAT definitions that provided for the option, the user (usually the Match Director) may specify their own
        /// interpolated values for designated fields. These are known as UserDefinedText. There are at most three user defined fields in a
        /// RESULT LIST FORMAT (man definitions do not have any).
        /// <para>The most common example is a demographic spanning text field.</para>
        /// <para>Text values are interpolated with any common field or user defined field. The list is common fields is at
        /// <see href="https://support.scopos.tech/index.html?definition-resultlistfield.html">support.scopos.tech</see></para>
        /// <para>Example text values:</para>
        /// <list type="bullet">
        /// <item>"Competitor Number: {CompetitorNumber}, Hometown: {Hometown}</item>
        /// <item>"Club: {Organization}, Coach: {Coach}</item>
        /// </list>
        /// </summary>
        [G_NS.JsonProperty( Order = 20 )]
        public Dictionary<UserDefinedFieldNames, string> UserDefinedText { get; set; } = new Dictionary<UserDefinedFieldNames, string>() {
            [UserDefinedFieldNames.USER_DEFINED_FIELD_1] = string.Empty,
            [UserDefinedFieldNames.USER_DEFINED_FIELD_2] = string.Empty,
            [UserDefinedFieldNames.USER_DEFINED_FIELD_3] = string.Empty,
        };

        /// <summary>
        /// Newtonsoft.json helper method, to determine if UserDefinedText should be serialized.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeUserDefinedText() {
            //Serialized when UserDefinedText has at least one value that's not an empty string.
            return (UserDefinedText is not null) &&
                ((UserDefinedText.TryGetValue( UserDefinedFieldNames.USER_DEFINED_FIELD_1, out string text1 ) && !string.IsNullOrEmpty( text1 )) ||
                (UserDefinedText.TryGetValue( UserDefinedFieldNames.USER_DEFINED_FIELD_2, out string text2 ) && !string.IsNullOrEmpty( text2 )) ||
                (UserDefinedText.TryGetValue( UserDefinedFieldNames.USER_DEFINED_FIELD_3, out string text3 ) && !string.IsNullOrEmpty( text3 )));
        }

        /// <summary>
        /// Returns a hash code that unique defines the structure of the Result List.</summary>
        /// <inheritdoc />
        public override int GetHashCode() {
            return this.ResultName.GetHashCode()
                ^ (this.Primary ? 32 : 0) | (this.Team ? 1 : 0)
                ^ this.ResultListFormatDef.GetHashCode()
                ^ this.RankingRuleDef.GetHashCode()
                ^ this.ScoreConfigName.GetHashCode()
                ^ this.AttributeFilter.GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString() {
            return this.ResultName;
        }

        /// <inheritdoc />
        public override bool Equals( object obj ) {
            if (obj is ResultListAbbr afe) {
                return Equals( afe );
            }
            return false;
        }

        /// <inheritdoc />
        public bool Equals( ResultListAbbr other ) {
            return this.GetHashCode() == other.GetHashCode();
        }

        /// <inheritdoc />
        public bool Equals( ResultListAbbr x, ResultListAbbr y ) => x.Equals( y );

        /// <inheritdoc />
        public int GetHashCode( ResultListAbbr obj ) => obj.GetHashCode();

        /// <inheritdoc/>
        public async Task FinishInitializationAsync() {
            await this.AttributeFilter.FinishInitializationAsync();
        }

    }
}
