using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    public class ClubTeam {

        public ClubTeam() {
            POC = new POC();
            ClubID = "";
        }

        /// <summary>
        /// The name of the club team. 
        /// For example West Potomac High School or Carrol County 4-H
        /// </summary>
        public string ClubName { get; set; }

        public string Mascot { get; set; }

        /// <summary>
        /// Globally unique identifier
        /// </summary>
        public string ClubID { get; set; }

        /// <summary>
        /// Point of contact
        /// </summary>
        public POC POC { get; set; }


        /// <summary>
        /// The GMT time this ClubTeam was last updated
        /// </summary>
        public DateTime LastUpdated { get; set; }

        public string JSONVersion { get; set; }
    }
}
