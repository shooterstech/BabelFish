using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// Defines one column within a Result List Format table. Which includes the header, body, and footer values.
    /// Also includes logic to dynamtically determine when the column is shown, or not shown.
    /// </summary>
    public class ResultListDisplayColumn : IReconfigurableRulebookObject    {

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
        public string Header { get; set; } = string.Empty;

        /// <summary>
        /// Text, with field interpolation, to display in each cell.
        /// <para>The value of .Body will always be displayed in a body row. If .Child is null, the value of .Body is always displayed in the child rows.</para>
        /// <para>Interpolation fields are defined in the ResultListFormat's Fields section.</para>
        /// </summary>
        public string Body { get; set; } = string.Empty;

        /// <summary>
        /// What, if anything, the text in this cell should link to.
        /// </summary>
        [DefaultValue( LinkToOption.None )]
        public LinkToOption BodyLinkTo { get; set; } = LinkToOption.None;

        /// <summary>
        /// Text, with field interpolation, to display in each cell in a child row.
        /// <para>If .Child is null or an empty stirng, the value of .Body is displayed in its place. </para>
        /// <para>Interpolation fields are defined in the ResultListFormat's Fields section.</para>
        /// </summary>
        [DefaultValue( "" )]
        public string Child { get; set; } = string.Empty;

        /// <summary>
        /// Text, without interpolation, to display in the footer cell.
        /// </summary>
        [DefaultValue( "" )]
        public string Footer { get; set; } = string.Empty;

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
        public List<string> ClassList { get; set; } = new List<string>();

        /// <summary>
        /// Logic to determine when this column should be shown.
        /// <para>Default is to always show the column.</para>
        /// </summary>
        public ShowWhenBase ShowWhen { get; set; } = ShowWhenVariable.ALWAYS_SHOW.Clone();


        [Obsolete( "Use .ClassList instead." )]
        public List<string> HeaderClassList { get; set; } = new List<string>();

        [Obsolete( "Use .ClassList instead." )]
        public List<string> BodyClassList { get; set; } = new List<string>();

        [Obsolete( "Use .ClassList instead." )]
        public List<string> FooterClassList { get; set; } = new List<string>();

        /// <inheritdoc/>
        [DefaultValue("")]
        public string Comment { get; set; } = string.Empty;
    }
}
