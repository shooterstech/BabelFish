using System;
using System.Collections.Generic;
using System.Text;
using ShootersTech.BabelFish.DataModel.OrionMatch;

namespace ShootersTech.BabelFish.Responses.OrionMatchAPI {

    /// <summary>
    /// Helper class that creates the added structure in the data model needed for Deserialzing a Match object from json.
    /// </summary>
    public class MatchLocationWrapper
    {
        public List<MatchLocation> MatchLocations = new List<MatchLocation>();
    }
}
