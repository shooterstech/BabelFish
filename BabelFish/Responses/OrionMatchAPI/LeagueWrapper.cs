using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {

	/// <summary>
	/// Helper class that creates the added structure in the data model needed for Deserialzing a League object from json.
	/// </summary>
	public class LeagueWrapper : BaseClass {
        public League League = new League();

        public override string ToString() {
            return $"League {League.LeagueName}";
        }
    }

}
