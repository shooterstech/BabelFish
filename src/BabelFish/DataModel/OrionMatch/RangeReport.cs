using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    public class RangeReport {

        public RangeReportKind ReportKind { get; set; } = RangeReportKind.MATCH;

        public MatchID? LeagueId { get; set; }

        /// <summary>
        /// League calendar week represented by a league recap report.
        /// </summary>
        public int? Week { get; set; }

        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        public DateTime? StartDate { get; set; }

        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        public DateTime? EndDate { get; set; }

        public MatchID? CanonicalLeagueId { get; set; }

        public string? LeagueName { get; set; }

        public List<string> LeagueTeamNames { get; set; } = new List<string>();

        public string Headline { get; set; } = string.Empty;

        public List<string> Paragraphs { get; set; } = new List<string>();

        public bool Published { get; set; } = false;

        /// <summary>
        /// For callers with email permission, indicates that the match report was sent
        /// or the recap was included in an email. Defaults to false when omitted by the API.
        /// </summary>
        public bool EmailSent {
            get => EmailSentStatus ?? false;
            set => EmailSentStatus = value;
        }

        /// <summary>The email status supplied by the API, when visible to this caller.</summary>
        [G_STJ_SER.JsonIgnore, G_NS.JsonIgnore]
        public bool? EmailSentStatus { get; private set; }

        public string MatchId { get; set; } = string.Empty;

        public int LicenseNumber { get; set; } = 0;

        public string ResultListName { get; set; } = string.Empty;

        public int CourseOfFireId { get; set; } = 1;

        [Obsolete( "MilestoneStrategy is no longer returned by the RangeReporter API." )]
        public string MilestoneStrategy { get; set; } = string.Empty;

        public List<int>? ShotMilestoneCounts { get; set; }

        public int? MilestoneCount { get; set; }

        public int? ExpectedShots { get; set; }

        public ScoreComponent? ScoreComponent { get; set; }

        [Obsolete( "SnapshotOrderBy is no longer returned by the RangeReporter API." )]
        public string SnapshotOrderBy { get; set; } = string.Empty;

        public List<string> UserContext { get; set; } = new List<string>();

        /// <summary>
        /// Tournament IDs supplied as context when generating this report.
        /// </summary>
        public List<string> TournamentContext { get; set; } = new List<string>();

        public string? S3Uri { get; set; }

        public string? Creator { get; set; }

        public int? TokensRemaining { get; set; }

        public bool? AiGenerated { get; set; } = true;

        public bool? DryRun { get; set; }

        public string? FormattedHtml { get; set; }

        public string? GenerationJobId { get; set; }

        public RangeReportStatus GenerationStatus { get; set; } = RangeReportStatus.UNKNOWN;

        public string? GenerationError { get; set; }
    }
}
