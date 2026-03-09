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
        internal new void OnDeserializedMethod( StreamingContext context ) {
            base.OnDeserializedMethod( context );

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
                    foreach (var d in value) {
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
        [DefaultValue( false )]
        public bool MultipleValues { get; set; } = false;

        /// <summary>
        /// GroupByPriority is a hint to the <see cref="ResultListWizard"/> telling it which Attributes to group by first.
        /// <list type="bullet">
        /// <item>A GroupByPriority value of 0 is the highest, and reserved for Individual and Team Result Lists. A value of 0
        /// may not be set on an Attribute.</item>
        /// <item>A GroupByPriority value of 1 is the next highest, is generally used for Attributres that describe equipment classes
        /// (Sporter versus Precision) or disabled categories (e.g. Non-Disabled, SH1, SH2). </item>
        /// <item>A GroupByPriority value of 2 is the default value.</item>
        /// <item>A GroupByPriority value of 3 is the lowest priority.</item>
        /// </list>
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 16 )]
        [G_NS.JsonProperty( Order = 16 )]
        public int GroupByPriority { get; set; } = 2;

        /// <summary>
        /// Each ATTRIBUTE defines a set of fields (formally called an AttributeField). A field can be defined to be one of
        /// many times, for example <see cref="AttributeFieldString">strings</see>, <see cref="AttributeFieldInteger">integers</see>, or
        /// <see cref="AttributeFieldDateTime">dates</see>. It may also be defined to be (for example) a
        /// <see cref="AttributeFieldStringList">list of strings</see>, a <see cref="AttributeFieldIntegerList">list of
        /// integers</see>, or a <see cref="AttributeFieldDateTimeList">list of dates</see>.
        /// <para>Each field must have a unique <see cref="AttributeFieldBase.FieldName"/> within this ATTRIBUTE. Usually, but not always
        /// the FieldName is the same as the <see cref="AttributeFieldBase.DisplayName"/>.</para>
        /// <para>Fields may be defined with a default value, may have validation logic, and may be constrained to a set of values.
        /// For example, you  could define a field to describe the type of rifle a marksment competes with, and specify the only two
        /// values may be Sporter or Precision.</para>
        /// <para>A <see cref="SimpleAttribute">"Simple Attribute"</see> is a special tyep of ATTRIBUTE 
        /// This is when <see cref="MultipleValues" /> is False, has only one <see cref="Fields">field defined</see>,
        /// and that field also has its <see cref="AttributeFieldBase.MultipleValues"/> set to False.</para>
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 17 )]
        [G_NS.JsonProperty( Order = 17 )]
        [DefaultValue( null )]
        public List<AttributeFieldBase> Fields { get; set; } = new List<AttributeFieldBase>();

        /// <summary>
        /// Returns True if this Attribute is considered a 'Simple Attribute.'
        /// This is when <see cref="MultipleValues" /> is False, has only one <see cref="Fields">field defined</see>,
        /// and that field also has its <see cref="AttributeFieldBase.MultipleValues"/> set to False.
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

        /// <summary>
        /// Returns True if this Attribute is considered a 'Simple String Attribute,' which is a further specification of <see cref="SimpleAttribute"/>.
        /// This is when <see cref="MultipleValues" /> is False, has only one <see cref="Fields">field defined</see>,
        /// the field also has its <see cref="AttributeFieldBase.MultipleValues"/> set to False, AND (this is the distinction between a SimpleAttribute)
        /// the one field is of type AttributeFieldString.
        /// </summary>
        public bool SimpleStringAttribute {

            get {
                return !MultipleValues
                    && Fields.Count == 1
                    && !Fields[0].MultipleValues
                    && Fields[0] is AttributeFieldString;
            }
        }

        /// <inheritdoc />
        public override async Task<bool> GetMeetsSpecificationAsync() {
            var validation = new IsAttributeValid();

            var meetsSpecification = await validation.IsSatisfiedByAsync( this );
            SpecificationMessages = validation.Messages;

            return meetsSpecification;
        }

        /// <inheritdoc />
        public override bool SetDefaultValues() {
            base.SetDefaultValues();

            Designation.Add( AttributeDesignation.ATHLETE );
            Designation.Add( AttributeDesignation.USER );
            Designation.Add( AttributeDesignation.TEAM );

            var field = new AttributeFieldString() {
                Comment = "This is an example field",
                DefaultValue = "AAAA",
                Required = false,
                FieldName = "Example Field",
                FieldType = FieldType.CLOSED
            };

            field.Values.Add( new AttributeValueOption<string>() {
                AttributeValueAppellation = "AAAA",
                Value = "AAAA",
                Name = "A Value"
            } );

            field.Values.Add( new AttributeValueOption<string>() {
                AttributeValueAppellation = "BBBB",
                Value = "BBBB",
                Name = "B Value"
            } );

            this.Fields.Add( field );

            return true;
        }

    }
}
