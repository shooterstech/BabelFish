namespace Scopos.BabelFish.DataModel.OrionMatch {
    public class DeleteTournamentResponse {

        /// <summary>
        /// The Match ID of the deleted tournament.
        /// </summary>
        public MatchID TournamentId { get; set; }

        /// <summary>
        /// Orion license number of the tournament owner.
        /// </summary>
        public int LicenseNumber { get; set; } 
    }
}
