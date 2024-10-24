using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.ComponentModel.DataAnnotations; //COMMENT OUT FOR .NET Standard 2.0
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Scopos.BabelFish.DataModel;

namespace Scopos.BabelFish.DataModel.Definitions {
    [Serializable]
    public abstract class Definition : BaseClass, IReconfigurableRulebookObject {

        [NonSerialized]
        public List<string> errorList = new List<string>();

        private string commonName = string.Empty;

        /// <summary>
        /// Public constructor for Definition class.
        /// </summary>
        public Definition() {
            Discipline = DisciplineType.NA; //Not all definitions use Discipline. Setting it null so by default it does not get included on JSON
            Discontinued = false;
        }

		/// <summary>
		/// Copies the Definition base class properties into the passed in copy instance.
		/// </summary>
		/// <param name="copy"></param>
		protected void Copy( Definition copy ) {
			copy.CommonName = this.CommonName;
			copy.SetName = this.SetName;
			copy.HierarchicalName = this.HierarchicalName;
			copy.JSONVersion = this.JSONVersion;
			copy.Type = this.Type;
			copy.Version = this.Version;
			copy.Description = this.Description;
			copy.Owner = this.Owner;
			copy.Discipline = this.Discipline;
			copy.Subdiscipline = this.Subdiscipline;
			copy.Discontinued = this.Discontinued;
			copy.Comment = this.Comment;
			copy.Tags.AddRange( this.Tags );
		}

        internal void OnDeserializedMethod(StreamingContext context) {
			//Note, each subclass of Definition will have to call base.OnSerializedMethod

			//Assures that Tags won't be null and as a default will be an empty list.
		}

		/// <summary>
		/// A SetName is a unique identifier for a Defintion file within a definition type. It has three parts, the version number, namespace, and propername.
		/// </summary>
		[JsonProperty( Order = 1 )]
		public string SetName { get; set; } = string.Empty;

		/// <summary>
		/// HierarchicalName is namespace:properName
		/// </summary>
		[JsonProperty(Order = 2)]
        public string HierarchicalName { get; set; } = string.Empty;

		/// <summary>
		/// The Version string of the JSON document
		/// </summary>
		[JsonProperty( Order = 2 )]
		[DefaultValue( "2020-03-31" )]
		public string JSONVersion { get; set; } = string.Empty;

		/// <summary>
		/// The Definition Type
		/// </summary>
		[JsonProperty( Order = 3 )]
		[JsonConverter( typeof( StringEnumConverter ) )]
		public DefinitionType Type { get; set; }

		/// <summary>
		/// The precise version number of this Definition. Note, the version number listed in the SetName is often 
		/// a reference to either the latest Major release. Version always provides both the Major and Minor release numbers and is not a reference.
		/// </summary>
		/// <example>1.5</example>
		[JsonProperty( Order = 4 )]
		public string Version { get; set; } = string.Empty;

		/// <summary>
		/// Returns the Version of the Definition as an object DefinitionVersion.
		/// the string varation is returned with the property .Version
		/// </summary>
		/// <returns></returns>
		/// <exception cref="ArgumentException">Thrown if the Version can not be parsed to format x.y</exception>
		public DefinitionVersion GetDefinitionVersion() {
			return new DefinitionVersion( Version );
		}

		/// <summary>
		/// A human readable short name for this Definition. If no specific value
		/// is given, then the ProperName portion of the SetName is returned instead.
		/// </summary>
		[JsonProperty( Order = 5 )]
		public string CommonName {
			get {
				if (string.IsNullOrEmpty( commonName )) {
					SetName sn;
					if (Scopos.BabelFish.DataModel.Definitions.SetName.TryParse( this.SetName, out sn )) {
						return sn.ProperName;
					}
					//Shouldn't ever really get here, b/c every Definition should/better have a SetName.
					return "Unknown";
				} else {
					return commonName;
				}
			}
			set {
				if (!string.IsNullOrEmpty( value )) {
					commonName = value;
				}
			}
		}

		/// <summary>
		/// A human readable description of this Definition. May be verbose.
		/// </summary>
		[JsonProperty( Order = 6 )]
		public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The Orion license account number of who owns, and is responsible for, this desciption. 
        /// There is often a one to one relationship between the Owner and namespace of a Definition.
        /// </summary>
        /// <example>OrionAcct001234</example>
        [JsonProperty(Order = 7)]
        public string Owner { get; set; } = string.Empty;

		/// <summary>
		/// The high level shooting discipline that uses this Definition.
		/// </summary>
		[JsonProperty(Order = 8)]
        [JsonConverter(typeof(StringEnumConverter))]
        public DisciplineType Discipline { get; set; }

		/// <summary>
		/// The subdiscipline (under the value of Discipline) to categorize this Definition.
		/// The value of a "Subdiscipline" field may be any text string. However, there is a list of common subdisciplines and tags that should be used as appropriate.
		/// </summary>
		[JsonProperty(Order=9)]
        [DefaultValue("")]
        public string Subdiscipline { get; set; } = string.Empty;

		/// <summary>
		/// The tag or tags to categorize this Definition with.
		/// The value of a "Tags" field may be any text string. However, there is a list of common subdisciplines and tags that should be used as appropriate.
		/// </summary>
		[JsonProperty(Order = 10)]
        public List<string> Tags { get; set; } = new List<string>();

        /// <inheritdoc/>
        [JsonProperty( Order = 99, DefaultValueHandling = DefaultValueHandling.Ignore )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// If true, this Definition is no longer in use and should not be referenced.
        /// </summary>
        [JsonProperty( Order = 101 )]
		[DefaultValue( false )]
		public bool Discontinued { get; set; }

		/// <summary>
		/// Returns a SetName object based on the Definitions Version and HierrchcialName
		/// If originalSetName is true, returns the setname as was loaded, usually with v1.0, or v0.0
		/// If false, returns the Version based on the version in the file
		/// </summary>
		/// <returns></returns>
		public SetName GetSetName(bool originalSetName = false) {
            SetName sn;
            if (originalSetName)
                Scopos.BabelFish.DataModel.Definitions.SetName.TryParse(SetName, out sn);
            else
                Scopos.BabelFish.DataModel.Definitions.SetName.TryParse(Version, HierarchicalName, out sn);
            return sn;
        }

        public override string ToString() {
            return $"{Type}: {SetName}";
        }

    }
}
