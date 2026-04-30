using System.ComponentModel;
using System.Runtime.Serialization;

namespace Scopos.BabelFish.Responses {

    /// <summary>
    /// A MetaDataResponse is common response object returned by Scopos' REST API call. It is returned off of the "MetaData" property on the response.
    /// Not all API calls return a MetaDataResponse, but when they do, the format of the
    /// MetaDataResponse is customized with a concrete class.
    /// </summary>
    public abstract class MetaDataResponse {

        /// <summary>
        /// Constructor.
        /// </summary>
        public MetaDataResponse() {
        }

        /// <summary>
        /// Concrete class identifier.  This is used to determine the concrete class of a MetaDataResponse when deserializing from JSON. 
        /// </summary>
        public MetaDataResponseType Type { get; protected set; }
    }

    /// <summary>
    /// MetaDataResponseUnknown is a default concrete class for MetaDataResponse objects. It is used when the concrete class of a MetaDataResponse is not known or not implemented.
    /// If your MetaData deserializes into a MetaDataResponseUnknown, it means that the concrete class for that MetaDataResponse has not been implemented in this version of the BabelFish library.
    /// <para>MetaDataResponseUnknown purposefully does not contain any properties.</para>
    /// </summary>
    public class MetaDataResponseUnknown : MetaDataResponse {

        /// <summary>
        /// Constructor
        /// </summary>
        public MetaDataResponseUnknown() : base() {
            Type = MetaDataResponseType.UNKNOWN;
        }
    }

    /// <summary>
    /// Concrete class identifier for <see cref="MetaDataResponse"/> objects. This is used to determine the concrete class of a MetaDataResponse when deserializing from JSON.
    /// </summary>
    public enum MetaDataResponseType {

        /*
         * When adding a new concrete class of MetaDataResponse, make sure to add a new entry in this enum with the same name as the 
         * concrete class. Also make sure to update the MetaDataResponseConverter to handle deserializing into the new concrete class 
         * when the "Type" property in the JSON matches the new enum entry.
         */

        /// <summary>
        /// Represents a <see cref="MetaDataResponseUnknown"/> concrete class.
        /// </summary>
        [Description( "Unknown" )]
        [EnumMember( Value = "Unknown" )]
        UNKNOWN,

        /// <summary>
        /// Represents a <see cref="MatchDetailMetaData"/> concrete class.
        /// </summary>
        [Description( "Match" )]
        [EnumMember( Value = "Match" )]
        MATCH
    }
}
