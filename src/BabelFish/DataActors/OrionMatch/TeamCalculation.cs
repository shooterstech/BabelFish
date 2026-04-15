using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.OrionMatch {
    public abstract class TeamCalculation {

        public TeamCalculation() {
        }

        public TeamCalculationType CalculationMethod { get; protected set; } = TeamCalculationType.SUM;

        /// <summary>
        /// Method to call to calculate the team score for a given ResultEvent. The passed in ResultEvent
        /// is expected to be for a <see cref="Team"/>, with the <see cref="ResultEvent.TeamMembers"/> property populated with the individual team member ResultEvents. The implementation of this method should calculate the team score based on the individual team member scores and the specified <see cref="CalculationMethod"/>.
        /// </summary>
        /// <param name="resultEvent"></param>
        /// <returns></returns>
        public abstract Task CalculateAsync();
    }
}
