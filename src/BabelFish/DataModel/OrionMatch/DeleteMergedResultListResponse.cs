namespace Scopos.BabelFish.DataModel.OrionMatch {
    public class DeleteMergedResultListResponse {

        /// <summary>
        /// The Match ID of the tournament that contained the deleted merged result list.
        /// </summary>
        public MatchID MatchId { get; set; }

        /// <summary>
        /// The identifier of the deleted merged result list.
        /// </summary>
        public string MergedId { get; set; } = string.Empty;
    }
}
