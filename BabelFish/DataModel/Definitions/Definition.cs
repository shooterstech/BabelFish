using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Scopos.BabelFish.Converters;
using Scopos.BabelFish.DataActors.Specification;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.DataModel.Definitions {

	[JsonConverter( typeof( DefinitionConverter ) )]
	[Serializable]
    public abstract class Definition : SparseDefinition, IReconfigurableRulebookObject {

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
		/// HierarchicalName is namespace:properName
		/// </summary>
		[JsonProperty( Order = 3 )]
        public string HierarchicalName { get; set; } = string.Empty;

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
        [JsonProperty( Order = 7 )]
        public string Owner { get; set; } = string.Empty;

        /// <summary>
        /// The high level shooting discipline that uses this Definition.
        /// </summary>
        [JsonProperty( Order = 8 )]
        [JsonConverter( typeof( StringEnumConverter ) )]
        public DisciplineType Discipline { get; set; }

        /// <summary>
        /// The subdiscipline (under the value of Discipline) to categorize this Definition.
        /// The value of a "Subdiscipline" field may be any text string. However, there is a list of common subdisciplines and tags that should be used as appropriate.
        /// </summary>
        [JsonProperty( Order = 9 )]
        [DefaultValue( "" )]
        public string Subdiscipline { get; set; } = string.Empty;

        /// <summary>
        /// The tag or tags to categorize this Definition with.
        /// The value of a "Tags" field may be any text string. However, there is a list of common subdisciplines and tags that should be used as appropriate.
        /// </summary>
        [JsonProperty( Order = 10 )]
        public List<string> Tags { get; set; } = new List<string>();

		/// <summary>
		/// The Version string of the JSON document.
		/// </summary>
		[JsonProperty( Order = 10, DefaultValueHandling = DefaultValueHandling.Include )]
		[DefaultValue( "2020-03-31" )]
		public string JSONVersion { get; set; } = "2020-03-31";

		/// <inheritdoc/>
		[JsonProperty( Order = 99, DefaultValueHandling = DefaultValueHandling.Ignore )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;

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

        public abstract Task<bool> GetMeetsSpecificationAsync();

        [JsonIgnore]
        public List<string> SpecificationMessages { get; protected set; }

		/// <summary>
		/// Returns the file name for this Definition. It should be stored in a directory named after the definition type.
		/// </summary>
		/// <returns></returns>
		public string GetFileName(bool useDevelopmentVersioning = false) {
            if (! useDevelopmentVersioning) 
                return $"{SetName.ToString().Replace( ':', ' ' )}.json";

            return $"d0.0 {HierarchicalName.ToString().Replace( ':', ' ' )}.json";
        }

        public string SaveToFile( DirectoryInfo definitionDirectory ) {

            string filePath = $"{definitionDirectory.FullName}\\{Type.Description()}\\{GetFileName(true)}";

            DirectoryInfo typeDirectory = new DirectoryInfo( $"{definitionDirectory.FullName}\\{Type.Description()}" );

            if (!typeDirectory.Exists)
                typeDirectory.Create();

			string json = JsonConvert.SerializeObject( this, Formatting.Indented );

            File.WriteAllText( filePath, json );

            return filePath ;
        }
    }
}
