using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Helpers;

namespace BabelFish.DataModel.OrionMatch {
    
    /// <summary>
    /// A LeagueWeek represents one week during a League's season.
    /// The list of LeagueWeeks are currently calculated based on a League's start and end dates.
    /// </summary>
    public class LeagueWeek {

        public string Name { get; set; }

        public DateTime StartOfWeek { get; set; }

        public DateTime EndOfWeek { get; set; }

        public override string ToString() {
            return $"Week {Name}: {StringFormatting.SpanOfDates( StartOfWeek, EndOfWeek )}";
        }
    }
}
