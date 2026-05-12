
using System.ComponentModel;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    /// <summary>
    /// Abstract class representing the complete squadding assignment for one participant (athlete or team).
    /// </summary>
    public abstract class SquaddingAssignment {

        /*
         * A description of how to describe Inherited / Abstract classes in OpenAPI 3.0 is at https://swagger.io/docs/specification/data-models/inheritance-and-polymorphism/
         */

        public SquaddingAssignment() { }

        /// <summary>
        /// Implementation of the IDeserializableAbstractClass interface.
        /// To have added control over the Deserialization of abstract classes, in to
        /// Concrete classes, the JSON should include a ConcreteClassId that specifies
        /// the Concrete class.
        /// </summary>
        [G_NS.JsonProperty( Order = 1, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public SquaddingAssignmentType SquaddingType { get; protected set; }

        [G_NS.JsonProperty( Order = 2 )]
        [DefaultValue( "" )]
        public string Range { get; set; } = string.Empty;

        /// <summary>
        /// When multiple Participants are shooting on the same SquaddingAssignment for the same relay, the Particpants will fire with this order. 
        /// </summary>
        [G_NS.JsonProperty( Order = 20 )]
        [DefaultValue( 0 )]
        public int FiringOrder { get; set; } = 0;

        /// <summary>
        /// Helper property to indicate whether this SquaddingAssignment is a placeholder assignment, meaning that the Participant has not yet been assigned to a concrete squadding assignment. 
        /// </summary>
        public virtual bool NotYetSquadded { get; }

        /// <summary>
        /// If this is Event uses re-entry, this is the unique reentry tag for this individual. The values of an empty string ("") or "No Reentry" mean the same thing.
        /// </summary>
        [G_NS.JsonProperty( Order = 90 )]
        [Obsolete( "Reentry is no longer supported in Orion 3.0 and BabelFish 2.0. This property is still included for backwards compatibility. Deprecated March 2026." )]
        public string ReentryTag { get; set; }

        public abstract string ToString( bool useAbbreviation );

        public override string ToString() {
            return this.ToString( false );
        }

    }
}
