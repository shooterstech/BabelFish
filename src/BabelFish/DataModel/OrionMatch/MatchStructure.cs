using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>New with BabelFish 2.0 / Orion 3.0 DataModel</remarks>
    public class MatchStructure : IGetCourseOfFireDefinition {

        public int CourseOfFireId { get; set; } = 1;

        public string CourseOfFireName { get; set; } = string.Empty;

        public SetName CourseOfFireDef { get; set; } = SetName.Parse( "v1.0:orion:Default" );

        /// <inheritdoc />
        public async Task<CourseOfFire> GetCourseOfFireDefinitionAsync() {
            return await DefinitionCache.GetCourseOfFireDefinitionAsync( CourseOfFireDef );
        }
    }
}
