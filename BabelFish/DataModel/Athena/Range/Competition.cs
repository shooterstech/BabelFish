using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.DataModel.Athena.Range
{

    /// <summary>
    /// Stores information about the current Competition going on.
    /// </summary>
    public class Competition
    {

        /// <summary>
        /// Public constructor
        /// </summary>
        public Competition() { }

        /// <summary>
        /// The name of the current Relay competing. If it is an empty string, then it either
        /// means there is not a Competiiton going on, or the first relay has not started.
        /// </summary>
        public string CurrentRelay { get; set; } = "";

        /// <summary>
        /// The Match ID that is currently going on. An empty string means there are no current matches.
        /// </summary>
        public string CurrentMatchID { get; set; } = "";

        /// <summary>
        /// Sets the CurrentRelay to an empty string, to indicate there is not a competition
        /// going on currently, or the first relay has not been sent to the Athena system.
        /// </summary>
        public void SetNoCurrentCompetition()
        {
            CurrentRelay = "";
            CurrentMatchID = "";
        }

        /// <summary>
        /// Sets CurrentRelay and CurrentMatchID if the matchID is different that the CurrentMatchID
        /// value. 
        /// </summary>
        /// <param name="matchID"></param>
        public void SetCurrentCompetition(string matchID)
        {
            if (CurrentMatchID != matchID)
            {
                this.CurrentMatchID = matchID;
                this.CurrentRelay = "";
            }
        }
    }
}