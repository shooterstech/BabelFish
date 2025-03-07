using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;
using BabelFish.DataModel.Definitions;
using Newtonsoft.Json;
using Scopos.BabelFish.Helpers;

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
        internal void OnDeserializedMethod(StreamingContext context) {

            if (ShowWhen == null)
                ShowWhen = ShowWhenVariable.ALWAYS_SHOW.Clone();

        }

        /// <summary>
        /// Text, with out interpolation, to display in the header cell.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty( Order = 1 )]
        public string Header { get; set; } = string.Empty;

        /// <summary>
        /// Text, with field interpolation, to display in each cell.
        /// <para>The value of .Body will always be displayed in a body row. If .Child is null, the value of .Body is always displayed in the child rows.</para>
        /// <para>Interpolation fields are defined in the ResultListFormat's Fields section.</para>
        /// </summary>
        [JsonProperty( Order = 2 )]
        public string Body { get; set; } = string.Empty;

        /// <summary>
        /// What, if anything, the text in this cell should link to.
        /// </summary>
        [JsonProperty( Order = 3 )]
        [DefaultValue(LinkToOption.None)]
        public LinkToOption BodyLinkTo { get; set; } = LinkToOption.None;

        /// <summary>
        /// Text, with field interpolation, to display in each cell in a child row.
        /// <para>If .Child is null or an empty stirng, the value of .Body is displayed in its place. </para>
        /// <para>Interpolation fields are defined in the ResultListFormat's Fields section.</para>
        /// </summary>
        [JsonProperty( Order = 4 )]
        [DefaultValue("")]
        public string Child { get; set; } = string.Empty;

        /// <summary>
        /// Text, without interpolation, to display in the footer cell.
        /// </summary>
        [JsonProperty( Order = 5 )]
        [DefaultValue("")]
        public string Footer { get; set; } = string.Empty;

        /// <summary>
        /// Logic to determine when this column should be shown.
        /// <para>Default is to always show the column.</para>
        /// </summary>
        [JsonProperty( Order = 6 )]
        public ShowWhenBase ShowWhen { get; set; } = ShowWhenVariable.ALWAYS_SHOW.Clone();

        /// <summary>
        /// List of ClassSet objects, each holds a name of a CSS class (string) and a ShowWhen object to determine if it should be added the the classes used when displaying the column.
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
        [JsonProperty( Order = 7 )]
        public List<ClassSet> ClassSet { get; set; } = new List<ClassSet>();

        public void CombineClassListSet()
        {
            if (ClassList is null || ClassList.Count == 0) return;

            if (ClassSet is null || ClassSet.Count == 0)
            {
                //true is classSet list and Convert to class set
                foreach (var cl in ClassList)
                {
                    var cs = new ClassSet();
                    cs.Name = cl;
                    cs.ShowWhen = ShowWhenVariable.ALWAYS_SHOW.Clone();
                    ClassSet.Add(cs);
                }
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
        [JsonProperty( Order = 8 )]
        [Obsolete("Use .ClassSet instead.")]
        public List<string> ClassList { get; set; } = new List<string>();

        /// <summary>
        /// A Newtonsoft Conditional Property to only serialize ClassList when the list has something in it.
        /// https://www.newtonsoft.com/json/help/html/ConditionalProperties.htm
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeClassList() {
            return (ClassList != null && ClassList.Count > 0);
        }

        /// <inheritdoc/>
        [DefaultValue("")]
        [JsonProperty( Order = 99  )]
        public string Comment { get; set; } = string.Empty;

        /// <inheritdoc/>
        public override string ToString() {
            return $"ResultListDisplayColumn {Header}";
        }
    }
}
