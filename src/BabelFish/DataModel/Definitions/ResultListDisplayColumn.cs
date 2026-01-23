using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// Defines one column within a Result List Format table. Which includes the header, body, and footer values.
    /// Also includes logic to dynamtically determine when the column is shown, or not shown.
    /// </summary>
    public class ResultListDisplayColumn : IReconfigurableRulebookObject {

        /// <summary>
        /// Public consructor.
        /// </summary>
        public ResultListDisplayColumn() { }

        [OnDeserialized]
        internal void OnDeserializedMethod( StreamingContext context ) {

            if (ShowWhen == null)
                ShowWhen = ShowWhenVariable.ALWAYS_SHOW.Clone();

        }

        /// <summary>
        /// Text, with out interpolation, to display in the header cell.
        /// </summary>
        [DefaultValue( "" )]
        [JsonProperty( Order = 1 )]
        public string Header { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the text, specific class set, and link to options in each line in this column.
        /// Each item in the list, represents a different line.
        /// <para>NOTE: Most Columsn only define a single line to display. </para>
        /// </summary>
        [JsonProperty( Order = 2 )]
        public List<ResultListCellValue> BodyValues { get; set; } = new List<ResultListCellValue>();

        /// <summary>
        /// Newtonsoft helper method, to determine if .BadyValues should be serialized.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeBodyValues() {
            return true;
        }

        /// <summary>
        /// Specifies the text, specific class set, and link to options in each line in this column's child cells.
        /// Each item in the list, represents a different line.
        /// <para>NOTE: Most Columsn only define a single line to display. </para>
        /// <para>If ChildValues are not specified .BodyValues is used instead.</para>
        /// </summary>
        [JsonProperty( Order = 3 )]
        public List<ResultListCellValue>? ChildValues { get; set; } = new List<ResultListCellValue>();

        /// <summary>
        /// Newtonsoft helper method, to determine if .ChildValues should be serialized.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeChildValues() {
            return ChildValues is not null && ChildValues.Count > 0;
        }

        /// <summary>
        /// Text, without interpolation, to display in the footer cell.
        /// </summary>
        [JsonProperty( Order = 7 )]
        [DefaultValue( "" )]
        public string Footer { get; set; } = string.Empty;

        /// <summary>
        /// Logic to determine when this column should be included (or shown).
        /// <para>Default is to always show the column.</para>
        /// </summary>
        [JsonProperty( Order = 8 )]
        public ShowWhenBase ShowWhen { get; set; } = ShowWhenVariable.ALWAYS_SHOW.Clone();

        public bool ShouldSerializeShowWhen() {

            //Dont serialize ShowWhen if it says to always show
            if (ShowWhen is ShowWhenVariable showWhen && showWhen.Condition == ShowWhenCondition.TRUE) {
                return false;
            }

            return true;
        }

        /// <summary>
        /// List of ClassSet objects that will decorate each cell in this column. 
        /// <para>Each ClassSet includes a name of a CSS class (string) and a ShowWhen object to determine if it should be added the the classes used when displaying the column.</para>
        /// <para>Every cell (each body cell and each child cell) will get decorate with the classes listed in .ClassSet. 
        /// To augment a specific cell, use the .ClassSet property in ResultListCellValue.</para>
        /// </summary>
        /// <remarks> Examples include:
        /// <list type="bullet">
        /// <item>rlf-col-rank</item>
        /// <item>rlf-col-profile</item>
        /// <item>rlf-col-participant</item>
        /// <item>rlf-col-matchinfo</item>
        /// <item>rlf-col-event</item>
        /// <item>rlf-col-stage</item>
        /// <item>rlf-col-series</item>
        /// <item>rlf-col-shot</item>
        /// <item>rlf-col-gap</item>
        /// </list>
        /// </remarks>
        [JsonProperty( Order = 9 )]
        public List<ClassSet> ClassSet { get; set; } = new List<ClassSet>();

        /// <summary>
        /// A Newtonsoft Conditional Property to only serialize ClassSEt when the list has something in it.
        /// https://www.newtonsoft.com/json/help/html/ConditionalProperties.htm
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeClassSet() {
            return (ClassSet != null && ClassSet.Count > 0);
        }

        /// <summary>
        /// Text, with field interpolation, to display accress multiple columns, starting at this column.
        /// <para>Only one ResultListDisplayColumn may have a value for Spanning.</para>
        /// </summary>
        [JsonProperty( Order = 11 )]
        public ResultListCellValue Spanning { get; set; }

        /// <summary>
        /// Logic to determine when the Spanning text is displayed.
        /// <para>In order for the Spanning text to be displayed, both this columns .ShowWhen and .ShowSpanningWhen must evaluate to true.</para>
        /// <para>Default is to always show the Spanning text.</para>
        /// </summary>
        [JsonProperty( Order = 12 )]
        public ShowWhenBase ShowSpanningWhen { get; set; } = ShowWhenVariable.ALWAYS_SHOW.Clone();

        /// <summary>
        /// Newtonsoft.json helper method to determine when the property ShowSpanningWhen is serialized.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeShowSpanningWhen() {

            //Dont serialize ShowWhen if it says to always show
            if (ShowSpanningWhen is ShowWhenVariable showWhen && showWhen.Condition == ShowWhenCondition.TRUE) {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Text, with field interpolation, to display in each cell.
        /// <para>The value of .Body will always be displayed in a body row. If .Child is null, the value of .Body is always displayed in the child rows.</para>
        /// <para>Interpolation fields are defined in the ResultListFormat's Fields section.</para>
        /// </summary>
        [JsonProperty( Order = 90 )]
        [Obsolete( "Use .BodyValues instead." )]
        public string Body { get; set; } = string.Empty;

        /// <summary>
        /// A list of css classes to decorate each cell within this column.
        /// </summary>
        /// <remarks> Examples include:
        /// <list type="bullet">
        /// <item>rlf-col-rank</item>
        /// <item>rlf-col-profile</item>
        /// <item>rlf-col-participant</item>
        /// <item>rlf-col-matchinfo</item>
        /// <item>rlf-col-event</item>
        /// <item>rlf-col-stage</item>
        /// <item>rlf-col-series</item>
        /// <item>rlf-col-shot</item>
        /// <item>rlf-col-gap</item>
        /// </list>
        /// </remarks>
        [JsonProperty( Order = 91 )]
        [Obsolete( "Use .ClassSet instead." )]
        public List<string> ClassList { get; set; } = new List<string>();

        /// <summary>
        /// What, if anything, the text in this cell should link to.
        /// </summary>
        [JsonProperty( Order = 95 )]
        [DefaultValue( LinkToOption.None )]
        [Obsolete( "Use .BodyValues instead." )]
        public LinkToOption BodyLinkTo { get; set; } = LinkToOption.None;

        /// <summary>
        /// Text, with field interpolation, to display in each cell in a child row.
        /// <para>If .Child is null or an empty stirng, the value of .Body is displayed in its place. </para>
        /// <para>Interpolation fields are defined in the ResultListFormat's Fields section.</para>
        /// </summary>
        [JsonProperty( Order = 96 )]
        [DefaultValue( "" )]
        [Obsolete( "Use .ChildValues instead." )]
        public string Child { get; set; } = string.Empty;

        /// <summary>
        /// A Newtonsoft Conditional Property to only serialize ClassList when the list has something in it.
        /// https://www.newtonsoft.com/json/help/html/ConditionalProperties.htm
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeClassList() {
            return (ClassList != null && ClassList.Count > 0);
        }

        /// <inheritdoc/>
        [DefaultValue( "" )]
        [JsonProperty( Order = 99 )]
        public string Comment { get; set; } = string.Empty;

        /// <inheritdoc/>
        public override string ToString() {
            return $"ResultListDisplayColumn {Header}";
        }
    }
}
