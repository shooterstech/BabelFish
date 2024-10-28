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
            copy.RequiredAttributes.AddRange( this.RequiredAttributes );
            foreach( var f in this.Fields )
            copy.Fields.Add( f.Copy() );

            copy.Designation = new List<AttributeDesignation>();
            copy.Designation.Clear();
            copy.Designation.AddRange( this.Designation );

            return copy;
        }

        [JsonProperty(Order = 10)]
        [DefaultValue("")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonProperty(Order = 11)]
        public List<AttributeDesignation> Designation { get; set; } = new List<AttributeDesignation>() { AttributeDesignation.ATHLETE, AttributeDesignation.CLUB, AttributeDesignation.MATCH_OFFICIAL, AttributeDesignation.TEAM, AttributeDesignation.TEAM_OFFICIAL, AttributeDesignation.USER };

        [JsonProperty(Order = 12)]
        [JsonConverter(typeof(StringEnumConverter))]
        public VisibilityOption MaxVisibility { get; set; }

        [JsonProperty( Order = 13 )]
        [DefaultValue( false )]
        public VisibilityOption DefaultVisibility { get; set; } = VisibilityOption.PUBLIC;

        [JsonProperty(Order = 13)]
        [DefaultValue(false)]
        public bool MultipleValues { get; set; } = false;

        [JsonProperty(Order = 14)]
        [DefaultValue(null)]
        public List<string> RequiredAttributes { get; set; } = new List<string>();

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

    }
}
