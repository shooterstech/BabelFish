using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Scopos.BabelFish.DataModel.Athena.Range {

    public class RangeState {

        public RangeState() {

        }

        /// <summary>
        /// The Key is the FiringPointNumber, the value is the FirignPointState.
        /// </summary>
        [JsonProperty(Order =1 )]
        public Dictionary< string, FiringPointState> FiringPoints { get; set; }

        /// <summary>
        /// Key is the Display thing name (aka address), value is the current state.
        /// </summary>
        [JsonProperty(Order = 2)]
        public Dictionary<string, DisplayState> Displays { get; set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context) {
            foreach(string fp in FiringPoints.Keys) {
                FiringPoints[fp].FiringPointNumber = fp;
            }

            if (NetworkManager == null) {
                NetworkManager = new NetworkManagerState();
                NetworkManager.Username = "pi";
                NetworkManager.Password = "";
            }

            if (Competition == null) {
                Competition = new Competition();
                Competition.SetNoCurrentCompetition();
            }

            if (Orion == null) {
                Orion = new OrionState();
            }
        }

        /// <summary>
        /// Returns a list of DisplayStateAddresses that are assigned to the past in firing point.
        /// </summary>
        /// <param name="fpName"></param>
        /// <returns></returns>
        public List<string> GetDisplaysForFiringPoint(string firingPointNumber) {
            //This is a bit of a brute force algorithm.
            List<string> displayList = new List<string>();
            foreach( var display in Displays) {
                //display.Key is the Display State Address
                //display.Value is the DisplayState dictionary
                if (display.Value.FiringPoints.Contains(firingPointNumber))
                    displayList.Add(display.Key);
            }

            return displayList;
        }

        public string GetActiveTargetForFiringPoint( string firingPointNumber) {
            try {
                var fpState = FiringPoints[firingPointNumber];
                foreach( var target in fpState.Targets) {
                    if (target.Value.Active)
                        return target.Key;
                }

            } catch (Exception e) {
                ;
            }
            return "";
        }

        public NetworkManagerState NetworkManager { get; set; } = new NetworkManagerState();

        /// <summary>
        /// Information about the Orion Instance that controls this range.
        /// </summary>
        public OrionState Orion { get; set; } = new OrionState();

        /// <summary>
        /// Information about the current Competition going on (if any).
        /// </summary>
        public Competition Competition { get; set; } = new Competition();
    }
}
