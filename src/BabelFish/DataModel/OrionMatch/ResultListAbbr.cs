using System.ComponentModel;
using Scopos.BabelFish.DataModel.Definitions;


namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// Describes the configuration of a ResultList.
    /// </summary>
    /// <remarks>
    /// Visit our Scopos-Labs project to see an example of using GetMatchSearch() to retreive a list of ResultListAbbr.
    /// <seealso href="https://github.com/shooterstech/scopos-labs/blob/master/csharp/Command Line Examples/Match Search API Example/Program.cs" />
    /// </remarks>
    public class ResultListAbbr : IEquatable<ResultListAbbr>, IEqualityComparer<ResultListAbbr> {

        /*
        {
            "Status": "OFFICIAL",
            "ResultName": "Individual - Sporter",
            "ResultListID": "-4606155898983857195",
            "Primary": true,
            "Team" : false,
            "ResultListFormatDef" : "",
            "RankingRuleDef" : "",
            "ScoreConfigName" : "",
            "UserDefinedText" : {},
            "AttributeFilters" : [
                {
                    "Operation" : "EQUATION", //Consistent with ShowWhen
                    "Boolean" : "AND", //Consistent with ShowWhen
                    "Arguments" : [ //Consistent with ShowWhen
                        {
                            "Operation" : "ATTRIBUTE_VALUE",
                            "AttributeValue" : {
                                "AttributeDef": "v1.0:ntparc:Three-Position Air Rifle Type",
                                "Visibility": "PUBLIC",
                                "AttributeValue": {
                                    "Three-Position Air Rifle Type": "Sporter"
                                },
                                "ConcreteClassId": 2
                            }
                        }
                    ]
                }
                                    
            ]
        }
        */

        /// <summary>
        /// Default public constructor
        /// </summary>
        public ResultListAbbr() {

        }

        /// <summary>
        /// The name of the Result List. Will be unique within the Match.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 1 )]
        [G_NS.JsonProperty( Order = 1 )]
        public string ResultName { get; set; } = string.Empty;

        /// <summary>
        /// Unique identifier, within this match, for this Result List.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 2 )]
        [G_NS.JsonProperty( Order = 2 )]
        [Obsolete( "ResultName should be unique within a course of fire with in match." )]
        public string ResultListID { get; set; }

        /// <summary>
        /// Boolean, indicating if this Result List is considered one of the primary (or featured) competition results in the match.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 3 )]
        [G_NS.JsonProperty( Order = 3 )]
        public bool Primary { get; set; }

        /// <summary>
        /// Boolean, indicating if this Result List is for a Team competition.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 4 )]
        [G_NS.JsonProperty( Order = 4 )]
        public bool Team { get; set; }

        /// <summary>
        /// Indicates the completion status of this Result List.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 5 )]
        [G_NS.JsonProperty( Order = 5, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        [DefaultValue( ResultStatus.FUTURE )]
        public ResultStatus Status { get; set; } = ResultStatus.FUTURE;

        /// <summary>
        /// The RESULT LIST FORMAT definition used for this Result List.
        /// </summary>
        [G_NS.JsonProperty( Order = 10 )]
        [G_NS.JsonIgnore] //EKA Note Feb 2026: JsonIgnore is temporary, until we start using this class
        public SetName ResultListFormatDef { get; set; } = SetName.Parse( "v1.0:orion:Default Qualification", true );

        /// <summary>
        /// The RANKING RULE definition used for this Result List.
        /// </summary>
        [G_NS.JsonProperty( Order = 11 )]
        [G_NS.JsonIgnore] //EKA Note Feb 2026: JsonIgnore is temporary, until we start using this class
        public SetName RankingRuleDef { get; set; } = SetName.Parse( "v1.0:orion:Generic Qualification", true );

        /// <summary>
        /// The name of the ScoreConfig, from the SCORE FORMAT COLLECTION used as the default format option.
        /// </summary>
        [G_NS.JsonProperty( Order = 12 )]
        public string ScoreConfigName { get; set; } = string.Empty;

        /// <summary>
        /// An AttributeFilter describes how a This ResultList will be filtered. That is to say, of the
        /// participants who shot the Evente, which of those should be included in this ResultList.
        /// <para>For example, a Result List could show all the Sporter Air Rifle marksmen (excluding
        /// the Precision Air Rifle marksmen).</para>
        /// </summary>
        [G_NS.JsonProperty( Order = 14 )]
        public List<AttributeFilter> AttributeFilters { get; set; } = new List<AttributeFilter>();

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
            StringBuilder sb = new StringBuilder();
            sb.Append( this.ResultName );
            sb.Append( this.Primary );
            sb.Append( this.Team );
            //Choosing not to include Status, as Status does not effect the structure of the ResultList
            sb.Append( this.ResultListFormatDef.ToString() );
            sb.Append( this.RankingRuleDef.ToString() );
            sb.Append( this.ScoreConfigName );
            foreach (var filter in this.AttributeFilters) {
                sb.Append( filter.GetHashCode() );
            }
            //Also choosing not to include UserDefinedText, as this too does nto effect the structure of the ResultList
            return sb.ToString().GetHashCode();
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

    }
}
