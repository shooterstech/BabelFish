using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.Range {

    /// <summary>
    /// Stores information about the current Competition going on.
    /// </summary>
    public class Competition {

        private string _currentRelay = "";

        [JsonIgnore]
        /// <summary>
        /// Fired when the Current Relay changes
        /// </summary>
        public EventHandler<EventArgs<Competition>> EventCurrentRelayChanged;

        /// <summary>
        /// Public constructor
        /// </summary>
        public Competition() { }

        /// <summary>
        /// The name of the current Relay competing. If it is an empty string, then it either
        /// means there is not a Competiiton going on, or the first relay has not started.
        /// </summary>
        public string CurrentRelay
        {
            get
            {
                return _currentRelay;
            }
            set
            {
                if (_currentRelay != value)
                {
                    _currentRelay = value;
                    EventCurrentRelayChanged?.Invoke(this, new EventArgs<Competition>(this));
                }
            }
        }

        /// <summary>
        /// The Match ID that is currently going on. An empty string means there are no current matches.
        /// </summary>
        public string CurrentMatchID { get; set; } = "";

        /// <summary>
        /// Check if there is currently a match running, if CurrentRelay == "" then False
        /// </summary>
        public bool IsMatchRunning
        {
            get
            {
                return (CurrentRelay != "");
            }
            private set { }
        }

        /// <summary>
        /// Sets the CurrentRelay to an empty string, to indicate there is not a competition
        /// going on currently, or the first relay has not been sent to the Athena system.
        /// </summary>
        public void SetNoCurrentCompetition() {
            CurrentRelay = "";
            CurrentMatchID = "";
        }

        /// <summary>
        /// Sets CurrentRelay and CurrentMatchID if the matchID is different that the CurrentMatchID
        /// value. 
        /// </summary>
        /// <returns>Boolean, true if this was a new match id from previously set. False otherwise. </returns>
        /// <param name="matchID"></param>
        public bool SetCurrentCompetition(string matchID) {
            if (CurrentMatchID != matchID) {
                this.CurrentMatchID = matchID;
                this.CurrentRelay = "";
                return true;
            } else {
                return false;
            }
        }
    }
}
