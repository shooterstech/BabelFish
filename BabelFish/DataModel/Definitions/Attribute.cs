using System.ComponentModel;
using System.Runtime.Serialization;
using Scopos.BabelFish.DataActors.Specification.Definitions;
using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// An Attribute is a property that helps describes a participant in a match. For example the type of rifle a participant is using 
    /// (e.g. Sporter vs Precision air rifle) or an age group the participant is a member of (e.g. Junior, Open, Senior).
    /// Attributes are highly configurable.  Each Attribute will have one or more fields. Each field may be configured to 
    /// be any value the user types in, a value from a pre-configured list, or a combination.
    /// </summary>
    public class Attribute : Definition {

        /// <summary>
        /// Public constructor
        /// </summary>
        public Attribute() : base() {
            Type = DefinitionType.ATTRIBUTE;
        }

        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context) {
            base.OnDeserializedMethod(context);

            //Designation is not required, but if the user doesn't include it, set it to all values except HIDDEN
            if (designation == null || designation.Count() == 0)
                designation = new List<AttributeDesignation>() { AttributeDesignation.ATHLETE, AttributeDesignation.CLUB, AttributeDesignation.MATCH_OFFICIAL, AttributeDesignation.TEAM, AttributeDesignation.TEAM_OFFICIAL, AttributeDesignation.USER };
        }

        private string displayName = string.Empty;

        /// <summary>
        /// DisplayName is the name displayed to the user for this ATTRIBUTE.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 11 )]
        [G_NS.JsonProperty( Order = 11 )]
        public string DisplayName {
            get {
                if (string.IsNullOrEmpty( displayName )) {
                    try {
                        var hn = this.GetHierarchicalName();
                        return hn.ProperName;
                    } catch (Exception ex) {
                        ; //Do nothing, just let it fall through
                    }
                }

                return displayName;
            }
            set {
                displayName = value;
            }
        }

		private List<AttributeDesignation> designation = new List<AttributeDesignation>();

        /// <summary>
        /// The type of participant, teams, or clubs that this Attribute may be applied to.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 12 )]
        [G_NS.JsonProperty( Order = 12 )]
        public List<AttributeDesignation> Designation {
            get {
                if (designation == null)
                    designation = new List<AttributeDesignation>();

                return designation;
            }
            set {
                if (value.Count == 0) {
                    //In essence the reset condition. 
                    designation.Clear();
                } else {
                    //Avoid adding duplicates
                    designation.Clear();
                    foreach( var d in value ) {
                        if (!designation.Contains( d ))
                            designation.Add( d ); 
                    }
                }
            }
        }

        /// <summary>
        /// The maximum visibility the user can set for the ATTRIBUTE VALUE.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 13 )]
        [G_NS.JsonProperty( Order = 13 )]
        [DefaultValue( VisibilityOption.PUBLIC )]
        public VisibilityOption MaxVisibility { get; set; } = VisibilityOption.PUBLIC;

        /// <summary>
        /// The default visibility for a new ATTRIBUTE VALUE. 
        /// Must be a Privacy value equal to or greater than the MaxVisibility.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 14 )]
        [G_NS.JsonProperty( Order = 14 )]
        [DefaultValue( VisibilityOption.PUBLIC )]
		public VisibilityOption DefaultVisibility { get; set; } = VisibilityOption.PUBLIC;

        /// <summary>
        /// Indicates if multiple field values may be assigned in the resulting ATTRIBUTE VALUEs.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 15 )]
        [G_NS.JsonProperty( Order = 15 )]
        [DefaultValue(false)]
        public bool MultipleValues { get; set; } = false;

        /// <summary>
        /// A list of AttributeFields that describe the make-up of this ATTRIBUTE.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 16 )]
        [G_NS.JsonProperty( Order = 16 )]
        [DefaultValue(null)]
        public List<AttributeFieldBase> Fields { get; set; } = new List<AttributeFieldBase>();

        /// <summary>
        /// Returns True if this Attribute is considered a 'Simple Attribute.'
        /// This is when MultipleValues is False, has only one AttributeField, and that field also has MultipleValues set to False
        /// </summary>
		[G_STJ_SER.JsonIgnore]
        [G_NS.JsonIgnore]
        public bool SimpleAttribute {
            get {
                return !MultipleValues
                    && Fields.Count == 1
                    && !Fields[0].MultipleValues;
            }
		}

		/// <inheritdoc />
		public override async Task<bool> GetMeetsSpecificationAsync() {
			var validation = new IsAttributeValid();

			var meetsSpecification = await validation.IsSatisfiedByAsync( this );
			SpecificationMessages = validation.Messages;

			return meetsSpecification;
		}

	}
}
