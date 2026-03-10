using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// A MaytchStructure may either be used as the complete configuration of an existing <see cref="Match"/>
    /// or it may be used as a template to create a new Match.
    /// </summary>
    /// <remarks>New with BabelFish 2.0 / Orion 3.0 DataModel</remarks>
    public class MatchStructure {

        /// <summary>
        /// Public constructor.
        /// </summary>
        public MatchStructure() {

        }

        /// <summary>
        /// <para>Unless you are the deserializer, it is generally best to add new CourseOfFireStructures using the
        /// <see cref="AddCourseOfFireAsync(SetName)"/> method. Ass this method sets a known good value for CourseOfFireId.</para>
        /// </summary>
        public List<CourseOfFireStructure> CoursesOfFire { get; set; } = new List<CourseOfFireStructure>();

        /// <summary>
        /// Attempts to find the CourseOfFireStructure with courseOfFireId. Returns a boolean indicating it's success or lack there of.
        /// </summary>
        /// <param name="courseOfFireId"></param>
        /// <param name="cof"></param>
        /// <returns></returns>
        public bool TryGetCourseOfFireStructure( int courseOfFireId, out CourseOfFireStructure cof ) {
            cof = CoursesOfFire.Find( x => x.CourseOfFireId == courseOfFireId );

            return cof is not null;
        }

        /// <summary>
        /// All participants in a <see cref="Match"/> must have a value for each SharedAttributes, and that value is used within each COF.
        /// </summary>
        public List<AttributeConfiguration> SharedAttributes { get; set; } = new List<AttributeConfiguration>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setName"></param>
        /// <returns>The newly added CoruseOfFire's Id.</returns>
        /// <exception cref="DefinitionNotFoundException">Thrown if the past in SetName is not a known COURSE OF FIRE definition
        /// or it is the DEFUALT.</exception>
        public async Task<int> AddCourseOfFireAsync( SetName setName ) {
            var cof = await CourseOfFireStructure.FactoryAsync( setName );

            var maxId = 0;
            foreach (var existingCof in CoursesOfFire) {
                if (maxId > existingCof.CourseOfFireId) {
                    maxId = existingCof.CourseOfFireId;
                }
            }
            cof.CourseOfFireId = maxId + 1;

            this.CoursesOfFire.Add( cof );

            return cof.CourseOfFireId;
        }

        protected internal async Task FinishInitializationAsync() {
            foreach (var sharedAttribute in this.SharedAttributes) {
                await sharedAttribute.FinishInitializationAsync();
            }

            foreach (var cof in this.CoursesOfFire) {
                await cof.FinishInitializationAsync();
            }
        }
    }
}
