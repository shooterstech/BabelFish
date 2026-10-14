using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {

    /// <summary>
    /// Represents metadata details for a match, including information about associated tournaments and available HTML
    /// reports.
    /// </summary>
    /// <remarks>This class extends MetaDataResponse to provide additional context about a match, such as its
    /// participation in tournaments and any related HTML reports. Use this type to access supplementary information
    /// when working with match details in the system.</remarks>
    public class MatchDetailMetaData : MetaDataResponse {

        /// <summary>
        /// Public constructor for MatchDetailMetaData. Initializes the Type property to MetaDataResponseType.MATCH to indicate that this metadata is specific to match details.
        /// </summary>
        public MatchDetailMetaData() : base() {
            this.Type = MetaDataResponseType.MATCH;
        }

        /// <summary>
        /// Contains a list of Tournament objects representing the tournaments that this match is a member of.
        /// </summary>
        public List<Tournament> Tournaments { get; set; } = new List<Tournament>();

        /// <summary>
        /// A list of MatchHtmlReports (e.g. pressrelease.html) that exist for this match.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 50 )]
        [G_NS.JsonProperty( Order = 50 )]
        public List<MatchHtmlReport> HtmlReports { get; set; } = new List<MatchHtmlReport>();
    }
}
