using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.DataModel.ScoreHistory {

    /// <summary>
    /// Base class for a Score History Response object .... needed so the compiler doesn't scream at us.
    /// </summary>
    [Serializable]
    [JsonConverter( typeof( ScoreHistoryBaseConverter ) )]
    public abstract class ScoreHistoryBase : IDeserializableAbstractClass {

        /*
         * A description of how to describe Inherited / Abstract classes in OpenAPI 3.0 is at https://swagger.io/docs/specification/data-models/inheritance-and-polymorphism/
         */

        /// <summary>
        /// The number of shots fired within this ScoreHistoryEntry
        /// </summary>
        public int NumberOfShots { get; set; } = 0;

        /// <summary>
        /// GUID formatted User ID of the athlete who shot this score.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        [JsonConverter( typeof( ScoposDateOnlyConverter ) )]
        public DateTime StartDate { get; set; }

        [JsonConverter( typeof( ScoposDateOnlyConverter ) )]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Implementation of the IDeserializableAbstractClass interface.
        /// To have added control over the Deserialization of abstract classes, in to
        /// Concrete classes, the JSON should include a ConcreteClassId that specifies
        /// the Concrete class.
        /// </summary>
        public int ConcreteClassId { get; set; }
    }
}
