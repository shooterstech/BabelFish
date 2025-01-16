using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// Format of the response object that is returned when asking for the 
    /// latest version of a Definition.
    /// </summary>
    public class SparseDefinition : BaseClass {

        /// <summary>
        /// The Definition Type
        /// </summary>
        [JsonPropertyOrder ( 1 )]
        
        public DefinitionType Type { get; set; }

        /// <summary>
        /// A SetName is a unique identifier for a Defintion file within a definition type. It has three parts, the version number, namespace, and propername.
        /// </summary>
        [JsonPropertyOrder ( 4 )]
        public string SetName { get; set; } = string.Empty;

        /// <summary>
        /// The precise version number of this Definition. Note, the version number listed in the SetName is often 
        /// a reference to either the latest Major release. Version always provides both the Major and Minor release numbers and is not a reference.
        /// </summary>
        /// <example>1.5</example>
        [JsonPropertyOrder ( 2 )]
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// If true, this Definition is no longer in use and should not be referenced.
        /// </summary>
        [JsonPropertyOrder ( 101 )]
        [DefaultValue( false )]
        public bool Discontinued { get; set; }

        /// <summary>
        /// Returns the Version of the Definition as an object DefinitionVersion.
        /// the string varation is returned with the property .Version
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown if the Version can not be parsed to format x.y</exception>
        public DefinitionVersion GetDefinitionVersion() {
            return new DefinitionVersion( Version );
        }

        /// <inheritdoc />
        public override string ToString() {
            return $"{Type}: {SetName}";
        }
    }
}
