﻿using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {

    /// <summary>
    /// Helper class that creates the added structure in the data model needed for Deserialzing a Match object from json.
    /// </summary>
    public class MatchLocationWrapper : BaseClass
    {
        public List<MatchLocation> MatchLocations = new List<MatchLocation>();
    }
}
