using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class ResultListDisplayColumn {

        public ResultListDisplayColumn() { }

        public ResultListDisplayColumn(string header, string headerClassDefault, string body, string bodyClassDefault, LinkToOption bodyLinkTo, string footer, string footerClassDefault ) {
            Header = header;
            HeaderClassList.Add( headerClassDefault );
            Body = body;
            BodyClassList.Add( bodyClassDefault );
            BodyLinkTo = bodyLinkTo;
            Footer = footer;
            FooterClassList.Add (footerClassDefault);
        }

        /// <summary>
        /// Text, with out interpolation, to display in the header cell.
        /// </summary>
        public string Header { get; set; } = string.Empty;

        public List<string> HeaderClassList { get; set; } = new List<string>();

        /// <summary>
        /// Text, with interpolation, to display in each cell.
        /// Interpolation fields are defined in the ResultListFormat's Fields section.
        /// </summary>
        public string Body { get; set; } = string.Empty;

        public List<string> BodyClassList { get; set; } = new List<string>();

        public LinkToOption BodyLinkTo { get; set; } = LinkToOption.None;

        /// <summary>
        /// Text, without interpolation, to display in the footer cell.
        /// </summary>
        public string Footer { get; set; } = string.Empty;

        public List<string> FooterClassList { get; set; } = new List<string>();
    }
}
