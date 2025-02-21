using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.DataModel.ScoreHistory {

    [Serializable]
    [G_NS.JsonConverter( typeof( G_BF_NS_CONV.ScoreAverageBaseConverter ) )]
    public abstract class ScoreAverageBase : IDeserializableAbstractClass {

        /*
         * A description of how to describe Inherited / Abstract classes in OpenAPI 3.0 is at https://swagger.io/docs/specification/data-models/inheritance-and-polymorphism/
         */

        /// <summary>
        /// GUID formatted User ID of the athlete who shot this score.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        public DateTime StartDate { get; set; }

        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
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
