namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// Abstract class representing the Entry and Squadding information for a single Course of Fire for a Match Participant.
    /// The concrete classes are <see cref="CourseOfFireEntryIndividual"/> and <see cref="CourseOfFireEntryTeam"/>, which represent
    /// the specific information for an Individual or a Team entry, respectively.
    /// <para>Withiin a <see cref="Match"/> there are multiple <see cref="CourseOfFireStructure">Courses of Fire</see>, and each Participant may be entered in some or all of the Course of Fire.
    /// This class represents the information about a single Course of Fire for a single Participant, including whether they are entered
    /// in the Course of Fire, and if so, their Squadding information for that Course of Fire.</para>
    /// <para> The preferred method for creating a new entry is to use the <see cref="MatchParticipant.CreateEntry(int)"/> method.
    /// Which will also set the backwards pointer <see cref="MatchParticipant"/></para>
    /// </summary>
    public abstract class CourseOfFireEntry {

        [G_NS.JsonProperty( Order = 1, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public int CourseOfFireId { get; set; } = 0;

        /// <summary>
        /// Concrete class identifying the type of Participant.
        /// This is used during deserialization to determine which concrete class to deserialize to, either a <see cref="CourseOfFireEntryIndividual"/> or a <see cref="CourseOfFireEntryTeam"/>.
        /// </summary>
        [G_NS.JsonProperty( Order = 3, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public ParticipantType ParticipantType { get; set; } = ParticipantType.INDIVIDUAL;

        /// <summary>
        /// The entry status for this Course of Fire. Indicates whether the Participant is entered, not entered, or withdrew from the Course of Fire.
        /// </summary>
        [G_NS.JsonProperty( Order = 5, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public EntryStatus EntryStatus { get; set; } = EntryStatus.NOT_ENTERED;

        /// <summary>
        /// The list of <see cref="RemarkAction"/> this Participant has for this Course of Fire. This can include things like DNS, DSQ, or in a Final AT RISK.
        /// </summary>
        public RemarkList RemarkList { get; set; } = new RemarkList();

        /// <summary>
        /// Backwards pointer to the MatchParticipant that owns this CourseOfFireEntry. This is set when the CourseOfFireEntry is created using the <see cref="MatchParticipant.CreateEntry(int)"/> method, and should not be set manually.
        /// </summary>
        [G_NS.JsonIgnore]
        public MatchParticipant? MatchParticipant { get; internal set; } = null;

        public bool ShouldSerializeRemarkList() {
            return RemarkList != null && RemarkList.Count > 0;
        }
    }

    public class CourseOfFireEntryIndividual : CourseOfFireEntry {

        /// <summary>
        /// Initializes a new instance of the CourseOfFireEntryIndividual class, setting the participant type to
        /// individual and generating a new unique identifier for the result.
        /// </summary>
        public CourseOfFireEntryIndividual() {
            ParticipantType = ParticipantType.INDIVIDUAL;
            ResultCofId = System.Guid.NewGuid().ToString();
        }

        /// <summary>
        /// The unique identifier for the Result Course of Fire.
        /// </summary>
        public string ResultCofId { get; set; } = string.Empty;

        /*
         * Purposefully not including a PaidStatus, as I am not ready to think through the implications of that, and it is not necessary for the current use cases.
         */

        /// <summary>
        /// Gets or sets the squadding assignment that defines where the Participant will shoot for this Course of Fire.
        /// </summary>
        public SquaddingAssignment SquaddingAssignment { get; set; } = new SquaddingAssignmentFiringPoint();
    }

    /// <summary>
    /// Represents a team entry for a course of fire within a Match, with the participant type set to TEAM.
    /// </summary>
    public class CourseOfFireEntryTeam : CourseOfFireEntry {

        /// <summary>
        /// Initializes a new instance of the CourseOfFireEntryTeam class with the participant type set to TEAM.
        /// </summary>
        public CourseOfFireEntryTeam() {
            ParticipantType = ParticipantType.TEAM;
        }

        //There is nothing additiional to track for Team entries, as Teams are not squadded (not yet at least). 
        //Team members are tracked as part of the Team Participant
    }
}
