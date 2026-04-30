using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// <see cref="Match"/> are made up of one to many <see cref="CourseOfFireStructure"/>. This class is an abbreviated version of the
    /// CourseOfFireStructure class, used in the MatchAbbr class to provide a lightweight representation of the CourseOfFireStructure associated with a Match.
    /// <para>Not all <see cref="MatchAbbr"/>will include a list of CourseOfFireStructureAbbr. For example, when the reference Match is a <see cref="Tournament"/>
    /// a list of CourseOfFireStructureAbbr will not be included.</para>
    /// </summary>
    public class CourseOfFireStructureAbbr : IGetCourseOfFireDefinition {

        /// <summary>
        /// Backwards pointer to the <see cref="MatchAbbr"/> that this CourseOfFireStructureAbbr is associated with.
        /// This allows for easy access to the Match information when working with a CourseOfFireStructureAbbr.
        /// </summary>
        public MatchAbbr? Match { get; set; } = null;
        /// <summary>
        /// Gets or sets the identifier for the <see cref="CourseOfFireStructure"/> associated with this entity.
        /// </summary>
        public int CourseOfFireId { get; set; } = 1;

        /// <summary>
        /// The SetName of the <see cref="CourseOfFire"/> that this CourseOfFireStructure will fire (or has fired).
        /// This allows for easy access to the CourseOfFire information when working with a CourseOfFireStructureAbbr.
        /// </summary>
        public SetName CourseOfFireRef { get; set; } = SetName.DEFAULT;

        /// <summary>
        /// The date that this <see cref="CourseOfFireStructure"/> starts firing. 
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        public DateTime StartDate { get; set; } = DateTime.Today;

        /// <summary>
        /// The date that this <see cref="CourseOfFireStructure"/> stops firing. 
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        public DateTime EndDate { get; set; } = DateTime.Today;

        /// <summary>
        /// The human readable common name for the <see cref="CourseOfFire"/> that this CourseOfFireStructure will fire (or has fired).
        /// <para>This value is defined by the COURSE OF FIRE.</para>
        /// </summary>
        public string CommonName { get; set; } = string.Empty;

        /// <summary>
        /// The top level discipline category for the <see cref="CourseOfFire"/> that this CourseOfFireStructure will fire (or has fired).
        /// <para>This value is defined by the COURSE OF FIRE.</para>
        /// </summary>
        public DisciplineType Discipline { get; set; } = DisciplineType.RIFLE;

        /// <summary>
        /// The subdiscipline category for the <see cref="CourseOfFire"/> that this CourseOfFireStructure will fire (or has fired).
        /// <para>This value is defined by the COURSE OF FIRE.</para>
        /// </summary>
        public string Subdiscipline { get; set; } = string.Empty;

        /// <summary>
        /// The name of the score configuration for the <see cref="CourseOfFire"/> that this CourseOfFireStructure will fire (or has fired).
        /// <para>This value is one of the options listed in the <see cref="ScoreFormatCollection"/> specified by the COURSE OF FIRE.</para>
        /// </summary>
        public string ScoreConfigName { get; set; } = string.Empty;

        /// <summary>
        /// The name of the target collection for the <see cref="CourseOfFire"/> that this CourseOfFireStructure will fire (or has fired).
        /// <para>This value is one of the options listed in the <see cref="TargetCollection"/> specified by the COURSE OF FIRE.</para>
        /// </summary>
        public string TargetCollectionName { get; set; } = string.Empty;

        /// <summary>
        /// The types of entries that this <see cref="CourseOfFireStructure"/> will accept.
        /// </summary>
        public EntryTypes EntryType { get; set; } = EntryTypes.INDIVIDUAL_AND_TEAM;

        /// <inheritdoc/>
        public Task<CourseOfFire> GetCourseOfFireDefinitionAsync() {
            return DefinitionCache.GetCourseOfFireDefinitionAsync( CourseOfFireRef );
        }
    }
}
