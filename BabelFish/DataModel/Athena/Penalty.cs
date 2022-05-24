using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.DataModel.Athena
{
    /// <summary>
    /// Describes a Penalty that is applied to a shot, athlete, team, etc
    /// </summary>
    public class Penalty
    {

        private float penalty = 0;

        /// <summary>
        /// Public constructor
        /// </summary>
        public Penalty()
        {
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
        public float PenaltyPoints
        {
            get { return penalty; }
            set
            {
                if (value >= 0)
                {
                    penalty = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"Penalty points must be greather than or equal to zero. Instead received {value}.");
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
    }
}