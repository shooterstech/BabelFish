using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.OrionMatch {
    public class ResultListWizard {

        private Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public ResultListWizard( Match match ) {
            Match = match;
        }

        public Match Match { get; private set; }

        public async Task<List<ResultListAbbr>> GenerateAsync( int courseOfFireId ) {
            CourseOfFireStructure? matchStructure = this.Match.MatchStructure.CoursesOfFire.Find( x => x.CourseOfFireId == courseOfFireId );

            if (matchStructure is null)
                throw new ArgumentOutOfRangeException( nameof( courseOfFireId ), $"No MatchStructure found with CourseOfFireId {courseOfFireId}." );

            var courseOfFire = await matchStructure.GetCourseOfFireDefinitionAsync();
            var topLevelEvent = EventComposite.GrowEventTree( courseOfFire );
            var stageEvents = topLevelEvent.GetEvents( EventtType.STAGE );

            //Bin the the global Attributes in this match, and the Attributes in the EventStructure by the GroupByPriority
            //There are six possible levels of grouping attributes. Level 0 is implied to be (and can only be) Individual and Teams.
            var attributeBins = new Dictionary<int, List<AttributeConfiguration>>() {
                { 1, new List<AttributeConfiguration>() },
                { 2, new List<AttributeConfiguration>() },
                { 3, new List<AttributeConfiguration>() }
            };

            foreach (var attributeConfig in this.Match.MatchStructure.SharedAttributes) {
                var attribute = await attributeConfig.GetAttributeDefinitionAsync();
                attributeBins[attribute.GroupByPriority].Add( attributeConfig );
            }

            List<bool> teamEvents = new List<bool>() { false, true };

            foreach (var teamEvent in teamEvents) {
                foreach (var attrConfig1 in attributeBins[0]) {

                    foreach (var attrConfig2 in attributeBins[1]) {
                        foreach (var attrConfig3 in attributeBins[2]) {
                            //Not quite sure how to do this yet
                        }
                    }
                }
            }

        }
    }
}
