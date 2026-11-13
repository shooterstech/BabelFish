using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.DataModel.Newsfeed {
    /// <summary>
    /// A plain-text newsfeed post, including its creator and soft-deletion audit fields.
    /// </summary>
    public class Post : BaseClass {
        /// <summary>Decimal string preserving the full unsigned BIGINT identifier range.</summary>
        public string PostId { get; set; } = string.Empty;

        /// <summary>The owning club, or null for a personal post.</summary>
        public int? LicenseNumber { get; set; }

        /// <summary>The submitted text, including its original whitespace and line endings.</summary>
        public string BodyText { get; set; } = string.Empty;

        /// <summary>UTC time at which the post was created.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>The authenticated creator's user ID.</summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>UTC time of the first deletion, or null while active.</summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>The first deleting user's ID, or null while active.</summary>
        public string? DeletedByUserId { get; set; }
    }
}
