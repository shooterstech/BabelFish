namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// Class representing a Participant and their squadding assignment in a Match's event. 
    /// </summary>
    public class Squadding :
        IRLIFItem,
        ICheckSum {

        /// <summary>
        /// Constructor
        /// </summary>
        public Squadding() { }

        /// <summary>
        /// The Participant (could be individual or team).
        /// </summary>
        public Participant Participant { get; set; }

        /// <summary>
        /// the Participant's SquaddingAssignment.
        /// </summary>
        public SquaddingAssignment SquaddingAssignment { get; set; }

        /// <inheritdoc />
        public override string ToString() {
            return $"Squadding for {Participant.DisplayName}: {SquaddingAssignment}";
        }

        /// <inheritdoc />
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public string CheckSum { get; set; }

        /// <inheritdoc />
        public ulong CalculateChecksum() {
            var hash = this.Participant.CalculateChecksum();
            hash ^= this.SquaddingAssignment.CalculateChecksum();

            return hash;
        }
    }
}
