using System;
using System.Collections.Generic;
using System.Text;
using ShootersTech.DataModel.OrionMatch;

namespace ShootersTech.Responses.OrionMatchAPI {

    /// <summary>
    /// Helper class that creates the added structure in the data model needed for Deserialzing a Match object from json.
    /// </summary>
    public class MatchWrapper {
        public Match Match = new Match();

        public override string ToString() {
            return $"Match {Match.Name}";
        }
    }

}
