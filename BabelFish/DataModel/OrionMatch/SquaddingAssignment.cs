using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    /// <summary>
    /// Abstract class representing the complete squadding assignment for one participant (athlete or team).
    /// </summary>
    [Serializable]
    [JsonConverter( typeof( SquaddingAssignmentConverter ) )]
    public abstract class SquaddingAssignment: IDeserializableAbstractClass, IParticipant {

        /*
         * A description of how to describe Inherited / Abstract classes in OpenAPI 3.0 is at https://swagger.io/docs/specification/data-models/inheritance-and-polymorphism/
         */

        public SquaddingAssignment() { }

        public string Range { get; set; } = string.Empty;

        /// <summary>
        /// When multiple Participants are shooting on the same SquaddingAssignment for the same relay, the Particpants will fire with this order. 
        /// </summary>
        public int FiringOrder { get; set; } = 0;

        /// <summary>
        /// The Participant (team or individual) that this SquaddingAssignment is for
        /// </summary>
        public Participant Participant { get; set; } = new Individual();

        /// <summary>
        /// If this is Event uses re-entry, this is the unique reentry tag for this individual. The values of an empty string ("") or "No Reentry" mean the same thing.
        /// </summary>
        public string ReentryTag { get; set; }

        /// <summary>
        /// Implementation of the IDeserializableAbstractClass interface.
        /// To have added control over the Deserialization of abstract classes, in to
        /// Concrete classes, the JSON should include a ConcreteClassId that specifies
        /// the Concrete class.
        /// </summary>
        public int ConcreteClassId { get; set; }

    }
}
