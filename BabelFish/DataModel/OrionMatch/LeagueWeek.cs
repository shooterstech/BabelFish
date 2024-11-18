using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    
    /// <summary>
    /// A LeagueWeek represents one week during a League's season.
    /// The list of LeagueWeeks are currently calculated based on a League's start and end dates.
    /// </summary>
    public class LeagueWeek {

        public string Name { get; set; }

        public DateTime StartOfWeek { get; set; }

        public DateTime EndOfWeek { get; set; }

        public override string ToString() {
            return $"{Name}: {StringFormatting.SpanOfDates( StartOfWeek, EndOfWeek )}";
        }
    }

    public static class LeagueWeekExtensions {

        public static LeagueWeek Default(this List<LeagueWeek> leagueWeeks) {
            return  leagueWeeks.Default( DateTime.Today );
        }

        public static LeagueWeek Default( this List<LeagueWeek> leagueWeeks, DateTime date ) {
            if ( date < leagueWeeks.First().StartOfWeek )
                return leagueWeeks.First();

            foreach( var leagueWeek in leagueWeeks ) {
                if ( date >= leagueWeek.StartOfWeek && date <= leagueWeek.EndOfWeek  ) {
                    return leagueWeek;
                }
            }

            if (date > leagueWeeks.Last().EndOfWeek)
                return leagueWeeks.Last();

            //We really should not ever get here, and not sure what to do if we did.
            return leagueWeeks.First();

        }
    }
}
