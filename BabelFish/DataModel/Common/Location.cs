using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Common {
    /// <summary>
    /// Represents a physical location on the globe. Usually city and state with longitude and latitude.
    /// </summary>
    [Serializable]
    public class Location {

        /// <summary>
        /// Boolean indicatingif the location is truly known. If the value is false, then that data in this 
        /// instance should not be trusted.
        /// </summary>
        public bool IsKnown { get; set; } = true;

        /// <summary>
        /// The name of the City. And empty string means the name of the City is not know.
        /// </summary>
        [DefaultValue( "" )]
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// May either be the full name of the State, or the two letter abbreviation.
        /// And empty string means the name of the State is not know.
        /// </summary>
        [DefaultValue("")]
        public string State { get; set; } = string.Empty;

        /// <summary>
        /// Postal code. Format is a string, and dependent on the country it comes from.
        /// </summary>
        public string PostalCode { get; set; } = string.Empty;

        /// <summary>
        /// May either be the full name of Country, or the three letter abbreviation.
        /// An empty string means the name of the country is not known.
        /// </summary>
        [DefaultValue( "" )]
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// The Longitude coordinates. A value of 0 likely means the coordinates are now known
        /// </summary>
        [DefaultValue( 0 )]
        public double Longitude { get; set; } = 0;

        /// <summary>
        /// The Latitude coordinates. A value of 0 likely means the coordinates are now known
        /// </summary>
        [DefaultValue( 0 )]
        public double Latitude { get; set; } = 0;

        /// <summary>
        /// The Altitude of this location, measured in meters. A value of 0 likely means the altitude is not known.
        /// </summary>
        [DefaultValue( 0 )]
        public double Altitude { get; set; } = 0;

        /// <summary>
        /// If known the location of the caller, this is the distance from the caller, measured in kilometers.
        /// </summary>
        [DefaultValue( 0 )]
        public double Distance { get; set; } = 0;
    }
}
