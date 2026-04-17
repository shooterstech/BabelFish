using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.OrionMatch {

    /// <summary>
    /// IComparer used to sort ResultEvents by their OutOfCompetition property, with those
    /// shooting for score (not out of competition) being sorted before those shooting out of competition (for score only).
    /// </summary>
    public class CompareOutOfCompetition : IComparer<IEventScores> {

        public static CompareOutOfCompetition Instance { get; } = new CompareOutOfCompetition();

        public int Compare( IEventScores x, IEventScores y ) {
            if (x.OutOfCompetition == y.OutOfCompetition) {
                return x.Participant?.DisplayName.CompareTo( y.Participant?.DisplayName ?? string.Empty ) ?? 0;
            } else if (x.OutOfCompetition && !y.OutOfCompetition) {
                return 1;
            } else {
                return -1;
            }
        }
    }
}
