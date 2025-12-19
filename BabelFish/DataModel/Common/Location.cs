using System.ComponentModel;
using Newtonsoft.Json;

namespace Scopos.BabelFish.DataModel.Common {
    /// <summary>
    /// Represents a physical location on the globe. Usually city and state with longitude and latitude.
    /// </summary>
    [Serializable]
    public class Location {

        /// <summary>
        /// The name of the City. And empty string means the name of the City is not know.
        /// </summary>
        [DefaultValue( "" )]
        [G_NS.JsonProperty( Order = 1 )]
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// May either be the full name of the State, or the two letter abbreviation.
        /// And empty string means the name of the State is not know.
        /// <para>State is deprecated. The value of .State is exactly the same as the value of .Region</para>
        /// </summary>
        [DefaultValue("")]
        [G_NS.JsonProperty( Order = 2 )]
        [Obsolete( "Use the more generic Region" )]
        public string State { get; set; } = string.Empty;

        /// <summary>
        /// May either be the full name of the State, or the two letter abbreviation.
        /// And empty string means the name of the State is not know.
        /// </summary>
        [G_NS.JsonProperty( Order = 3 )]
        public string Region {
            get {
                return State;
            }
            set {
                State = value;
            }
        }

        /// <summary>
        /// Postal code. Format is a string, and dependent on the country it comes from.
        /// </summary>
        [G_NS.JsonProperty( Order = 5 )]
        public string PostalCode { get; set; } = string.Empty;

        /// <summary>
        /// May either be the full name of Country, or the three letter abbreviation.
        /// An empty string means the name of the country is not known.
        /// </summary>
        [DefaultValue( "" )]
        [G_NS.JsonProperty( Order = 6 )]
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// The Longitude coordinates. A value of 0 likely means the coordinates are now known
        /// </summary>
        [DefaultValue( 0.00 )]
        [G_NS.JsonProperty( Order = 10 )]
        public float? Longitude { get; set; }

        /// <summary>
        /// The Latitude coordinates. A value of 0 likely means the coordinates are now known
        /// </summary>
        [DefaultValue( 0.00 )]
        [G_NS.JsonProperty( Order = 11 )]
        public float? Latitude { get; set; }

        /// <summary>
        /// The Altitude of this location, measured in meters. A value of 0 likely means the altitude is not known.
        /// </summary>
        [DefaultValue(0.00)]
        [G_NS.JsonProperty( Order = 12 )]
        public float? Altitude { get; set; }

        /// <summary>
        /// If known the location of the caller, this is the distance from the caller, measured in kilometers.
        /// </summary>
        [DefaultValue(0.00)]
        [G_NS.JsonProperty( Order = 15 )]
        public float? Distance { get; set; }

        /// <summary>
        /// Boolean indicatingif the location is truly known. If the value is false, then that data in this 
        /// instance should not be trusted.
        /// </summary>
        [G_NS.JsonProperty( DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, Order = 19 )]
        [DefaultValue( true )]
        public bool IsKnown { get; set; } = true;

        /// <inheritdoc />
        public override string ToString() {
            return StringFormatting.Hometown( this );
        }
    }
}
