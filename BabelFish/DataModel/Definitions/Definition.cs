using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.Requests.DefinitionAPI;

namespace Scopos.BabelFish.DataModel.Definitions {

	[Serializable]
    [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DefinitionConverter ) )]
    public abstract class Definition : SparseDefinition, IReconfigurableRulebookObject {

        private string commonName = string.Empty;

        /// <summary>
        /// Public constructor for Definition class.
        /// </summary>
        public Definition() {
            Discipline = DisciplineType.NA; //Not all definitions use Discipline. Setting it null so by default it does not get included on JSON
            Discontinued = false;
        }

        internal void OnDeserializedMethod(StreamingContext context) {
			//Note, each subclass of Definition will have to call base.OnSerializedMethod

			//Assures that Tags won't be null and as a default will be an empty list.
		}

		/// <summary>
		/// HierarchicalName is namespace:properName
		/// </summary>
		[G_STJ_SER.JsonPropertyOrder( 3 )]
        [G_NS.JsonProperty( Order = 3 )]
        public string HierarchicalName { get; set; } = string.Empty;

        /// <summary>
        /// Returns the value of .HierarchicalName as an HierarchicalName object.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidDataException">Thrown if the value of .HierarchicalName can not be parsed.</exception>
        public HierarchicalName GetHierarchicalName() {
            HierarchicalName hierarchicalName;
            if (Scopos.BabelFish.DataModel.Definitions.HierarchicalName.TryParse( this.HierarchicalName, out hierarchicalName )) {
                return hierarchicalName;
            }

            throw new InvalidDataException( $"Unable to parse '{HierarchicalName}' into a HierarchicalName." );
        }

        /// <summary>
        /// A human readable short name for this Definition. If no specific value
        /// is given, then the ProperName portion of the SetName is returned instead.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 5 )]
        [G_NS.JsonProperty( Order = 5 )]
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
		[G_STJ_SER.JsonPropertyOrder( 6 )]
        [G_NS.JsonProperty( Order = 6 )]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The Orion license account number of who owns, and is responsible for, this desciption. 
        /// There is often a one to one relationship between the Owner and namespace of a Definition.
        /// </summary>
        /// <example>OrionAcct001234</example>
		[G_STJ_SER.JsonPropertyOrder( 7 )]
        [G_NS.JsonProperty( Order = 7 )]
        public string Owner { get; set; } = string.Empty;

        /// <summary>
        /// The high level shooting discipline that uses this Definition.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 8 )]
        [G_NS.JsonProperty( Order = 8 )]
        [G_NS.JsonConverter( typeof( G_NS_CONV.StringEnumConverter ) )]
        public DisciplineType Discipline { get; set; } = DisciplineType.NA;

        /// <summary>
        /// The subdiscipline (under the value of Discipline) to categorize this Definition.
        /// The value of a "Subdiscipline" field may be any text string. However, there is a list of common subdisciplines and tags that should be used as appropriate.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 9 )]
        [G_NS.JsonProperty( Order = 9 )]
        [DefaultValue( "" )]
        public string Subdiscipline { get; set; } = string.Empty;

        /// <summary>
        /// The tag or tags to categorize this Definition with.
        /// The value of a "Tags" field may be any text string. However, there is a list of common subdisciplines and tags that should be used as appropriate.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 10 )]
        [G_NS.JsonProperty( Order = 10 )]
        public List<string> Tags { get; set; } = new List<string>();

        /// <summary>
        /// The Version string of the JSON document.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 98 )]
        [G_NS.JsonProperty( Order = 98 )]
        [DefaultValue( "2020-03-31" )]
		public string JSONVersion { get; set; } = "2020-03-31";

        /// <inheritdoc/>
        [G_STJ_SER.JsonPropertyOrder( 99 )]
        [G_NS.JsonProperty( Order = 99 )]
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

        [G_STJ_SER.JsonIgnore]
        [G_NS.JsonIgnore]
        public List<string> SpecificationMessages { get; protected set; }

        /// <summary>
        /// Checks to see if there is a newer minor version of the Definition file avaliable through the Rest API
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsVersionUpdateAvaliableAsync() {
            //The set name would have 0.0 or m.0 as the listed version, if asked for by the user, which is common.
            //This means the latest major or minor version avaliable for the SetName.
            //The value of .Version is the specific version number

            var currentSetName = this.GetSetName( true );

            //currentSetName may be null, if this is a new definition file and has not yet been set
            if (currentSetName == null)
                return false;

            //Check if this definition is for a specific version (e.g. v1.10) and not a most recent version (e.g. v1.0).
            //If it is for a specific version, then there can never be an update for it. 
            if (currentSetName.MinorVersion != 0)
                return false; 

            var specificVersion = this.GetDefinitionVersion();
            //Request a check of the current version stored in the Rest API
            var versionRequest = new GetDefinitionVersionPublicRequest( currentSetName, this.Type );
            var versionResponse = await DefinitionFetcher.FETCHER.GetDefinitionVersionPublicAsync( versionRequest );

            if (versionResponse.StatusCode == System.Net.HttpStatusCode.OK) {
                //The happy path
                var apiVersion = versionResponse.Value.GetDefinitionVersion();
                return specificVersion < apiVersion;
            } else if ( versionResponse.StatusCode == System.Net.HttpStatusCode.NotFound ) {
                //Likely means that this is a new Definition, that's not been uploaded before
                return false;
            } else {
                //Throw an error as something unexpected happen.
                throw new ScoposAPIException( $"Unable to complete GetDefinitionVersionPublicAsync request with status code {versionResponse.StatusCode}." );
            }
        }

        /// <summary>
        /// Returns the file name for this Definition. It should be stored in a directory named after the definition type.
        /// </summary>
        /// <returns></returns>
        public string GetFileName( bool useDevelopmentVersioning = false ) {
            if (!useDevelopmentVersioning)
                return $"{SetName.ToString().Replace( ':', ' ' )}.json";

            return $"d0.0 {HierarchicalName.ToString().Replace( ':', ' ' )}.json";
        }

        /// <summary>
        /// Helper method to save the Definition file to local storage.
        /// <para>Will save the definition file under [definitionDirectory]/DEFINITIONS/[Definition Type]/[SetName].json</para>
        /// </summary>
        /// <param name="definitionDirectory"></param>
        /// <returns>The full path of the saved file.</returns>
        public string SaveToFile( DirectoryInfo definitionDirectory ) {

            if (definitionDirectory == null)
                throw new ArgumentNullException( nameof( definitionDirectory ) );

            string filePath = $"{definitionDirectory.FullName}\\{GetRelativePath()}";

            var directoryPath = Path.GetDirectoryName( filePath );

            if (!Directory.Exists( directoryPath )) {
                Directory.CreateDirectory( directoryPath );
            }

            string json = G_NS.JsonConvert.SerializeObject( this, Helpers.SerializerOptions.NewtonsoftJsonSerializer );

            File.WriteAllText( filePath, json );

            return filePath;
        }

        /// <summary>
        /// Helper method to save the Definition to file
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns>The full path of the saved file.</returns>
        public string SaveToFile( FileInfo fileInfo ) {

            if (fileInfo == null)
                throw new ArgumentNullException( nameof( fileInfo ) );

            string json = G_NS.JsonConvert.SerializeObject( this, Helpers.SerializerOptions.NewtonsoftJsonSerializer );

            File.WriteAllText( fileInfo.FullName, json );

            return fileInfo.FullName;
        }

        /// <summary>
        /// Definitions, by commonality, are saved to file system in a standard relative directory structure. This method
        /// returns that standard relative path. 
        /// </summary>
        /// <returns></returns>
        public string GetRelativePath( ) {
            string relativePath = $"DEFINITIONS\\{Type.Description()}\\{GetFileName( false )}";
            return relativePath;
        }

        /// <summary>
        /// Returns this Definition as a json serialized string.
        /// </summary>
        /// <returns></returns>
        public string SerializeToJson() {
            string json = G_NS.JsonConvert.SerializeObject( this, Helpers.SerializerOptions.NewtonsoftJsonSerializer );

            return json;
        }

        /// <summary>
        /// Method to set default values on a new Definition. Implementation specific for each definition type.
        /// </summary>
        /// <remarks>This method should be called seperatly by the user. It is NOT called from the new Definition() method.</remarks>
        /// <returns>Returns a boolean indicating if one or more property values, within the definition, got updated.</returns>
        public virtual bool SetDefaultValues() {

            //These are common values appropirate for defaults.
            Discipline = DisciplineType.RIFLE;
            Subdiscipline = "Subdiscipline Name";
            return true;
        }

        /// <summary>
        /// Method to set default values on a new Definition. Implementation specific for each definition type.
        /// </summary>
        /// <remarks>Gets called in GetDefinitionPublciResponse.ConvertBodyToValue(). In other words, when ever a definition 
        /// is read from the REST API</remarks>
        /// <returns>Returns a boolean indicating if one or more property values, within the definition, got updated.</returns>
        public virtual bool ConvertValues() {
            //Default implementation is to do nothing.
            return false;
        }
    }
}
