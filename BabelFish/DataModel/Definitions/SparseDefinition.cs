using System.ComponentModel;
using NLog;
using Scopos.BabelFish.Converters.Microsoft;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// Format of the response object that is returned when asking for the 
    /// latest version of a Definition.
    /// </summary>
    public class SparseDefinition : BaseClass {

        protected Logger Logger { get; set; } = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The Definition Type
        /// </summary>
        [G_NS.JsonProperty( Order = 1, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public DefinitionType Type { get; set; }

        /// <summary>
        /// A SetName is a unique identifier for a Defintion file within a definition type. It has three parts, the version number, namespace, and propername.
        /// </summary>
        [G_NS.JsonProperty( Order = 4 )]
        public string SetName { get; set; } = string.Empty;

        /// <summary>
        /// The precise version number of this Definition. Note, the version number listed in the SetName is often 
        /// a reference to either the latest Major release. Version always provides both the Major and Minor release numbers and is not a reference.
        /// </summary>
        /// <example>1.5</example>
        [G_NS.JsonProperty( Order = 2 )]
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// The date and time this definition version was uploaded to the Rest API.
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( ScoposDateTimeConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateTimeConverter ) )]
        [G_NS.JsonProperty( Order = 118 )]
        public DateTime ModifiedAt { get; set; } = DateTime.MinValue;

		/// <summary>
		/// If true, the major version of this Definition will be marked Discontinued in the future. 
        /// <para>The author should put in .Comments additional information on when it will be deprecated. </para>
		/// </summary>
		[G_NS.JsonProperty( Order = 119 )]
		[DefaultValue( false )]
		public bool Deprecated { get; set; } = false;

		/// <summary>
		/// If true, the major version of this Definition is no longer in use and should not be referenced. Other major versions 
        /// of this same Definition may still  be in use (e.g. v1.0 vs v2.0).
		/// </summary>
		[G_NS.JsonProperty( Order = 120 )]
        [DefaultValue( false )]
        public bool Discontinued { get; set; } = false;

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
