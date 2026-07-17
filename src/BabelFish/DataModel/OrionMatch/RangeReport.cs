namespace Scopos.BabelFish.DataModel.OrionMatch {
    public class RangeReport {

        public string Headline { get; set; } = string.Empty;

        public List<string> Paragraphs { get; set; } = new List<string>();

        public bool Published { get; set; } = false;

        public string MatchId { get; set; } = string.Empty;

        public int LicenseNumber { get; set; } = 0;

        public string ResultListName { get; set; } = string.Empty;

        public int CourseOfFireId { get; set; } = 1;

        public string MilestoneStrategy { get; set; } = string.Empty;

        public List<int>? ShotMilestoneCounts { get; set; }

        public int? ExpectedShots { get; set; }

        public string SnapshotOrderBy { get; set; } = string.Empty;

        public List<string> UserContext { get; set; } = new List<string>();
        public string? S3Uri { get; set; }

        public string? Creator { get; set; }

        public int? TokensRemaining { get; set; }

        public bool? AiGenerated { get; set; }

        public string FormattedHtml { get; set; } = string.Empty;
    }
}
