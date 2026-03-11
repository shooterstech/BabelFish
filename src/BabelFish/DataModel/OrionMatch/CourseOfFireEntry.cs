namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// Represents the Entry and Squadding information for a single Course of Fire for a Match Participant. 
    /// </summary>
    public abstract class CourseOfFireEntry {
        /*
         [
           {
             "CourseOfFireId" : 1,
             "ResultCofId" : "UUID String",
             "Score" : {} ???
             "Squadding" : {}
            }
          ]
        */

        //What is my concrete class identifier?
        // Kinda think it should be the same as how we distinguished Individual and Team Participants. But that's currently an integer, and we can do better.
        // Maybe just go ahead and change Participant's concrete class identifier to be an enum. 

        public int CourseOfFireId { get; set; } = 0;

        /// <summary>
        /// The entry status for this Course of Fire. Indicates whether the Participant is entered, not entered, or withdrew from the Course of Fire.
        /// </summary>
        public EntryStatus EntryStatus { get; set; } = EntryStatus.NOT_ENTERED;
    }

    public class CourseOfFireEntryIndividual : CourseOfFireEntry {

        public string ResultCofId { get; set; } = string.Empty;

        /*
         * Purposefully not including a PaidStatus, as I am not ready to think through the implications of that, and it is not necessary for the current use cases.
         */

        /*
         * Do I want SquaddingAssiognment on a Team entry? I don't. But how do I deal with both Individual and Team entries? 
         */
        public SquaddingAssignment SquaddingAssignment { get; set; } = new SquaddingAssignmentFiringPoint();
    }

    public class CourseOfFireEntryTeam : CourseOfFireEntry {

        //There is nothing additiional to track for Team entries, as Teams are not squadded (not yet at least). 
        //Team members are tracked as part of the Team Participant
    }
}
