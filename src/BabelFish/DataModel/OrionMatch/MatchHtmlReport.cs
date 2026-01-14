using System.Text.Json.Serialization;
using HtmlAgilityPack;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Scopos.BabelFish.DataModel.OrionMatch {
#pragma warning restore IDE0130 // Namespace does not match folder structure

    /// <summary>
    /// Represents a html report that would be associated with a match. Such as the match's
    /// post-competition press release.
    /// </summary>
    public class MatchHtmlReport {

        /*
         * EKA NOTE Jan 2026
         * 
         * I have choosed to make ReportType a string and not an enum. While 
         * it is true there is a common set of values for ReportType (e.g. "pressrelease" for
         * most Matches), in some cases (notable leagues) the values for ReportType are dynamic
         * ("Week 1-recap", "Week 2-recap", ...). As such, it just doesn't make sense to 
         * make ReportType a enum.
         * 
         * This class should likely be worked in with (not sure how yet) with the OrionMatchAPI GetPressReleaseGenerationAuthetication() API call.
         * As that method generates a press release, and this method stores where the pressrelease is at. 
         */

        /// <summary>
        /// Default public constructor
        /// </summary>
        public MatchHtmlReport() { }

        /// <summary>
        /// Factory method that generates a MatchHtmlReport for the common (post-competitino) pressrelease report.
        /// Generates the instance based on the passed in MatchId, and assumes storage of report on cdn.scopos.tech.
        /// <para>Caller should check that the report exists, by calling .IsValidUriAsync() before adding the instance
        /// to the Match object's HtmlReports property.</para>
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="reportType">The type of report to generate. The most common value is 'pressrelease'</param>
        /// <exception cref="ArgumentNullException">Thrown when reportType is null or an empty string.</exception>"
        public static async Task<MatchHtmlReport> FactoryAsync( MatchID matchId, string reportType = "pressrelease" ) {
            if (string.IsNullOrEmpty( reportType )) {
                throw new ArgumentNullException( "The argument reportType may not be null or an empty string. The most common value is 'pressrelease'." );
            }

            MatchHtmlReport report = CreateInstance();
            report.ReportType = "pressrelease";
            report.Uri = $"https://cdn.scopos.tech/matches/{matchId.GetParentMatchID()}/{reportType}.html";

            if (await report.IsValidUriAsync()) {
                try {
                    var content = await report.GetHtmlReportAsync();
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml( content );
                    var firstH1 = htmlDoc.DocumentNode.SelectSingleNode( "//h1" );
                    report.Title = firstH1.InnerText.Trim();
                } catch (Exception ex) {
                    MatchHtmlReport._logger.Error( ex, $"Could not read the title of the press release found at {report.Uri}" );
                    report.Title = string.Empty;
                }
            } else {
                MatchHtmlReport._logger.Warn( $"Could not learn the title of the press release found at {report.Uri}, as that URI was not found." );
                report.Title = string.Empty;
            }

            return report;
        }

        protected static MatchHtmlReport CreateInstance() {
            return new MatchHtmlReport();
        }


        /// <summary>
        /// The type of match html report. The common values are as follows. In parenthesis are the types of matches you usually find them in.:
        /// <list type="bullet">
        /// <item>pressrelease (Local and Virtual Matches)</item>
        /// <item>home (Leagues)</item>
        /// <item>athltes (Leagues)</item>
        /// <item>documents (Leagues)</item>
        /// <item>schedule (Leagues)</item>
        /// <item>standings (Leagues)</item>
        /// <item>teams (Leagues)</item>
        /// <item>Week n-recap (Leagues, n is integer 1..a lot)</item>
        /// </list>
        /// </summary>
        [JsonPropertyOrder( 1 )]
        public string ReportType { get; set; } = "pressrelease";

        /// <summary>
        /// The title of this MatchHtmlReport.
        /// </summary>
        /// <example>Rams post season-high 2,356.9; Sookhoo closes strong to win Game 4 individual as Fall slate ends</example>
        [JsonPropertyOrder( 2 )]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// The URI where to find this Match Html Report.
        /// </summary>
        [JsonPropertyOrder( 3 )]
        public string Uri { get; set; } = string.Empty;

        // --- Cache fields ---
        protected bool _cachedResponseSuccess = false;
        protected string _cachedReportContent = string.Empty; //Would be an empty string if unable to retreive the content.
        protected DateTime _cacheExpiration = DateTime.MinValue;

        protected static readonly HttpClient _http = new HttpClient();
        protected static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Returns true if the URI responds with HTTP 200.
        /// Result is cached for 10 minutes.
        /// </summary>
        /// <remarks>This method is marked virtual, so the class can be overwritten for unit testing, without making acrual http calls.</remarks>
        public virtual async Task<bool> IsValidUriAsync() {
            // If cached and not expired, return cached value
            if ((DateTime.UtcNow - _cacheExpiration).TotalMinutes < 10.0d)
                return (bool)_cachedResponseSuccess;

            // Otherwise, recheck
            ClearCache();

            try {
                using (var response = await _http.GetAsync( Uri, HttpCompletionOption.ResponseHeadersRead )) {
                    if (response.IsSuccessStatusCode) {  // true for 200–299
                        _cachedResponseSuccess = response.IsSuccessStatusCode;
                        _cachedReportContent = await response.Content.ReadAsStringAsync();
                    }
                }
            } catch (Exception ex) {
                _logger.Error( ex, $"Unable to read the Uri {Uri}." );
            }

            // Cache result for 10 minutes
            _cacheExpiration = DateTime.UtcNow;

            return _cachedResponseSuccess;
        }

        /// <summary>
        /// Returns the html content, as a string, from the .Uri address.
        /// <para>If the .Uri is invalid (doesn't return a 200 success message), then this
        /// method returns an empty string. Check .IsValidUriAsync() to test if the Uri
        /// is valid first.</para>
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetHtmlReportAsync() {

            // If not cached or expired, recheck
            if ((DateTime.UtcNow - _cacheExpiration).TotalMinutes > 10.0d)
                await IsValidUriAsync();

            return _cachedReportContent;
        }

        /// <summary>
        /// Clears the response cache. User should call this if they recently assked the REST API
        /// to generate a match report. 
        /// </summary>
        public void ClearCache() {
            _cachedResponseSuccess = false;
            _cachedReportContent = string.Empty;
        }

    }
}
