using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.DataModel.ScoreHistory {

    [Serializable]
    [JsonConverter( typeof( ScoreAverageBaseConverter ) )]
    public abstract class ScoreAverageBase : IDeserializableAbstractClass {

        /*
         * A description of how to describe Inherited / Abstract classes in OpenAPI 3.0 is at https://swagger.io/docs/specification/data-models/inheritance-and-polymorphism/
         */

        /// <summary>
        /// GUID formatted User ID of the athlete who shot this score.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

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
