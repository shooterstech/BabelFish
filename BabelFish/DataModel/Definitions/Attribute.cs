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

namespace BabelFish.DataModel.Definitions {

    public class AttributeWrapper : Dictionary<string,Attribute>
    {
        //public Dictionary<string, Attribute> AttributeDictionary = new Dictionary<string, Attribute>();

        public Attribute Attribute
        {
            get { return this.Values.FirstOrDefault(); }
        }
    }

    [Serializable]
    public class Attribute : Definition {

        //TODO: Convert string constants into an ENUM
        public const string DESIGNATION_ATHLETE = "ATHLETE";
        public const string DESIGNATION_CLUB = "CLUB";
        public const string DESIGNATION_MATCH_OFFICIAL = "MATCH OFFICIAL";
        public const string DESIGNATION_TEAM = "TEAM";
        public const string DESIGNATION_TEAM_OFFICIAL = "TEAM OFFICIAL";
        public const string DESIGNATION_USER = "USER";
        public const string DESIGNATION_HIDDEN = "HIDDEN";

        [JsonConverter(typeof(StringEnumConverter))]
        public enum VisibilityOption { PRIVATE, PUBLIC, PROTECTED };

        public Attribute() : base() {
            Type = DefinitionType.ATTRIBUTE;
        }

        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context) {
            base.OnDeserializedMethod(context);

            if (Fields == null)
                Fields = new List<AttributeField>();

            if (Designation == null)
                Designation = new List<string>();

            //Designation is not required, but if the user doesn't include it, set it to all values except HIDDEN
            if (Designation.Count() == 0)
                Designation = new List<string>() { DESIGNATION_ATHLETE, DESIGNATION_CLUB, DESIGNATION_MATCH_OFFICIAL, DESIGNATION_TEAM, DESIGNATION_TEAM_OFFICIAL, DESIGNATION_USER };


            if (RequiredAttributes == null)
                RequiredAttributes = new List<string>();
        }

        [JsonProperty(Order = 10)]
        [DefaultValue("")]
        public string DisplayName { get; set; }

        [JsonProperty(Order = 11)]
        [DefaultValue(null)]
        public List<string> Designation { get; set; }

        [JsonProperty(Order = 12)]
        [JsonConverter(typeof(StringEnumConverter))]
        public VisibilityOption MaxVisibility { get; set; }

        [JsonProperty(Order = 13)]
        [DefaultValue(false)]
        public bool MultipleValues { get; set; }

        [JsonProperty(Order = 14)]
        [DefaultValue(null)]
        public List<string> RequiredAttributes { get; set; }

        [JsonProperty(Order = 15)]
        [DefaultValue(null)]
        public List<AttributeField> Fields { get; set; }

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
