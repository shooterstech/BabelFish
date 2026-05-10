namespace Scopos.BabelFish.DataModel.Athena {
    /// <summary>
    /// Describes a Penalty that is applied to a shot, athlete, team, etc
    /// </summary>
    [Serializable]
    public class Penalty {

        private float penalty = 0;

        /// <summary>
        /// Public constructor
        /// </summary>
        public Penalty() {
        }

        /// <summary>
        /// Rulebook reference number for this penalty.
        /// </summary>
        public string RuleNumber { get; set; }

        /// <summary>
        /// Human readable description of this penalty.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Number of penalty points to be applied.
        /// Must be a value greather than or equal to 0.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if attempting to set penalties to a value less than zero.</exception>"
        public float PenaltyPoints {
            get { return penalty; }
            set {
                if (value >= 0) {
                    penalty = value;
                } else {
                    throw new ArgumentOutOfRangeException( $"Penalty points must be greather than or equal to zero. Instead received {value}." );
                }
            }
        }

        /// <summary>
        /// GUID Formatted String, the user id of the jury member who approved the Penalty.
        /// </summary>
        public string JuryMember { get; set; }

        /// <summary>
        /// GUID formatted string, the unique ID of the penalty.
        /// </summary>
        public string PenaltyID { get; set; }

        /// <inheritdoc />
        /// <remarks>Choosing not to include CheckSum in the serialized value, as it is not a top level document.</remarks>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public string CheckSum { get; set; }


        /// <inheritdoc />
        public ulong CalculateChecksum() {
            var combined = $"{RuleNumber}|{Description}|{PenaltyPoints}|{JuryMember}|{PenaltyID}";
            return Helpers.Common.Md5ToUlong( combined );
        }
    }
}
