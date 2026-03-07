namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// A MaytchStructure may either be used as the complete configuration of an existing <see cref="Match"/>
    /// or it may be used as a template to create a new Match.
    /// </summary>
    /// <remarks>New with BabelFish 2.0 / Orion 3.0 DataModel</remarks>
    public class MatchStructure {

        public MatchStructure() {

        }

        public List<CourseOfFireStructure> CoursesOfFire { get; set; } = new List<CourseOfFireStructure>();

        /// <summary>
        /// All participants in a <see cref="Match"/> must have a value for each SharedAttributes, and that value is used within each COF.
        /// </summary>
        public List<AttributeConfiguration> SharedAttributes { get; set; } = new List<AttributeConfiguration>();
    }
}
