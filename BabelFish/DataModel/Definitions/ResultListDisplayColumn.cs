using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class ResultListDisplayColumn : IReconfigurableRulebookObject, ICopy<ResultListDisplayColumn>
    {

        public ResultListDisplayColumn() { }

        /// <summary>
        /// Text, with out interpolation, to display in the header cell.
        /// </summary>
        [DefaultValue( "" )]
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.Ignore )]
        public string Header { get; set; } = string.Empty;

        /// <summary>
        /// A list of css classes to decorate each cell within this column.
        /// </summary>
        public List<string> ClassList { get; set; } = new List<string>();

        /// <summary>
        /// Text, with interpolation, to display in each cell.
        /// Interpolation fields are defined in the ResultListFormat's Fields section.
        /// </summary>
        public string Body { get; set; } = string.Empty;

        /// <summary>
        /// What, if anything, the text in this cell should link to.
        /// </summary>
        [DefaultValue( LinkToOption.None )]
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.Ignore )]
        public LinkToOption BodyLinkTo { get; set; } = LinkToOption.None;

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

        public ResultListDisplayColumn Copy()
        {
            ResultListDisplayColumn rlfdc = new ResultListDisplayColumn();
            rlfdc.Header = this.Header;
            if (this.ClassList != null)
            {
                foreach (var cl in this.ClassList)
                {
                    rlfdc.ClassList.Add(cl);
                }
            }
            rlfdc.Body = this.Body;
            rlfdc.BodyLinkTo = this.BodyLinkTo;
            rlfdc.Footer = this.Footer;
            if (this.HeaderClassList != null)
            {
                foreach (var cl in this.HeaderClassList)
                {
                    rlfdc.HeaderClassList.Add(cl);
                }
            }
            if (this.BodyClassList != null)
            {
                foreach (var cl in this.BodyClassList)
                {
                    rlfdc.BodyClassList.Add(cl);
                }
            }
            if (this.FooterClassList != null)
            {
                foreach (var cl in this.FooterClassList)
                {
                    rlfdc.FooterClassList.Add(cl);
                }
            }
            return rlfdc;
        }

        /// <inheritdoc/>
        [JsonProperty(Order = 99, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string Comment { get; set; } = string.Empty;
    }
}
