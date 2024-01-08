using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    public class LeagueRecord {

        /// <summary>
        /// Float value from 0.00 to 1.00, indicating the winning percentage of the team.
        /// </summary>
        [DefaultValue(0)]
        public float WinningPercentage { get; set; } = 0;

        /// <summary>
        /// The team's high score through the league. While this property is stored as a 
        /// float, the value could eihter represent integer scoring or decimal scoring. Check
        /// the League's Configuration.ScoreConfigName.
        /// </summary>
        [DefaultValue( 0 )]
        public float HighScore { get; set; } = 0;

        /// <summary>
        /// The number of wins this team has earned.
        /// </summary>
        [DefaultValue( 0 )]
        public int WIN { get; set; } = 0;

        /// <summary>
        /// The number of losses this team has earned.
        /// </summary>
        [DefaultValue( 0 )]
        public int LOSE { get; set; } = 0;

        /// <summary>
        /// The number of did not starts this team has earned.
        /// </summary>
        [DefaultValue( 0 )]
        public int DNS { get; set; } = 0;

        /// <summary>
        /// The number of disqualifications this team has earned.
        /// </summary>
        [DefaultValue( 0 )]
        public int DSQ { get; set; } = 0;

        /// <summary>
        /// The number of scores the team turned in late, that can not be counted towards a WIN.
        /// </summary>
        [DefaultValue( 0 )]
        public int LATE { get; set; } = 0;

        /// <summary>
        /// The total number of games the team has competed in. Does not include games scheduled and not shot, or games shot but not declared.
        /// </summary>
        [DefaultValue( 0 )]
        public int NumberOfGames { get; set; } = 0;

        /// <summary>
        /// The total number of league points the team has earned. Value would be zero if the League is not configured for ranking teams by League Points.
        /// </summary>
        [DefaultValue( 0 )]
        public float LeaguePoints { get; set; } = 0;

        /// <inheritdoc />
        public override string ToString() {
            return $"{WIN} - {LOSE + DNS + DSQ + LATE}";

        }
    }
}
