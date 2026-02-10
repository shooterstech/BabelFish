using System.ComponentModel;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// Describes the content of one cell within a Result List Intermedidate Formatted table. A cell can have
    /// interpolated text, it may contain a link, and it may contain a series of css classes to decorate it. 
    /// </summary>
    public class ResultListCellValue : IReconfigurableRulebookObject {

        /// <summary>
        /// Represents an cell that has no text, no link option, and no css classes.
        /// </summary>
        public static readonly ResultListCellValue EMPTY = new ResultListCellValue();

        /// <summary>
        /// Text, with field interpolation, to display in each cell.
        /// <para>Interpolation fields are defined in the ResultListFormat's Fields section.</para>
        /// </summary>
        [G_NS.JsonProperty( Order = 1 )]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// What, if anything, the text in this cell should link to.
        /// </summary>
        [G_NS.JsonProperty( Order = 3 )]
        [DefaultValue( LinkToOption.None )]
        public LinkToOption LinkTo { get; set; } = LinkToOption.None;


        /// <summary>
        /// List of ClassSet to augment the cell with.
        /// <para>These classes are added to the class list in addition to the .ClassSet specified in the ResultListDisplayColumn instance.</para>.
        /// </summary>
        /// <remarks> Examples include:
        /// <list type="bullet">
        /// <item>rlf-col-rank</item>
        /// <item>rlf-col-profile (deprecated)</item>
        /// <item>rlf-col-participant</item>
        /// <item>rlf-col-matchinfo</item>
        /// <item>rlf-col-squadding</item>
        /// <item>rlf-col-event</item>
        /// <item>rlf-col-stage</item>
        /// <item>rlf-col-series</item>
        /// <item>rlf-col-round</item>
        /// <item>rlf-col-shot</item>
        /// <item>rlf-col-spanning</item>
        /// <item>rlf-col-gap</item>
        /// </list>
        /// </remarks>
        [G_NS.JsonProperty( Order = 5 )]
        public List<ClassSet> ClassSet { get; set; } = new List<ClassSet>();

        /// <summary>
        /// Helper property to determine if the value of this ResultListCellValue is effective empty.
        /// </summary>
        [G_NS.JsonIgnore]
        public bool IsEmpty {
            get {
                return (string.IsNullOrEmpty( this.Text ))
                    && (this.LinkTo == LinkToOption.None)
                    && (this.ClassSet is null || this.ClassSet.Count == 0);
            }
        }

        /// <summary>
        /// A Newtonsoft Conditional Property to only serialize ClassSEt when the list has something in it.
        /// https://www.newtonsoft.com/json/help/html/ConditionalProperties.htm
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeClassSet() {
            return (ClassSet != null && ClassSet.Count > 0);
        }

        /// <inheritdoc/>
        [DefaultValue( "" )]
        [G_NS.JsonProperty( Order = 99 )]
        public string Comment { get; set; } = string.Empty;
    }
}
