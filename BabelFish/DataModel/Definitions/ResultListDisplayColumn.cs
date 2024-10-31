﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// Defines one column within a Result List Format table. Which includes the header, body, and footer values.
    /// Also includes logic to dynamtically determine when the column is shown, or not shown.
    /// </summary>
    public class ResultListDisplayColumn : IReconfigurableRulebookObject, ICopy<ResultListDisplayColumn>
    {

        public ResultListDisplayColumn() { }

        /// <inheritdoc />
        public ResultListDisplayColumn Copy() {
            ResultListDisplayColumn rlfdc = new ResultListDisplayColumn();
            rlfdc.Header = this.Header;
            if (this.ClassList != null) {
                foreach (var cl in this.ClassList) {
                    rlfdc.ClassList.Add( cl );
                }
            }
            rlfdc.Body = this.Body;
            rlfdc.Child = this.Child;
            rlfdc.BodyLinkTo = this.BodyLinkTo;
            rlfdc.Footer = this.Footer;
            if (this.HeaderClassList != null) {
                foreach (var cl in this.HeaderClassList) {
                    rlfdc.HeaderClassList.Add( cl );
                }
            }
            if (this.BodyClassList != null) {
                foreach (var cl in this.BodyClassList) {
                    rlfdc.BodyClassList.Add( cl );
                }
            }
            if (this.FooterClassList != null) {
                foreach (var cl in this.FooterClassList) {
                    rlfdc.FooterClassList.Add( cl );
                }
            }
            return rlfdc;
        }

        [OnDeserialized]
        internal void OnDeserializedMethod( StreamingContext context ) {

            if (ShowWhen == null)
                ShowWhen = ShowWhenVariable.ALWAYS_SHOW.Copy();

        }

        /// <summary>
        /// Text, with out interpolation, to display in the header cell.
        /// </summary>
        [DefaultValue( "" )]
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.Ignore )]
        public string Header { get; set; } = string.Empty;

        /// <summary>
        /// A list of css classes to decorate each cell within this column.
        /// </summary>
        /// <remarks> Examples include:
        /// <list type="bullet">
        /// <item>rlf-col-rank</item>
        /// <item>rlf-col-profile</item>
        /// <item>rlf-col-participant</item>
        /// <item>rlf-col-matchinfo</item>
        /// <item>rlf-col-stage</item>
        /// <item>rlf-col-event</item>
        /// <item>rlf-col-shot</item>
        /// <item>rlf-col-gap</item>
        /// </list>
        /// </remarks>
        public List<string> ClassList { get; set; } = new List<string>();

        /// <summary>
        /// Text, with interpolation, to display in each cell.
        /// <para>The value of .Body will always be displayed in a body row. If .Child is null, the value of .Body is always displayed in the child rows.</para>
        /// <para>Interpolation fields are defined in the ResultListFormat's Fields section.</para>
        /// </summary>
        public string Body { get; set; } = string.Empty;

        /// <summary>
        /// Text, with interpolation, to display in each cell in a child row.
        /// <para>If .Child is null, the value of .Body is displayed in its place. An empty string indicates not to display any values in the child rows.</para>
        /// <para>Interpolation fields are defined in the ResultListFormat's Fields section.</para>
        /// </summary>
        [DefaultValue( "" )]
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate )]
        public string Child { get; set; } = string.Empty;

        /// <summary>
        /// What, if anything, the text in this cell should link to.
        /// </summary>
        [DefaultValue( LinkToOption.None )]
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.Ignore )]
        public LinkToOption BodyLinkTo { get; set; } = LinkToOption.None;

        /// <summary>
        /// Logic to determine when this column should be shown.
        /// <para>Default is to always show the column.</para>
        /// </summary>
        public ShowWhenBase ShowWhen { get; set; } = ShowWhenVariable.ALWAYS_SHOW.Copy();

        /// <summary>
        /// Text, without interpolation, to display in the footer cell.
        /// </summary>
        [DefaultValue( "" )]
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.Ignore )]
        public string Footer { get; set; } = string.Empty;


        [Obsolete( "Use .ClassList instead." )]
        public List<string> HeaderClassList { get; set; } = new List<string>();

        [Obsolete( "Use .ClassList instead." )]
        public List<string> BodyClassList { get; set; } = new List<string>();

        [Obsolete( "Use .ClassList instead." )]
        public List<string> FooterClassList { get; set; } = new List<string>();

        /// <inheritdoc/>
        [JsonProperty(Order = 99, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string Comment { get; set; } = string.Empty;
    }
}
