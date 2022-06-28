using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.DataModel.OrionMatch {
    [Serializable]
    public class Location {

        /// <summary>
        /// The name of the City. And empty string means the name of the City is not know.
        /// </summary>
        [DefaultValue("")]
        public string City { get; set; }

        /// <summary>
        /// May either be the full name of the State, or the two letter abbreviation.
        /// And empty string means the name of the State is not know.
        /// </summary>
        [DefaultValue("")]
        public string State { get; set; }

        /// <summary>
        /// May either be the full name of Country, or the three letter abbreviation.
        /// An empty string means the name of the country is not known.
        /// </summary>
        [DefaultValue( "" )]
        public string Country { get; set; }

        /// <summary>
        /// The Longitude coordinates. A value of 0 likely means the coordinates are now known
        /// </summary>
        [DefaultValue( 0 )]
        public double Longitude { get; set; }

        /// <summary>
        /// The Latitude coordinates. A value of 0 likely means the coordinates are now known
        /// </summary>
        [DefaultValue( 0 )]
        public double Latitude { get; set; }

        /// <summary>
        /// The Altitude of this location, measured in meters. A value of 0 likely means the altitude is not known.
        /// </summary>
        [DefaultValue( 0 )]
        public double Altitude { get; set; }
    }
}
