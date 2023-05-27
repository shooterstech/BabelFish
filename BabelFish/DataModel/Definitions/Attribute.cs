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

    [Serializable]
    public class Attribute : Definition {


        public Attribute() : base() {
            Type = DefinitionType.ATTRIBUTE;
        }

        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context) {
            base.OnDeserializedMethod(context);

            //Designation is not required, but if the user doesn't include it, set it to all values except HIDDEN
            if (Designation.Count() == 0)
                Designation = new List<AttributeDesignation>() { AttributeDesignation.ATHLETE, AttributeDesignation.CLUB, AttributeDesignation.MATCH_OFFICIAL, AttributeDesignation.TEAM, AttributeDesignation.TEAM_OFFICIAL, AttributeDesignation.USER };
        }

        [JsonProperty(Order = 10)]
        [DefaultValue("")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonProperty(Order = 11)]
        public List<AttributeDesignation> Designation { get; set; } = new List<AttributeDesignation>() { AttributeDesignation.ATHLETE, AttributeDesignation.CLUB, AttributeDesignation.MATCH_OFFICIAL, AttributeDesignation.TEAM, AttributeDesignation.TEAM_OFFICIAL, AttributeDesignation.USER };

        [JsonProperty(Order = 12)]
        [JsonConverter(typeof(StringEnumConverter))]
        public VisibilityOption MaxVisibility { get; set; }

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
