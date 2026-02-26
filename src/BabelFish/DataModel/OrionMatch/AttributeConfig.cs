using System.ComponentModel;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    /// <summary>
    /// Specifies an ATTRIBUTEs (that's correct, this is technically not an AttributeValue) that a Match includes to help describe its Participants.
    /// </summary>
    [Serializable]
    public class AttributeConfig {

        /// <summary>
        /// Public constructor.
        /// </summary>
        public AttributeConfig() {
        }

        public SetName AttributeDef { get; set; } = SetName.Parse( "v1.0:orion:Default" );

        public ConstantFieldValueList AttributeValue { get; set; } = new ConstantFieldValueList();

        /// <summary>
        /// Property storing how broadly this AttributeValue may be shared.
        /// </summary>
        public VisibilityOption Visibility { get; set; }

        /// <summary>
        /// The unique identifier for the CourseOfFire that this Attribute is associated with. A value of 1 or greater specifies the CourseOfFire within the match. A value of 0 indicates
        /// this is a shared Attribute that is used by all Coruses of Fire in the match.
        /// </summary>
        public int CourseOfFireId { get; set; } = 1;

        /// <summary>
        /// If true, then all Participants in the match have the same value for this Attribute. If false, then different Participants in the match may have different values for this Attribute.
        /// </summary>
        [DefaultValue( false )]
        public bool Constant { get; set; } = false;

        // QUESTION: Do we need to add properties to designate if this ATTRIBUTE is for Individuals, Teams, or both? 
    }
}
