using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
//using System.Text.Json; //COMMENT OUT FOR .NET Standard 2.0
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Scopos.BabelFish.DataActors.Specification.Definitions;
using Scopos.BabelFish.DataModel.AttributeValue;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// An Attribute is a property that helps describes a participant in a match. For example the type of rifle a participant is using (e.g. Sporter vs Precision air rifle) or an age group the participant is a member of (e.g. Junior, Open, Senior).
    /// Attributes are highly configurable.  Each Attribute will have one or more fields. Each field may be configured to be any value the user types in, a value from a pre-configured list, or a combination.
    /// </summary>
    [Serializable]
    public class Attribute : Definition, ICopy<Attribute> {

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
            if (Designation == null || Designation.Count() == 0)
                Designation = new List<AttributeDesignation>() { AttributeDesignation.ATHLETE, AttributeDesignation.CLUB, AttributeDesignation.MATCH_OFFICIAL, AttributeDesignation.TEAM, AttributeDesignation.TEAM_OFFICIAL, AttributeDesignation.USER };
        }

        /// <inheritdoc />
        public Attribute Copy() {
            Attribute copy = new Attribute();
            base.Copy(copy);

            copy.DisplayName = this.DisplayName;
            copy.MaxVisibility = this.MaxVisibility;
            copy.MultipleValues = this.MultipleValues;
            foreach( var f in this.Fields )
            copy.Fields.Add( f.Copy() );

            copy.Designation = new List<AttributeDesignation>();
            copy.Designation.Clear();
            copy.Designation.AddRange( this.Designation );

            return copy;
        }

        private string displayName = string.Empty;

        /// <summary>
        /// DisplayName is the name displayed to the user for this ATTRIBUTE.
        /// </summary>
        [JsonProperty(Order = 10)]
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

                return "";
            }
            set {
                displayName = value;
            }
        }

		private List<AttributeDesignation> designation = new List<AttributeDesignation>();

		/// <summary>
		/// The type of participant, teams, or clubs that this Attribute may be applied to.
		/// </summary>
		[JsonProperty(Order = 11)]
        public List<AttributeDesignation> Designation {
            get {
                if (designation == null)
                    designation = new List<AttributeDesignation>();

                if (designation.Count == 0) {
                    //If no designationare are listed, assume all except hidden.
                    designation.Add( AttributeDesignation.ATHLETE );
					designation.Add( AttributeDesignation.CLUB );
					designation.Add( AttributeDesignation.MATCH_OFFICIAL );
					designation.Add( AttributeDesignation.TEAM );
					designation.Add( AttributeDesignation.TEAM_OFFICIAL );
					designation.Add( AttributeDesignation.USER );
				}

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
		[JsonProperty( Order = 12 )]
        [JsonConverter( typeof( StringEnumConverter ) )]
        [DefaultValue( VisibilityOption.PRIVATE )]
        public VisibilityOption MaxVisibility { get; set; } = VisibilityOption.PRIVATE;

		/// <summary>
		/// The default visibility for a new ATTRIBUTE VALUE. 
        /// Must be a Privacy value equal to or greater than the MaxVisibility.
		/// </summary>
		[JsonProperty( Order = 13 )]
		[DefaultValue( VisibilityOption.PUBLIC )]
		public VisibilityOption DefaultVisibility { get; set; } = VisibilityOption.PUBLIC;

		/// <summary>
		/// Indicates if multiple field values may be assigned in the resulting ATTRIBUTE VALUEs.
		/// </summary>
		[JsonProperty(Order = 13)]
        [DefaultValue(false)]
        public bool MultipleValues { get; set; } = false;

		/// <summary>
		/// A list of AttributeFields that describe the make-up of this ATTRIBUTE.
		/// </summary>
		[JsonProperty(Order = 15)]
        [DefaultValue(null)]
        public List<AttributeField> Fields { get; set; } = new List<AttributeField>();

        /// <summary>
        /// Returns True if this Attribute is considered a 'Simple Attribute.'
        /// This is when MultipleValues is False, has only one AttributeField, and that field also has MultipleValues set to False
        /// </summary>
        [JsonIgnore]
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
